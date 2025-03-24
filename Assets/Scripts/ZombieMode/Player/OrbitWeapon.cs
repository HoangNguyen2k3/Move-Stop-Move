using UnityEngine;

public class OrbitWeapon : MonoBehaviour
{
    [SerializeField] private WeaponObject weapon;
    [SerializeField] private Transform player;
    [SerializeField] private LevelManager currentlevelObject;
    public float orbitRadius = 5f;
    [SerializeField] private float orbitSpeed = 50f;
    [SerializeField] private float heightOffset = 0.5f;

    private float angle;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Ch?a gán Player!");
            return;
        }
        if (!weapon.isTurning)
        {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
            // transform.Rotate(Vector3.up * (weapon.speedRotate * Time.deltaTime));
        }
        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void Update()
    {
        if (player == null) return;

        RotateAroundPlayer();
        SelfRotate();
    }

    private void RotateAroundPlayer()
    {
        angle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
        float y = player.position.y + heightOffset;

        transform.position = new Vector3(player.position.x + x, y, player.position.z + z);
    }

    private void SelfRotate()
    {
        if (weapon.isTurning)
        {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
            // transform.Rotate(Vector3.up * (weapon.speedRotate * Time.deltaTime));
        }
    }
    public void SetUp(float range, WeaponObject weapon)
    {
        this.weapon = weapon;
        GetComponent<MeshFilter>().mesh = weapon.weaponThrow.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshRenderer>().materials = (weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials);
        this.orbitRadius = range;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ThrowWeapon>()) { return; }
        if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
        if (other.gameObject.GetComponentInChildren<EnemiesHealth>())
        {
            currentlevelObject.AddLevel();
        }
    }
}
