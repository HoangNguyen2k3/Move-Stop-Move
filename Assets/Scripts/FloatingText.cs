using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Transform main_cam;
    private Transform unit;
    [SerializeField] private Transform worldSpaceCanvas;
    public Transform root;

    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        root = transform.root;
    }
    private void Start()
    {
        main_cam = Camera.main.transform;
        unit = transform.parent;
        // worldSpaceCanvas = FindFirstObjectByType<Canvas>().transform;
        transform.SetParent(worldSpaceCanvas);
    }
    private void Update()
    {
        if (root == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(transform.position - main_cam.transform.position);
            transform.position = unit.position + offset;
        }


    }
}
