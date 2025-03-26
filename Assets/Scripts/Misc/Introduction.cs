using UnityEngine;

public class Introduction : MonoBehaviour
{
    [SerializeField] private GameObject gameIntroduction;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerZombie playerZombie;
    [SerializeField] private float timeRemain = 5f;
    private float temp_time = 0f;
    private void Update()
    {
        temp_time += Time.deltaTime;
        if (temp_time > timeRemain)
        {
            Destroy(gameObject);
            return;
        }
        if (playerController != null)
        {
            if (playerController.direct == Vector3.zero)
            {
                gameIntroduction.SetActive(true);
            }
            else
            {
                gameIntroduction.SetActive(false);
            }
        }
        else if (playerZombie != null)
        {
            if (playerZombie.direct == Vector3.zero)
            {
                gameIntroduction.SetActive(true);
            }
            else
            {
                gameIntroduction.SetActive(false);
            }
        }
    }
}
