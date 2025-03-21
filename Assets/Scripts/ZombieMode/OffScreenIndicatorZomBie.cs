using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicatorZombie : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
    public float edgeOffset = 50f;
    public Image arrow;


    private void Start()
    {
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        if (ZombieManager.Instance.currentInLobbyZombie)
        {
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        arrow.enabled = isOffScreen;

        if (isOffScreen)
        {
            screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

            Vector3 targetDirection = target.position - Camera.main.transform.position;
            targetDirection.z = 0;

            arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
        }

        arrow.GetComponent<RectTransform>().position = screenPos;
    }
}
