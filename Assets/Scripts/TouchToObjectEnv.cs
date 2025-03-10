using UnityEngine;

public class TouchToObjectEnv : MonoBehaviour
{
    [SerializeField] private Material transparent_material;
    private Material begin_material;
    private void Start()
    {
        begin_material = GetComponent<MeshRenderer>().material;
    }

    /*    private void OnCollisionEnter(Collision collision)
        {
            GetComponent<MeshRenderer>().material = transparent_material;
        }
        private void OnCollisionExit(Collision collision)
        {
            GetComponent<MeshRenderer>().material = begin_material;
        }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().material = transparent_material;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().material = begin_material;
        }
    }
}
