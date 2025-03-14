using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float angle = 5f;
    [SerializeField] private Joystick my_joyStick;
    [SerializeField] private GameObject hold_weapon;
    [SerializeField] private Transform posStart;
    [SerializeField] private GameObject circleTarget;
    [SerializeField] private CinemachineCamera cam_end;

    public CharaterObj characterPlayer;

    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] private SkinnedMeshRenderer current_Mesh;

    public float addingScale = 0;
    private bool isCoolDown = false;
    private bool isEnemyInRange = false;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 direct;
    private Transform firstEnemy = null;
    private List<Transform> enemiesInRange = new List<Transform>();

    public bool isDead = false;
    private bool isAnimationDead = false;
    [HideInInspector] public bool isWinning = false;


    public GameObject[] skinPlayerObject;

    private void Start()
    {
        transform.localScale = new Vector3(characterPlayer.beginRange, characterPlayer.beginRange, characterPlayer.beginRange);
        animator = GetComponent<Animator>();
        circleTarget.SetActive(false);
        TakeInfoHoldWeapon();
        TakeInfoCloth();
    }
    public void TakeInfoCloth()
    {
        for (int i = 0; i < skinPlayerObject.Length; i++)
        {
            if (characterPlayer.skinClother[i] != null)
            {
                SettingSkin(characterPlayer.skinClother[i], i);
            }
        }
    }
    public void SettingSkin(ClotherShop skin, int index)
    {
        switch (index)
        {
            case 0:
                foreach (Transform child in skinPlayerObject[index].transform)
                {
                    Destroy(child.gameObject);
                }
                Instantiate(skin.skin, skinPlayerObject[index].transform);
                break;
            case 1:
                skinPlayerObject[index].GetComponent<SkinnedMeshRenderer>().materials = skin.skin.GetComponentInChildren<MeshRenderer>().sharedMaterials;
                break;
            case 2:
                goto case 0;
            case 3: break;

        }
    }
    /*    public void TakeColorWeaponFromDatabase()
        {
            if (!PlayerPrefs.HasKey("EquipCurrentWeapon")) { return; }
            string name_weapon = PlayerPrefs.GetString("EquipCurrentWeapon");
            for (int i = 0; i < characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
            {
                string key = "Color_" + name_weapon + "_custom_" + i;
                string hexColor = PlayerPrefs.GetString(key);

                if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
                {
                    characterPlayer.skin_current_weapon.material[i].color = newColor;
                }
            }

        }*/
    public void TakeInfoHoldWeapon()
    {
        //TakeColorWeaponFromDatabase();
        for (int i = 0; i < characterPlayer.current_Weapon.weaponHold.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
        {
            characterPlayer.current_Weapon.weaponHold.GetComponent<MeshRenderer>().sharedMaterials[i].color = characterPlayer.skin_current_weapon.material[i].color;
            characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials[i].color = characterPlayer.skin_current_weapon.material[i].color;

        }
        hold_weapon.GetComponent<MeshFilter>().mesh = characterPlayer.current_Weapon.weaponHold.GetComponent<MeshFilter>().sharedMesh;
        hold_weapon.GetComponent<MeshRenderer>().materials = characterPlayer.current_Weapon.weaponHold.GetComponent<MeshRenderer>().sharedMaterials;
    }
    private void Update()
    {
        if (isWinning)
        {
            return;
        }
        if (isDead && !isAnimationDead) { StartCoroutine(DiePlayer()); }
        direct.x = my_joyStick.Horizontal;
        direct.z = my_joyStick.Vertical;
        Movement();
        RotateCharacter();
        if (isEnemyInRange)
        {
            if (firstEnemy)
            {
                circleTarget.transform.position = firstEnemy.position;
            }

        }
    }

    private void FixedUpdate()
    {
        if (isWinning)
        {
            return;
        }
        //       Movement();
    }
    private IEnumerator DiePlayer()
    {
        ParticleSystem temp = Instantiate(take_damage_FX, transform.position, Quaternion.identity);
        temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
        isAnimationDead = true;
        animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
        yield return new WaitForSeconds(0.3f);
        cam_end.Priority = 10;
        Destroy(gameObject);
    }
    private void Movement()
    {
        if (direct != Vector3.zero)
        {
            transform.position += direct.normalized * speed * Time.deltaTime;
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, false);
        }
        else
        {
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
        }
    }

    private void RotateCharacter()
    {
        if (direct == Vector3.zero)
            return;
        Quaternion rot = Quaternion.LookRotation(direct.normalized);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot, angle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG))
        {
            Transform enemy = other.transform;
            if (!enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);

            if (firstEnemy == null)
            {
                firstEnemy = enemy;
                UpdateCircleTarget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG))
        {
            Transform enemy = other.transform;
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy)
            {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }

    private void UpdateCircleTarget()
    {
        if (firstEnemy != null)
        {
            if (firstEnemy.GetComponent<EnemiesHealth>().isAlive == false)
            {
                isEnemyInRange = false;
                circleTarget.SetActive(false);
                return;
            }
            circleTarget.SetActive(true);
            isEnemyInRange = true;
            circleTarget.transform.position = firstEnemy.position;
        }
        else
        {
            isEnemyInRange = false;
            circleTarget.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG) && !isCoolDown && firstEnemy == other.transform && direct == Vector3.zero)
        {
            StartCoroutine(Attack());
            if (other.gameObject.GetComponentInChildren<TargetPos>())
            {
                ThrowWeapon(other.gameObject.GetComponentInChildren<TargetPos>().transform.position);
            }
        }
    }

    private IEnumerator Attack()
    {
        isCoolDown = true;
        hold_weapon.SetActive(false);
        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, true);
        yield return new WaitForSeconds(characterPlayer.coolDownAttack / 5);
        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, false);
        yield return new WaitForSeconds(characterPlayer.coolDownAttack / 2);
        hold_weapon.SetActive(true);
        yield return new WaitForSeconds(characterPlayer.coolDownAttack / 2);
        isCoolDown = false;
    }

    public void ThrowWeapon(Vector3 target)
    {
        if (firstEnemy != null)
        {
            Vector3 directionToEnemy = firstEnemy.position - transform.position;
            directionToEnemy.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * 10);
        }

        GameObject throwWeaponPrefab = Instantiate(characterPlayer.current_Weapon.weaponThrow, posStart.position, Quaternion.identity);
        throwWeaponPrefab.transform.localScale += Vector3.one * addingScale;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        //  target.y = posStart.position.y;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
        // throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target.GetComponentInChildren<TargetPos>().transform.position;
    }
    public void RemoveEnemyFromList(Transform enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy)
            {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }
    private void OnDestroy()
    {
        Destroy(circleTarget);
    }
}
