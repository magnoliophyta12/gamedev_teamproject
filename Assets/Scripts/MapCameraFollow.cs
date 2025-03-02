using UnityEngine;

public class MapCameraFollow : MonoBehaviour
{
    public Transform player; // Игрок
    public Vector3 offset = new Vector3(0, 50, 0); // Высота камеры

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
