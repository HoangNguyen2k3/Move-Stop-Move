using UnityEngine;

public class TouchToObjectEnv : MonoBehaviour
{
    [SerializeField] private Material transparent_material;
    private Material begin_material;
    private bool isInCircle = false;
    public bool isTouch = true;
    private void Start()
    {
        begin_material = GetComponent<MeshRenderer>().material;
        if (!isTouch) { return; }
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
        if (!isTouch) { return; }
        if (other.CompareTag("Player") && other.isTrigger)
        {
            isInCircle = true;
            TransparentObject();
        }
    }
    /*    private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                TransparentObject();
            }
        }*/
    private void OnTriggerExit(Collider other)
    {
        if (!isTouch) { return; }
        if (other.CompareTag("Player") && other.isTrigger)
        {
            isInCircle = false;
            GetComponent<MeshRenderer>().material = begin_material;
        }
    }
    public void TransparentObject()
    {
        //if (GetComponent<MeshRenderer>().material != transparent_material)
        //{
        GetComponent<MeshRenderer>().material = transparent_material;
        //}
    }
    public void ReturnColorObject()
    {
        //if (GetComponent<MeshRenderer>().material != begin_material)
        //{
        if (!isTouch)
        {
            GetComponent<MeshRenderer>().material = begin_material;
            return;
        }
        if (!isInCircle)
        {
            GetComponent<MeshRenderer>().material = begin_material;
        }
        //}
    }
}
