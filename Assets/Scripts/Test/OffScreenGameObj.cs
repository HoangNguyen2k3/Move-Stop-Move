using TMPro;
using UnityEngine;

public class OffScreenGameObj : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
    public TextMeshProUGUI indicatorImage;
    public float edgeOffset = 50f;
    private void Start()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
    }
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        /*        Debug.Log(screenPos);*/
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

        indicatorImage.enabled = isOffScreen;

        if (isOffScreen)
        {
            screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

            Vector3 dir = (target.position - mainCamera.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicatorImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        indicatorImage.rectTransform.position = screenPos;
    }
}
