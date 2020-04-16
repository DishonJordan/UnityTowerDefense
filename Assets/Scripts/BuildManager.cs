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

    [Header("Bank")]
    public Bank bank;

    private GameObject turretOnTile;
    private Color originalColor;
    private Renderer myRenderer;
    private Vector3 offset = new Vector3(0f, 0.2f, 0f);

    private void Start()
    {
        shopUIActive = false;
        myRenderer = GetComponent<Renderer>();
        originalColor = myRenderer.materials[1].color;
    }

    /* Changes the color of tile to show it is clickable */
    private void OnMouseEnter()
    {
        if (!shopUIActive && turretOnTile == null)
        {
            myRenderer.materials[1].color = highlightColor.color;
        }
    }

    /* Reverts the color of the Tile */
    private void OnMouseExit()
    {
        if (!shopUIActive || turretOnTile == null)
        {
            myRenderer.materials[1].color = originalColor;
        }
    }

    /* Activates the shop UI */
    private void OnMouseDown()
    {
        if (turretOnTile == null && !EventSystem.current.IsPointerOverGameObject() && !shopUIActive)
        {
            EnableShopUI();
        }
    }

    /* This is called by the onclick event of the turretShopUI turret button */
    public void BuildTurret(GameObject turret)
    {
        Turret t = turret.GetComponent<Turret>();

        if (turret != null && turretOnTile == null && bank.WithdrawMoney(t.purchaseCost)) 
        {
            turretOnTile = Instantiate(turret, transform.position + offset, transform.rotation);
            turretShopUI.SetActive(false);
            shopUIActive = false;
        }
    }

    /* Enables the shop UI */
    public void EnableShopUI()
    {
        turretShopUI.SetActive(true);
        shopUIActive = turretShopUI.activeSelf;
        myRenderer.materials[1].color = originalColor;
    }

    /* Disables the shop UI */
    public void DisableShopUI()
    {
        turretShopUI.SetActive(false);
        shopUIActive = turretShopUI.activeSelf;
    }
}
