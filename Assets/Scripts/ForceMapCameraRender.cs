using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ForceMapCameraRender : MonoBehaviour
{
    private Camera mapCamera;

    void Awake()
    {
        mapCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (mapCamera != null && mapCamera.enabled)
        {
            mapCamera.Render();
        }
    }
}
