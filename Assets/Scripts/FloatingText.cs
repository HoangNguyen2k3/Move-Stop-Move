using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private Transform worldSpaceCanvas;
    [SerializeField] private Vector3 offset;

    public Transform root;
    public LevelManager levelManager;
    private Transform main_cam;
    private Transform unit;
    private float current_add_offset;

    private void Awake()
    {

        if (GetComponentInParent<EnemiesHealth>())
        {
            root = GetComponentInParent<EnemiesHealth>().transform;
        }
        else
        {
            root = GetComponentInParent<PlayerController>().transform;
        }
        levelManager = GetComponentInParent<LevelManager>();
    }
    private void Start()
    {
        if (worldSpaceCanvas == null)
        {
            worldSpaceCanvas = GameObject.FindGameObjectWithTag("CanvasWorldSpace").transform;
        }
        main_cam = Camera.main.transform;
        unit = transform.parent;
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
            if (current_add_offset != levelManager.offset_floatingtext)
            {
                current_add_offset = levelManager.offset_floatingtext;
                AddOffset(current_add_offset);
            }
            transform.rotation = Quaternion.LookRotation(transform.position - main_cam.transform.position);
            transform.position = unit.position + offset;
        }


    }
    public void AddOffset(float offsetAdd)
    {
        offset += new Vector3(0, offsetAdd, 0);
    }
}
