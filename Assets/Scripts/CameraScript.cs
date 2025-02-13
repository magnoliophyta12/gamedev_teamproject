using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraAnchor;
    private Vector3 cameraOffset;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    private float sensitivityH = 4.0f;
    private float sensitivityV = 3.0f;
    private float minVAngle = -45.0f;
    private float maxVAngle = 75.0f;

    void Start()
    {
        cameraOffset = this.transform.position - cameraAnchor.position;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();
        cameraAngles.x = Mathf.Clamp(
            cameraAngles.x - lookValue.y * sensitivityV,
            minVAngle, maxVAngle);
        cameraAngles.y += lookValue.x * sensitivityH;
        if (cameraAngles.y > 360.0)
        {
            cameraAngles.y -= 360.0f;
        }
        if (cameraAngles.y < -360.0)
        {
            cameraAngles.y += 360.0f;
        }
    }

    void LateUpdate()
    {
        this.transform.eulerAngles = cameraAngles;
        this.transform.position = cameraAnchor.position +
            Quaternion.Euler(cameraAngles) * cameraOffset;
    }
}
