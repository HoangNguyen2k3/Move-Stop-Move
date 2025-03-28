using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] public SkinnedMeshRenderer current_Mesh;

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
    public GameObject[] fullSkinPlayObject;

    private Material begin_Material;
    private LobbyManager lobby;

    [Header("Zombie Mode")]
    public bool InZombieMode = false;
    public float num_throw_attack = 1f;
    [SerializeField] private Transform[] posStartZombie;

    public bool canRevive = true;
    public UIManager uiManager;

    [SerializeField] private TextMeshProUGUI name_text;
    [SerializeField] private Image level_bg;

    private GameObject temp_target;

    private void Start()
    {
        //   PlayerPrefs.SetFloat("Coin", 1000f);
        lobby = FindFirstObjectByType<LobbyManager>();
        begin_Material = current_Mesh.material;
        transform.localScale = new Vector3(characterPlayer.beginRange, characterPlayer.beginRange, characterPlayer.beginRange);
        animator = GetComponent<Animator>();
        circleTarget.SetActive(false);
        TakeInfoHoldWeapon();
        if (characterPlayer.fullSkinPlayer)
        {
            TakeInfoFullSkin();
        }
        else
        {
            TakeInfoCloth();
        }
        name_text.color = current_Mesh.material.color;
        level_bg.color = current_Mesh.material.color;
    }
    //Skin set up
    public void TakeInfoCloth()
    {

        current_Mesh.material = begin_Material;
        for (int i = 0; i < skinPlayerObject.Length; i++)
        {
            if (characterPlayer.skinClother[i] != null)
            {
                SettingSkin(characterPlayer.skinClother[i], i);
            }
            else
            {
                switch (i)
                {
                    case 0:
                        foreach (Transform child in skinPlayerObject[i].transform)
                        {
                            Destroy(child.gameObject);
                        }
                        break;
                    case 1:
                        skinPlayerObject[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = begin_Material;
                        break;
                    case 2:
                        goto case 0;
                    case 3: break;

                }
            }
        }
    }
    public void SettingSkin(ClotherShop skin, int index)
    {
        //if (characterPlayer.fullSkinPlayer != null) { return; }
        current_Mesh.material = begin_Material;
        SetActiveOnSmt(skinPlayerObject, true);
        SetActiveOffSmt(fullSkinPlayObject);
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
    public void SetupSkinWhenChooseFullSkin()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (characterPlayer.skinClother[i] != null)
            {
                characterPlayer.skinClother[i].status = "Purchase";
                characterPlayer.skinClother[i] = null;
            }
        }
        characterPlayer.skinClother[0] = null;
        characterPlayer.skinClother[1] = null;
        characterPlayer.skinClother[2] = null;
        foreach (Transform child in skinPlayerObject[0].transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < skinPlayerObject[1].GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
        {
            skinPlayerObject[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[i] = begin_Material;
        }

        foreach (Transform child in skinPlayerObject[2].transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void CheckPlayerCharacter()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (characterPlayer.skinClother[i] && characterPlayer.skinClother[i].status == "Purchase")
            {
                characterPlayer.skinClother[i] = null;
            }
        }
    }
    //Full skin set up
    public void SettingFullSkin(FullSkinObject skin)
    {
        SetActiveOffSmt(skinPlayerObject, true);
        SetActiveOnSmt(fullSkinPlayObject);
        fullSkinPlayObject[0].GetComponent<SkinnedMeshRenderer>().material = skin.skin;
        setupSomething(skin.accessories, 1);
        setupSomething(skin.head, 2);
        setupSomething(skin.weaponSkin, 3);
        setupSomething(skin.tail, 4);
    }
    private void setupSomething(GameObject obj, int index)
    {

        foreach (Transform child in fullSkinPlayObject[index].transform)
        {
            Destroy(child.gameObject);
        }
        if (obj == null) { return; }
        Instantiate(obj, fullSkinPlayObject[index].transform);

    }
    public void TakeInfoFullSkin()
    {
        if (characterPlayer.fullSkinPlayer)
            SettingFullSkin(characterPlayer.fullSkinPlayer);
    }
    //something misc
    public void Ahaha()
    {
        SetActiveOnSmt(skinPlayerObject, true);
        SetActiveOffSmt(fullSkinPlayObject);
    }
    private void SetActiveOffSmt(GameObject[] gameObject, bool ignore_first = false)
    {
        if (ignore_first == true) { gameObject[0].SetActive(false); }
        for (int i = 1; i < gameObject.Length; i++)
        {
            gameObject[i].SetActive(false);
        }
    }
    private void SetActiveOnSmt(GameObject[] gameObject, bool ignore_first = false)
    {
        if (ignore_first == true) { gameObject[0].SetActive(true); }
        for (int i = 1; i < gameObject.Length; i++)
        {
            gameObject[i].SetActive(true);
        }
    }
    //Weapon set up
    public void TakeInfoHoldWeapon()
    {
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
        if (isDead && !isAnimationDead)
        {
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFXSound(SoundManager.Instance.dead);
            StartCoroutine(DiePlayer());
        }
        if (isDead) { return; }
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
        yield return new WaitForSeconds(0.5f);
        /*        if (!InZombieMode)
                    cam_end.Priority = 10;*/
        if (canRevive)
        {
            uiManager.SetReviveActive();
            gameObject.SetActive(false);
            canRevive = false;

        }
        else
        {
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }
    public void RevivePlayer()
    {
        isDead = false;
        gameObject.transform.position = GameManager.Instance.GetRandomNavMeshPositionPlayer(25f);
        gameObject.SetActive(true);
        isAnimationDead = false;
        enemiesInRange.Clear();
        firstEnemy = null;
        animator.SetBool(ApplicationVariable.IS_DEAD_STATE, false);
        animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
    }
    private void Movement()
    {
        if (direct != Vector3.zero)
        {
            if (hold_weapon.activeSelf == false)
            {
                hold_weapon.SetActive(true);
            }
            animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, false);
            transform.position += direct.normalized * speed * Time.deltaTime;
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, false);
            isCoolDown = false;
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
        if (InZombieMode)
        {
            if (other.CompareTag(ApplicationVariable.ENEMY_TAG) && !isCoolDown && firstEnemy == other.transform && direct == Vector3.zero && !isDead)
            {
                StartCoroutine(Attack());
            }
            return;
        }
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG) && !isCoolDown && firstEnemy == other.transform && direct == Vector3.zero && !lobby.currentinLobby && !isDead)
        {
            StartCoroutine(Attack());
            temp_target = other.gameObject;
            /*            if (other.gameObject.GetComponentInChildren<TargetPos>())
                        {
                            temp_target = other.gameObject;
                            // ThrowWeapon(other.gameObject.GetComponentInChildren<TargetPos>().transform.position, posStart.position);
                        }*/

        }
    }

    /*    private IEnumerator Attack()
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
        }*/
    private IEnumerator Attack()
    {

        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, true);
        yield return null;
    }
    //New Attack
    public void StartAttack()
    {
        SoundManager.Instance.PlaySFXSound(SoundManager.Instance.throwWeapon);
        if (firstEnemy != null)
        {
            Vector3 directionToEnemy = firstEnemy.position - transform.position;
            directionToEnemy.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * 20);
        }
        isCoolDown = true;
    }
    public void StopAttack()
    {
        isCoolDown = false;
        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, false);
    }
    public void StartThrow()
    {
        if (temp_target && temp_target.GetComponentInChildren<TargetPos>())
        {
            ThrowWeapon(temp_target.GetComponentInChildren<TargetPos>().transform.position, posStart.position);
        }
    }
    public void ThrowWeapon(Vector3 target, Vector3 startPos)
    {


        GameObject throwWeaponPrefab = Instantiate(characterPlayer.current_Weapon.weaponThrow, startPos, Quaternion.identity);
        throwWeaponPrefab.transform.localScale += Vector3.one * addingScale;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
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
