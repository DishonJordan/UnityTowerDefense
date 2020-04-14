using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public GameObject tileShopUI;
    public static bool uiIsActive;

    private GameObject turretOnTile;

    public Material highlightColor;
    private Color originalColor;
    private Renderer myRenderer;

    private Vector3 offset = new Vector3(0f, 0.2f, 0f);

    private void Start()
    {
        uiIsActive = false;
        myRenderer = GetComponent<Renderer>();
        originalColor = myRenderer.materials[1].color;
    }

    private void OnMouseEnter()
    {
        if (!uiIsActive && turretOnTile == null)
        {
            myRenderer.materials[1].color = highlightColor.color;
        }
    }

    private void OnMouseExit()
    {
        if (!uiIsActive || turretOnTile == null)
        {
            myRenderer.materials[1].color = originalColor;
        }
    }

    private void OnMouseDown()
    {
        if (turretOnTile == null && !EventSystem.current.IsPointerOverGameObject() && !uiIsActive)
        {
            tileShopUI.SetActive(!tileShopUI.activeSelf);
            uiIsActive = tileShopUI.activeSelf;
            myRenderer.materials[1].color = originalColor;
        }
    }

    public void BuildTurret(GameObject turret) {
        Debug.Log(turret);
        //Need to check money also
        if (turret != null && turretOnTile == null){ 
            turretOnTile = (GameObject)Instantiate(turret, transform.position + offset, transform.rotation);
            tileShopUI.SetActive(false);
            uiIsActive = false;
        }
    }
}
