using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    /* Ensures only one TileUI is active at once */
    public static bool shopUIActive;

    [Header("UI Elements")]
    public GameObject turretShopUI;

    [Header("Materials")]
    public Material highlightColor;
    public Material pendingColor;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip buildSound;
    public AudioClip upgradeSound;

    [HideInInspector]
    public bool taskInProgress;

    private GameObject mechanicIcon;
    private GameObject turretOnTile;
    private Color originalColor;
    private Renderer myRenderer;
    private Vector3 groundOffset = new Vector3(0f, 0.2f, 0f);

    private void Start()
    {
        shopUIActive = false;
        taskInProgress = false;

        myRenderer = GetComponent<Renderer>();
        originalColor = myRenderer.materials[1].color;
        mechanicIcon = transform.GetChild(1).gameObject;
    }

    /* Changes the color of tile to show it is clickable */
    private void OnMouseEnter()
    {
        if (!shopUIActive && !taskInProgress && turretOnTile == null)
        {
            SetTileToHighlightColor(true);
        }
    }

    /* Reverts the color of the Tile */
    private void OnMouseExit()
    {
        if ((!shopUIActive && turretOnTile == null) && !taskInProgress)
        {
            SetTileToHighlightColor(false);
        }
    }

    /* Activates the shop UI */
    private void OnMouseDown()
    {
        if (turretOnTile == null && !EventSystem.current.IsPointerOverGameObject()
            && !shopUIActive && !Turret.turretUIActive && !taskInProgress)
        {
            EnableShopUI();
            SetTileToHighlightColor(false);
        }
    }

    /* Requests that the Mechanic Manager builds the tower */
    public void RequestBuild(GameObject turret)
    {
        Turret t = turret.GetComponentInChildren<Turret>();

        if (turret != null && turretOnTile == null && Bank.instance.WithdrawMoney(t.purchaseCost))
        {
            Task task = new BuildTask(this, turret);
            MechanicManager.instance.AddTask(task);

            DisableShopUI();
            SetTaskActive(true);
        }
    }

    /* This is called by the onclick event of the turretShopUI turret button */
    public void BuildTurret(GameObject turret)
    {
        audioSource.PlayOneShot(buildSound);
        turretOnTile = Instantiate(turret, transform.position + groundOffset, transform.rotation);

        Turret turretScript = turretOnTile.GetComponentInChildren<Turret>();
        turretScript.SetBuildManager(this);

        DisableShopUI();
        SetTaskActive(false);
    }

    /* Replaces the turret on the tile with a new one */
    public void ReplaceTurret(GameObject turret)
    {
        if (turret != null)
        {
            audioSource.PlayOneShot(upgradeSound);
            turretOnTile = null;

            BuildTurret(turret);
        }
    }

    /* Enables the shop UI */
    public void EnableShopUI()
    {
        audioSource.PlayOneShot(openSound);
        turretShopUI.SetActive(true);
        shopUIActive = true;
    }

    /* Disables the shop UI */
    public void DisableShopUI()
    {
        audioSource.PlayOneShot(closeSound);
        turretShopUI.SetActive(false);
        shopUIActive = false;
    }

    /* Sets the tile to the pending highlight color*/
    public void SetTileToPendingColor(bool b)
    {
        myRenderer.materials[1].color = (b) ? pendingColor.color : originalColor;
    }

    /* Sets the tile to the hover highlight color */
    public void SetTileToHighlightColor(bool b)
    {
        myRenderer.materials[1].color = (b) ? highlightColor.color : originalColor;
    }

    /* Sets the task related fields based on whether the task is active or not */
    public void SetTaskActive(bool b)
    {
        taskInProgress = b;
        SetTileToPendingColor(b);
        mechanicIcon.SetActive(b);
    }
}
