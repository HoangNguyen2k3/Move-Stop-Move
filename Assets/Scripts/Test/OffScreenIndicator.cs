using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
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

        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        indicatorObject.SetActive(isOffScreen);

        if (isOffScreen)
        {
            screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

            Vector3 targetDirection = target.position - Camera.main.transform.position;
            targetDirection.z = 0;

            indicatorObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
        }

        indicatorObject.GetComponent<RectTransform>().position = screenPos;
    }
}
