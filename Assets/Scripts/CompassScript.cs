using UnityEngine;

public class CompassScript : MonoBehaviour
{
    public RectTransform arrow; 
    public Transform player; 
    private Transform goal; 
    
    void Start()
    {
        GameObject goalObject = GameObject.FindGameObjectWithTag("Goal");
        if (goalObject != null)
        {
            goal = goalObject.transform;
        }
        else
        {
            Debug.LogError("Сундук не найден! Убедитесь, что объект имеет тег 'Goal'");
        }
    }

    void Update()
    {
        Vector3 d = goal.position - player.position;
        Vector3 f = Camera.main.transform.forward;
        d.y = 0f;
        f.y = 0f;
        float compasAngle = Vector3.SignedAngle(f, d, Vector3.down);
        arrow.eulerAngles = new Vector3(0, 0, compasAngle);
    }
}
