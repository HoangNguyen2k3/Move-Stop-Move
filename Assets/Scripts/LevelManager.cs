using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textlevel;
    [Header("If is Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerZombie playerZombie;
    [SerializeField] private float maxCam = 200;
    [SerializeField] private ParticleSystem levelup;

    [SerializeField] private GameObject textAdding;
    [SerializeField] private GameObject textAnnouceDistance;

    public float startLevel = 0f;
    private float temp = 1;
    public float current_level;
    private float addingLevel = 1;
    [SerializeField] private CinemachineCamera cam;
    private bool isPlayer = false;

    [Header("ZombieMode")]

    [SerializeField] private bool zombieMode = false;
    [SerializeField] private PermParamAdd addingItem;
    public float current_num_weapon_throw = 1f;
    public GameObject circle;
    public ZombieManager zombieManager;

    private float addingOrbit = 0.15f;
    private void OnEnable()
    {
        if (zombieMode)
            LevelUpRangeSetUp();
    }
    private void Start()
    {

        if (zombieMode && playerZombie)
        {
            temp = 2f;
            isPlayer = true;
        }
        else
        {
            if (gameObject.GetComponent<PlayerController>())
            {
                isPlayer = true;
            }

        }
        current_level = startLevel;
        textlevel.text = current_level.ToString();
    }
    private void Update()
    {
        if (isPlayer && GameManager.Instance)
        {
            GameManager.Instance.num_coin = current_level;
        }
    }
    public void AddLevel()
    {
        if (isPlayer && textAdding)
        {
            textAdding.SetActive(true);
            if (playerZombie)
            {
                textAdding.GetComponent<TextMeshProUGUI>().text = "+" + 1;
            }
            else
            {
                textAdding.GetComponent<TextMeshProUGUI>().text = "+" + addingLevel;
            }
        }
        if (playerZombie)
        {
            current_level++;
        }
        else
        {
            current_level += addingLevel;
        }
        if (zombieManager)
        {
            zombieManager.num_coin = current_level;
        }
        textlevel.text = current_level.ToString();
        if (current_level >= 5 * temp && current_level != 0)
        {
            LevelUp();
        }

    }
    public void AddLevelLoop()
    {
        textlevel.text = current_level.ToString();
        if (current_level >= 5 * temp && current_level != 0)
        {
            LevelUp();
            AddLevelLoop();
        }

    }
    private void LevelUp()
    {
        if (playerController || playerZombie)
        {
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFXSound(SoundManager.Instance.level_up);
            textAnnouceDistance.SetActive(true);
            textAnnouceDistance.GetComponent<TextMeshProUGUI>().text = (transform.localScale.x * 10).ToString("F2") + " m";
            textAnnouceDistance.GetComponent<Animator>().Play("TextAnouce");

            if (cam.Lens.FieldOfView <= maxCam)
            {
                cam.Lens.FieldOfView += 2.5f;
            }
        }
        if (zombieMode && playerZombie)
        {
            if (zombieMode && current_num_weapon_throw < addingItem.num_max_throw)
            {
                current_num_weapon_throw++;
                playerZombie.num_throw_attack = current_num_weapon_throw;
                if (playerZombie.num_choose == 2)
                {
                    playerZombie.ChangeRangeOrbit(playerZombie.range.gameObject.GetComponent<CapsuleCollider>().radius + addingOrbit);
                    addingOrbit += 0.15f;
                }
            }
        }
        if (!levelup.isPlaying)
        {
            levelup.Play();
        }
        if (zombieMode)
        {
            temp += 4;
        }
        else
        {
            temp++;
        }
        addingLevel++;
        transform.localScale += new Vector3(0.025f, 0.025f, 0.025f);

        if (playerController)
        {

            playerController.addingScale += 2.5f;
        }
        if (zombieMode && playerZombie)
        {
            playerZombie.addingScale += 2.5f;
        }

    }
    public void LevelUpRange()
    {
        if (cam.Lens.FieldOfView <= maxCam)
        {
            cam.Lens.FieldOfView += 2.5f / 2;
        }
        // circle.transform.localScale += new Vector3(0.025f * 2, 0.025f * 2, 0.025f * 2);
        transform.localScale += new Vector3(0.025f, 0.025f, 0.025f);
        playerZombie.speed += 0.3f;
    }
    public void LevelUpRangeSetUp()
    {
        float temp = addingItem.num_add_range;
        if (cam.Lens.FieldOfView <= maxCam)
        {
            cam.Lens.FieldOfView += 2.5f * temp / 10;
        }
        float temp_1 = 0.025f * temp / 10 * 2;
        circle.transform.localScale += new Vector3(temp_1, temp_1, temp_1);
    }
}
