using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private float speedRotate = 200f;
    [SerializeField] private float speedMove = 10f;
    [SerializeField] private GameObject touchSomething;


    public LevelManager currentlevelObject;
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
        if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
        if (other.gameObject.GetComponentInChildren<EnemiesHealth>())
        {
            currentlevelObject.AddLevel();
            Destroy(gameObject);
        }
        else
        {
            if (!other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG))
            {
                Destroy(gameObject);
            }
            else if (!other.gameObject.GetComponentInChildren<EnemiesHealth>() && !other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG))
            {
                Instantiate(touchSomething, transform.position, Quaternion.identity);
            }
        }
    }
}
