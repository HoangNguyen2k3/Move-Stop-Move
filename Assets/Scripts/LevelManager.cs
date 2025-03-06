using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textlevel;
    [Header("If is Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float maxCam = 84;
    [SerializeField] private ParticleSystem levelup;
    [Header("Floating text")]
    [SerializeField] private float numAddingOffset = 0.05f;
    public float offset_floatingtext = 0f;

    [SerializeField] private GameObject textAdding;

    public float startLevel = 0f;
    private float temp = 1;
    //   private float start_level = 0;
    public float current_level;
    private float addingLevel = 6;
    private CinemachineCamera cam;
    private bool isPlayer = false;
    private void Start()
    {
        if (gameObject.GetComponent<PlayerController>())
        {
            isPlayer = true;
        }
        current_level = startLevel;
        textlevel.text = current_level.ToString();
        cam = FindFirstObjectByType<CinemachineCamera>();
    }
    /*    public void CheckLevelBegin()
        {
            current_level = 
        }*/
    public void AddLevel()
    {
        if (isPlayer)
        {
            textAdding.SetActive(true);
        }
        current_level += addingLevel;
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
        offset_floatingtext += numAddingOffset;
        if (!levelup.isPlaying)
        {
            levelup.Play();
        }
        temp++;
        addingLevel++;
        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        if (cam.Lens.FieldOfView <= maxCam)
        {
            cam.Lens.FieldOfView += 10;
        }
        if (playerController)
        {
            playerController.addingScale += 2.5f;
        }

    }
}
