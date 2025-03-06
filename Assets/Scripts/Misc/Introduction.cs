using UnityEngine;

public class Introduction : MonoBehaviour
{
    [SerializeField] private GameObject gameIntroduction;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float timeRemain = 10f;
    private float temp_time = 0f;
    private void Update()
    {
        temp_time += Time.deltaTime;
        if (temp_time > timeRemain)
        {
            Destroy(gameObject);
            return;
        }
        if (playerController.direct == Vector3.zero)
        {
            gameIntroduction.SetActive(true);
        }
        else
        {
            gameIntroduction.SetActive(false);
        }
    }
}
