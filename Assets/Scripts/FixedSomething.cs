using UnityEngine;

public class FixedSomething : MonoBehaviour
{
    [SerializeField] private bool isFixRotation = false;
    Vector3 rotation_begin = new Vector3(0f, 0f, 0f);
    private void Start()
    {

    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(rotation_begin);
    }
}
