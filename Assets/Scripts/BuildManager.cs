using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject tileShopUI;

    public GameObject turretToBuild;
    private GameObject turretOnTile;

    private Vector3 offset = new Vector3(0f, 0.2f, 0f);

    void OnMouseDown()
    {
        if (turretToBuild != null && turretOnTile == null ) { //Will need to also check money later
            BuildTurret();
        }
    }

    void BuildTurret() {
        turretOnTile = Instantiate(turretToBuild, transform.position + offset, transform.rotation);
    }

}
