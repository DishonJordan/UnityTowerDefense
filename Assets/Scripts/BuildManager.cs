﻿using UnityEngine;
using UnityEngine.Assertions;
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
    [HideInInspector]
    public GameObject mechanicIcon;

    private GameObject turretOnTile;
    private Color originalColor;
    private Renderer myRenderer;
    private Vector3 offset = new Vector3(0f, 0.2f, 0f);
    private ShopUIController controller;

    private void Start()
    {
        shopUIActive = false;
        myRenderer = GetComponent<Renderer>();
        originalColor = myRenderer.materials[1].color;
        taskInProgress = false;
        controller = transform.GetChild(0).GetComponentInChildren<ShopUIController>();
        mechanicIcon = transform.GetChild(1).gameObject;
    }

    /* Changes the color of tile to show it is clickable */
    private void OnMouseEnter()
    {
        if (!shopUIActive && turretOnTile == null && !taskInProgress)
        {
            myRenderer.materials[1].color = highlightColor.color;
        }
    }

    /* Reverts the color of the Tile */
    private void OnMouseExit()
    {
        if (!shopUIActive || turretOnTile == null)
        {
            if (!taskInProgress)
            {
                myRenderer.materials[1].color = originalColor;
            }
        }
    }

    /* Activates the shop UI */
    private void OnMouseDown()
    {
        if (turretOnTile == null && !EventSystem.current.IsPointerOverGameObject()
            && !shopUIActive && !Turret.turretUIActive && !taskInProgress)
        {
            EnableShopUI();
        }
    }

    /* Requests that the Mechanic Manager builds the tower */
    public void RequestBuild(GameObject turret)
    {
        Turret t = turret.GetComponentInChildren<Turret>();
        Assert.IsNotNull(turret, "Build Manager Could Not Find Turret");

        if (turret != null && turretOnTile == null && Bank.instance.WithdrawMoney(t.purchaseCost))
        {
            Task task = new Task(transform.position, Task.Type.Build, this, turret, t.TurretSprite, t.purchaseCost);
            MechanicManager.instance.AddTask(task);

            turretShopUI.SetActive(false);
            shopUIActive = false;

            taskInProgress = true;
            SetTileToPendingColor(true);
            mechanicIcon.SetActive(true);
            controller.ChangeButtonInteractivity(false);
        }
    }

    /* This is called by the onclick event of the turretShopUI turret button */
    public void BuildTurret(GameObject turret)
    {
        audioSource.PlayOneShot(buildSound);
        turretOnTile = Instantiate(turret, transform.position + offset, transform.rotation);

        Turret turretScript = turretOnTile.GetComponentInChildren<Turret>();
        turretScript.SetBuildManager(this);

        turretShopUI.SetActive(false);
        shopUIActive = false;

        taskInProgress = false;
        controller.ChangeButtonInteractivity(true);
        SetTileToPendingColor(false);
        mechanicIcon.SetActive(false);
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

        if (!taskInProgress)
        {
            myRenderer.materials[1].color = originalColor;
        }
    }

    /* Disables the shop UI */
    public void DisableShopUI()
    {
        audioSource.PlayOneShot(closeSound);
        turretShopUI.SetActive(false);
        shopUIActive = false;
    }

    /* Sets the tile to the pending*/
    public void SetTileToPendingColor(bool b)
    {
        myRenderer.materials[1].color = (b) ? pendingColor.color : originalColor;
    }

    /* Undoes the Pending Task Highlight and State */
    public void UndoPendingTask()
    {
        taskInProgress = false;
        controller.ChangeButtonInteractivity(true);
        SetTileToPendingColor(false);
        mechanicIcon.SetActive(false);
    }
}
