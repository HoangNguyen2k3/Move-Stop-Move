using UnityEngine;

public class FixedSomething : MonoBehaviour
{
    [SerializeField] private bool inZombieMode = false;
    Vector3 rotation_begin = new Vector3(0f, 0f, 0f);
    private void Start()
    {
    }
    void LateUpdate()
    {
        if (inZombieMode)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.Euler(rotation_begin);
        }


    }
}
