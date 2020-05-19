using UnityEngine;

public class GrowAndShrink : MonoBehaviour
{
    [Header("Properties")]
    public float scaleSize = 1.0f;
    public float scaleSpeed = 1.0f;


    private void Update()
    {
        transform.localScale = new Vector3(
            Mathf.PingPong(Time.time * scaleSpeed, scaleSize) + scaleSize,
            Mathf.PingPong(Time.time * scaleSpeed, scaleSize) + scaleSize, 0
        );
    }
}
