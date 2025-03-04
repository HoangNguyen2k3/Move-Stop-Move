using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private float speedRotate = 200f;
    [SerializeField] private float speedMove = 10f;
    [SerializeField] private GameObject touchSomething;
    [SerializeField] private string CURRENT = "Player";
    [SerializeField] private string TARGET = "Enemy";


    public Vector3 target;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (speedRotate * Time.deltaTime), 0f);
        Vector3 newPosition = Vector3.MoveTowards(transform.position,
            new Vector3(target.x, transform.position.y, target.z), speedMove * Time.deltaTime);

        transform.position = newPosition;
        if (newPosition == target)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(CURRENT))
        {
            Destroy(gameObject);
        }
        else if (!other.gameObject.CompareTag(TARGET) && !other.gameObject.CompareTag(CURRENT))
        {
            Instantiate(touchSomething, transform.position, Quaternion.identity);
        }

    }
}
