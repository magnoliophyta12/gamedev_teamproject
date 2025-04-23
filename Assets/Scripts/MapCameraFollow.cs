using UnityEngine;

public class MapCameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0, 50, 0); 

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
