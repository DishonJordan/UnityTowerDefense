using UnityEngine;

public class PointTowardsCamera : MonoBehaviour
{
    public static GameObject mainCamera;
    private readonly float turnRate = 9f;

    private void Start()
    {
        /* Only called once by all tiles */
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");
        }
    }

    void Update()
    {
        Vector3 direction = mainCamera.transform.position - transform.position;
        Quaternion qRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, qRotation, Time.deltaTime * turnRate).eulerAngles;

        /* Rotates the ui about the y axis*/
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
