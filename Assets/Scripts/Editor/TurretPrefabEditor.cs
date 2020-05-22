using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class TurretPrefabEditor
{
    private const string TURRET_FOLDER = "Assets/Prefabs/Turrets";
    private const string VFX_PREFAB = "Assets/Prefabs/Effects/Turret Effects.prefab";

    // The return value indicates if you want to save prefab changes
    public delegate bool PrefabFunc(GameObject prefab);


    [MenuItem("Tools/Turret Prefabs/Reset Position")]
    public static void ResetPosition()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            if(turretPrefab.transform.position != Vector3.zero)
            {
                turretPrefab.transform.position = Vector3.zero;
                return true;
            }
            return false;
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Reset position to (0,0,0)");
    }

    [MenuItem("Tools/Turret Prefabs/Add Animation Support")]
    public static void UpdateHierarchyToSupportAnim()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            return ModifyTurretPrefab(turretPrefab);
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Updated hierarchy to support animation.");
    }

    [MenuItem("Tools/Turret Prefabs/Turret Effects/Add")]
    public static void AddVFX()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            return AddVFXToTurret(turretPrefab);
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Added VFX.");
    }

    [MenuItem("Tools/Turret Prefabs/Turret Effects/Remove")]
    public static void RemoveVFX()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            TurretEffects effects = turretPrefab.GetComponentInChildren<TurretEffects>(true);
            if (!effects)
                return false;

            UnityEngine.Object.DestroyImmediate(effects.gameObject);
            return true;
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Added VFX.");
    }

    [MenuItem("Tools/Turret Prefabs/Set Health to Max Health")]
    public static void MaxOutHealth()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            Turret turret = turretPrefab.GetComponentInChildren<Turret>();
            if (!turret)
                return false;

            turret.health = turret.maxHealth;
            return true;
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Set health to max health.");
    }

    [MenuItem("Tools/Turret Prefabs/Connect TurretUI to Turret")]
    public static void ConnectToTurretUI()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            Turret turret = turretPrefab.GetComponentInChildren<Turret>();
            if (!turret)
                return false;

            TurretUIController controller = turretPrefab.GetComponentInChildren<TurretUIController>();
            if (!controller)
                return false;

            controller.turret = turret.gameObject;
            return true;
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Connected TurretUI to Turret.");
    }

    [MenuItem("Tools/Turret Prefabs/Connect Turret Health Bar to Turret")]
    public static void ConnectToHealthBar()
    {
        int modified = ForeachPrefabInFolder(TURRET_FOLDER, (turretPrefab) =>
        {
            Turret turret = turretPrefab.GetComponentInChildren<Turret>();
            if (!turret)
                return false;

            TurretHealthBar healthBar = turretPrefab.GetComponentInChildren<TurretHealthBar>();
            if (!healthBar)
                return false;

            healthBar.turret = turret;
            return true;
        });
        Debug.Log("Modified " + modified + " turret prefabs: " + "Connected health bar to Turret.");
    }

    private static bool AddVFXToTurret(GameObject turretPrefab)
    {
        Turret turret = turretPrefab.GetComponentInChildren<Turret>();
        if (!turret)
            return false;

        TurretEffects effects = turretPrefab.GetComponentInChildren<TurretEffects>();
        if(!effects)
        {
            GameObject VFXObj = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(VFX_PREFAB), turretPrefab.transform);
            VFXObj.transform.localPosition = Vector3.zero;
            effects = VFXObj.GetComponentInChildren<TurretEffects>(true);
            if (!effects)
                return false;
        }

        effects.turret = turret;
        return true;
    }

    private static bool ModificationNeeded(GameObject turretPrefab)
    {
        // If the Turret script and MeshRenderer are on the same object, modification is needed
        Turret turret = turretPrefab.GetComponentInChildren<Turret>();
        if (turret == null)
            return false;

        MeshRenderer mrend = turret.GetComponent<MeshRenderer>();
        if (mrend == null)
            return false;

        return true;
    }

    private static bool ModifyTurretPrefab(GameObject turretPrefab)
    {
        if (!ModificationNeeded(turretPrefab))
            return false;

        // Reset parent transform
        turretPrefab.transform.position = Vector3.zero;

        // If the turret script is attached to an object with a mesh renderer, we must move it
        Turret turret = turretPrefab.GetComponentInChildren<Turret>();
        GameObject oldTurretGameObject = turret.gameObject;
        MeshRenderer mrend = turret.GetComponent<MeshRenderer>();

        // Create new parent object for turret visual
        GameObject turretParent = new GameObject("Turret Parent");
        turretParent.transform.parent = turretPrefab.transform;
        turret.transform.parent = turretParent.transform;

        // Move components from turret to turret parent
        AudioSource a = MoveComponent(turret.GetComponent<AudioSource>(), turretParent);

        HandleMoveBoxCollider(turret, turretParent);

        turret = MoveComponent(turret.GetComponent<Turret>(), turretParent);

        // Update turret script references
        turret.audioSource = a;

        // Special case for Ballista Turret
        if(turret is BallistaTurret t)
        {
            t.arrow = oldTurretGameObject.transform.Find("bow/arrow").gameObject;
        }

        if (turret.GetComponent<Animator>() == null)
        {
            Animator turretAnim = turretParent.AddComponent<Animator>();
            turretAnim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/TurretAnimator.controller");
        }

        // Update turret ui references
        TurretUIController turretUIController = turretPrefab.GetComponentInChildren<TurretUIController>(true);
        if (turretUIController != null)
        {
            turretUIController.turret = turret.gameObject;
        }

        return true;
    }

    private static void HandleMoveBoxCollider(Turret turret, GameObject turretParent)
    {
        BoxCollider b = MoveComponent(turret.GetComponent<BoxCollider>(), turretParent);
        b.size *= turret.transform.localScale.x;
        b.center *= turret.transform.localScale.x;
    }

    private static T MoveComponent<T>(T source, GameObject targetObj) where T: Component
    {
        if (source != null)
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(source);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetObj);
            UnityEngine.Object.DestroyImmediate(source);
        }
        return targetObj.GetComponent<T>();
    }

    private static int ForeachPrefabInFolder(string path, PrefabFunc func)
    {
        string[] guids = AssetDatabase.FindAssets("t:GameObject", new string[] { path });
        int modified = 0;
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject turretPrefab = PrefabUtility.LoadPrefabContents(assetPath);
            if (turretPrefab != null)
            {
                if (func(turretPrefab))
                {
                    PrefabUtility.SaveAsPrefabAsset(turretPrefab, assetPath);
                    modified++;
                }
                PrefabUtility.UnloadPrefabContents(turretPrefab);
            }
        }
        return modified;
    }
}
