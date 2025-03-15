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
    public FloatingText[] floating;
    [SerializeField] private GameObject textAdding;
    [SerializeField] private GameObject textAnnouceDistance;

    public float startLevel = 0f;
    private float temp = 1;
    //   private float start_level = 0;
    public float current_level;
    private float addingLevel = 1;
    [SerializeField] private CinemachineCamera cam;
    private bool isPlayer = false;
    private void Start()
    {
        if (gameObject.GetComponent<PlayerController>())
        {
            isPlayer = true;
        }
        current_level = startLevel;
        textlevel.text = current_level.ToString();
        //        cam = FindFirstObjectByType<CinemachineCamera>();
    }
    private void Update()
    {
        if (isPlayer)
        {
            GameManager.Instance.num_coin = current_level;
        }
    }
    public void AddLevel()
    {
        if (isPlayer && textAdding)
        {
            textAdding.SetActive(true);
            textAdding.GetComponent<TextMeshProUGUI>().text = "+" + addingLevel;
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
        if (playerController)
        {
            textAnnouceDistance.SetActive(true);
            textAnnouceDistance.GetComponent<TextMeshProUGUI>().text = (transform.localScale.x * 10).ToString() + " m";
            textAnnouceDistance.GetComponent<Animator>().Play("TextAnouce");

            if (cam.Lens.FieldOfView <= maxCam)
            {
                cam.Lens.FieldOfView += 5;
            }
        }
        for (int i = 0; i < floating.Length; i++)
        {
            floating[i].AddOffset(numAddingOffset);

        }
        if (!levelup.isPlaying)
        {
            levelup.Play();
        }
        temp++;
        addingLevel++;
        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);

        if (playerController)
        {

            playerController.addingScale += 2.5f;
        }

    }
}
