using UnityEngine;

public class CameraController : MonoBehaviour {
    private bool movementAllowed = true;
    public float panSpeed = 3f;
    public float panBorderThickness = 1f;
    public float scrollSpeed = 1f;
    public float minHeight = 5f;
    public float maxHeight = 10f;
    public float maxLeft = 1f;
    public float maxRight = 4f;
    public float maxForward = 3f;
    public float maxBack = 7f;

    // Update is called once per frame
    void Update() {

        if(GameManager.gameIsOver)
        {
            this.movementAllowed = false;
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            movementAllowed = !movementAllowed;

        if(!movementAllowed)
            return;

        if(transform.position.x >= maxForward && (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if(transform.position.x <= maxBack && (Input.GetKey("s") ||  Input.mousePosition.y <= panBorderThickness))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if(transform.position.z >= maxLeft && (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if(transform.position.z <= maxRight && (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 100;
        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);

        transform.position = pos;


    }
}
