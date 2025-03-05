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

    private float temp = 1;
    private float start_level = 0;
    public float current_level;
    private float addingLevel = 1;
    private CinemachineCamera cam;
    private void Start()
    {
        current_level = start_level;
        textlevel.text = current_level.ToString();
        cam = FindFirstObjectByType<CinemachineCamera>();
    }

    public void AddLevel()
    {
        current_level += addingLevel;
        textlevel.text = current_level.ToString();
        if (current_level >= 5 * temp && current_level != 0)
        {
            LevelUp();
        }

    }
    private void LevelUp()
    {
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
