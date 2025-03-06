using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private float speedRotate = 200f;
    [SerializeField] private float speedMove = 10f;
    [SerializeField] private GameObject touchSomething;
    [SerializeField] private bool isTurning = true;

    [HideInInspector] public GameObject who_throw_obj;
    [HideInInspector] public string who_throw = "Player";
    [HideInInspector] public LevelManager currentlevelObject;
    [HideInInspector] public Vector3 target;
    private void Start()
    {
        if (!isTurning)
        {
            transform.LookAt(target);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
    private void Update()
    {
        if (currentlevelObject == null) { Destroy(gameObject); }
        Vector3 newPosition = FindNewPosition();
        transform.position = newPosition;
        if (newPosition == target)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 FindNewPosition()
    {
        if (isTurning)
        {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (speedRotate * Time.deltaTime), 0f);
        }
        Vector3 newPosition = Vector3.MoveTowards(transform.position,
                   new Vector3(target.x, target.y, target.z), speedMove * Time.deltaTime);
        return newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ThrowWeapon>()) { return; }
        if (who_throw == ApplicationVariable.PLAYER_TAG)
        {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<EnemiesHealth>())
            {
                currentlevelObject.AddLevel();
                //                gameManager.MinusEnemy();
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
            //Instantiate(touchSomething, transform.position, Quaternion.identity);
        }
        else
        {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<PlayerController>())
            {
                if (other.isTrigger) { return; }
                currentlevelObject.AddLevel();
                other.gameObject.GetComponentInChildren<PlayerController>().isDead = true;
                Destroy(gameObject);
            }
            else if (other.gameObject != who_throw_obj && other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG) && who_throw_obj != null)
            {
                other.gameObject.GetComponent<EnemiesHealth>().isAlive = false;
                currentlevelObject.AddLevel();
                //               gameManager.MinusEnemy();
                other.gameObject.GetComponent<EnemiesHealth>().Die();
                Destroy(gameObject);
            }
            else
            {
                /*                if (!other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG))
                                {
                                    Destroy(gameObject);
                                }*/
                if (!other.gameObject.GetComponentInChildren<PlayerController>() && !other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG))
                {
                    Instantiate(touchSomething, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

    }
}
