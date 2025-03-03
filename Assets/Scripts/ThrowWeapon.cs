using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private float speedRotate = 5f;
    [SerializeField] private float speedMove = 10f;
    [SerializeField] private Vector3 direct;


    public Vector3 target;
    private void Start()
    {
        direct = direct * speedRotate;
    }
    private void Update()
    {
        transform.Rotate(direct * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target, speedMove * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
