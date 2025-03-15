using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
    //    public Image indicatorImage;
    public float edgeOffset = 50f;
    public GameObject indicatorObject;
    private LobbyManager lobby;
    private void Start()
    {
        lobby = FindFirstObjectByType<LobbyManager>();
    }
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        if (lobby.currentinLobby)
        {
            return;
        }
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;
        //        indicatorImage.enabled = isOffScreen;
        indicatorObject.SetActive(isOffScreen);
        if (isOffScreen)
        {
            screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

            Vector3 dir = (target.position - mainCamera.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //  indicatorImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
            indicatorObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        indicatorObject.GetComponent<RectTransform>().position = screenPos;
        //        indicatorImage.rectTransform.position = screenPos;
    }
}



