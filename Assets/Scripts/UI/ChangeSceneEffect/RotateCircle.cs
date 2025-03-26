using UnityEngine;

public class RotateCircle : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
