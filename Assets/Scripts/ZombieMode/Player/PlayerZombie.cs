using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerZombie : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] private float angle = 5f;
    [SerializeField] private Joystick my_joyStick;
    [SerializeField] private GameObject hold_weapon;
    [SerializeField] private Transform posStart;
    [SerializeField] private GameObject circleTarget;
    public CharaterObj characterPlayer;
    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] public SkinnedMeshRenderer current_Mesh;
    public float addingScale = 0;
    private bool isCoolDown = false;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 direct;
    public bool isDead = false;
    private bool isAnimationDead = false;
    [HideInInspector] public bool isWinning = false;
    public GameObject[] skinPlayerObject;
    public GameObject[] fullSkinPlayObject;

    private Material begin_Material;

    [Header("Zombie Mode")]
    public float num_throw_attack = 1f;
    private float angle_attack = 15f;
    public CircleRange range;
    public int num_alive_player = 0;
    public bool canAttack = true;
    public GameObject[] shields;
    public PermParamAdd permParam;
    public GameObject shieldObj;
    public Vector3 currentPosAttack;
    private Rigidbody rb;

    [Header("-------Abilities Temp-------")]
    public int num_choose = 0;
    public GameObject orbitWeapon;
    private void OnEnable()
    {
        InitPlayerAbilities();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        //       OrbitWeapon();
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
        }
    }
    public void SettingSkin(ClotherShop skin, int index)
    {
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
        if (isDead && !isAnimationDead) { StartCoroutine(DiePlayer()); }
        if (isDead) { return; }
        direct.x = my_joyStick.Horizontal;
        direct.z = my_joyStick.Vertical;
        Movement();
        RotateCharacter();
    }
    private IEnumerator DiePlayer()
    {
        ParticleSystem temp = Instantiate(take_damage_FX, transform.position, Quaternion.identity);
        temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
        isAnimationDead = true;
        animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void Movement()
    {
        if (direct != Vector3.zero)
        {
            rb.MovePosition(transform.position + direct.normalized * speed * Time.deltaTime);
            // transform.position += direct.normalized * speed * Time.deltaTime;
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

    public void AttackEnemy(GameObject other)
    {
        if (!isCoolDown && direct == Vector3.zero && !isDead)
        {
            switch (num_choose)
            {
                case 0: BaseAttack(other); break;
                case 1: BehindAttack(other); break;
                /*                case 2: OrbitWeapon(); break;*/
                case 3: BulletPlus(other); break;
                case 4: ChaseAttack(other); break;
                case 5: ContinuousAttack(other); break;
                case 6: CrossAttack(other); break;
                case 7: DiagonAttack(other); break;
                case 8: break;
                case 9: GrowingAttack(other); break;
                case 14: TripleAttack(other); break;
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
    public void ThrowWeapon(Vector3 target, Vector3 startPos, Vector3 dir, Transform target_trans = null, bool upscale = false)
    {
        if (range.firstEnemy != null)
        {
            Vector3 directionToEnemy = range.firstEnemy.position - transform.position;
            directionToEnemy.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * 10);
        }
        startPos.y = transform.position.y + 0.6f;
        GameObject throwWeaponPrefab = Instantiate(characterPlayer.current_Weapon.weaponThrow, startPos, Quaternion.identity);
        throwWeaponPrefab.transform.localScale += Vector3.one * addingScale;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().dir = dir.normalized;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().isZombieMode = true;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target_transform = target_trans;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().who_throw_obj = transform.gameObject;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().upScaleWeapon = upscale;
        throwWeaponPrefab.transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider && collision.gameObject.CompareTag("Enemy") && num_alive_player == 0 && canAttack)
        {
            isDead = true;
            ZombieManager.Instance.islose = true;
        }
        else if (collision.collider && collision.gameObject.CompareTag("Enemy") && num_alive_player >= 1 && canAttack)
        {
            WaitToPlayerShield();
        }
    }

    // ShieldFeature
    public async void WaitToPlayerShield()
    {
        shieldObj.SetActive(true);
        Debug.Log("something good " + num_alive_player);
        num_alive_player--;
        CheckShieldIcon(num_alive_player, permParam.max_shield);

        canAttack = false;
        await Task.Delay(1000);
        Debug.Log("something not good " + num_alive_player);
        canAttack = true;
        shieldObj.SetActive(false);
    }

    public void CheckShieldIcon(int current, int max)
    {
        for (int i = 0; i < (current); i++)
        {
            shields[i].SetActive(true);
        }
        for (int i = current; i < max; i++)
        {
            shields[i].SetActive(false);
        }
    }

    //Setup speed and shield
    public void InitPlayerAbilities()
    {
        num_alive_player = permParam.num_add_shield;
        speed = speed + speed * permParam.num_add_speed / 200;
    }

    //Base Attack
    private void BaseAttack(GameObject other)
    {
        StartCoroutine(Attack());

        if (other.gameObject && other.gameObject.GetComponentInChildren<TargetPos>())
        {
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            if (num_throw_attack == 1)
            {
                ThrowWeapon(target, currentPosAttack, dir);
                return;
            }
            float total_angle = angle_attack * (num_throw_attack - 1);
            float start_angle = -total_angle / 2;
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
        }
    }
    //Abilities 1
    private void BehindAttack(GameObject other)
    {
        StartCoroutine(Attack());

        if (other.gameObject.GetComponentInChildren<TargetPos>())
        {
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;

            float total_angle = angle_attack * (num_throw_attack - 1);
            float start_angle = -total_angle / 2;
            if (num_throw_attack == 1)
            {
                ThrowWeapon(target, currentPosAttack, dir);
                float angle = 180f;
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
                return;
            }
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1)) + 180f;
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
        }
    }
    //Abilities 2
    private void OrbitWeapon()
    {
        orbitWeapon.SetActive(true);
        orbitWeapon.GetComponent<OrbitWeapon>().SetUp(range.gameObject.GetComponent<CapsuleCollider>().radius, characterPlayer.current_Weapon);
    }

    //Abilities 3
    private void BulletPlus(GameObject other)
    {
        float num_throw_add;
        StartCoroutine(Attack());

        if (other.gameObject.GetComponentInChildren<TargetPos>())
        {
            num_throw_add = num_throw_attack + 1;
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            float total_angle = angle_attack * (num_throw_add - 1);
            float start_angle = -total_angle / 2;
            for (int i = 0; i < num_throw_add; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_add - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
        }
    }
    //Abilities 4
    private void ChaseAttack(GameObject other)
    {
        StartCoroutine(Attack());

        if (other.gameObject.GetComponentInChildren<TargetPos>())
        {
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            if (num_throw_attack == 1)
            {
                ThrowWeapon(target, currentPosAttack, dir, other.transform);
                return;
            }
            float total_angle = angle_attack * (num_throw_attack - 1);
            float start_angle = -total_angle / 2;
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw, other.transform);
            }
        }
    }
    //Abilities 5
    private async void ContinuousAttack(GameObject other)
    {
        BaseAttack(other);
        await Task.Delay(400);
        BaseAttack(other);
    }

    //Abilities 6 
    private void CrossAttack(GameObject other)
    {
        StartCoroutine(Attack());

        if (other.gameObject.GetComponentInChildren<TargetPos>())
        {
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            float total_angle = angle_attack * (num_throw_attack - 1);
            float start_angle = -total_angle / 2;
            if (num_throw_attack == 1)
            {
                ThrowWeapon(target, currentPosAttack, dir);
                float angle = 90f;
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
                angle = -90f;
                dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
                return;
            }
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1)) + 90f;
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1)) - 90f;
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
        }
    }
    //Abilities 7
    private void DiagonAttack(GameObject other)
    {
        float num_throw_add;
        StartCoroutine(Attack());

        if (other.gameObject.GetComponentInChildren<TargetPos>())
        {
            num_throw_add = num_throw_attack + 2;
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            float total_angle = angle_attack * (num_throw_add - 1);
            float start_angle = -total_angle / 2;
            for (int i = 0; i < num_throw_add; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_add - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
        }

    }
    //Abilities 9
    private void GrowingAttack(GameObject other)
    {
        StartCoroutine(Attack());

        if (other.gameObject && other.gameObject.GetComponentInChildren<TargetPos>())
        {
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            if (num_throw_attack == 1)
            {
                ThrowWeapon(target, currentPosAttack, dir, null, true);
                return;
            }
            float total_angle = angle_attack * (num_throw_attack - 1);
            float start_angle = -total_angle / 2;
            for (int i = 0; i < num_throw_attack; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_attack - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw, null, true);
            }
        }
    }
    //Abilities 14
    private void TripleAttack(GameObject other)
    {
        float num_throw_add;
        StartCoroutine(Attack());

        if (other.gameObject.GetComponentInChildren<TargetPos>())
        {
            num_throw_add = 3 * num_throw_attack;
            currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 0.6f;
            Vector3 target = other.gameObject.GetComponentInChildren<TargetPos>().transform.position;
            target.y = transform.position.y + 0.6f;
            Vector3 dir = (target - currentPosAttack).normalized;
            float total_angle = angle_attack * (num_throw_add - 1);
            float start_angle = -total_angle / 2;
            for (int i = 0; i < num_throw_add; i++)
            {
                float angle = start_angle + i * (total_angle / (num_throw_add - 1));
                Vector3 dir_throw = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                ThrowWeapon(target, currentPosAttack, dir_throw);
            }
        }
    }
}
