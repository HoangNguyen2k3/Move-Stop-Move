using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Transform root;
    public Transform unit;

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
    }
    private void Update()
    {
        if (root == null)
        {
            Destroy(gameObject);
        }
    }
    public void AddOffset(float offsetAdd)
    {
        if (unit == null) { return; }
        unit.position += new Vector3(0, offsetAdd, 0);
    }
}
