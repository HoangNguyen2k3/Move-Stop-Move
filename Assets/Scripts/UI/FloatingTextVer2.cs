using TMPro;
using UnityEngine;

public class FloatingTextScreenSpace : MonoBehaviour
{
    public Transform nameTagPosition;
    private Canvas screenCanvas;
    public Transform root;
    public TextMeshProUGUI name_object;
    private RectTransform rectTransform;
    public GameObject image;

    void Start()
    {
        screenCanvas = GameObject.FindGameObjectWithTag("FloatingText").GetComponent<Canvas>();


        root = transform.root;
        transform.SetParent(screenCanvas.transform, false);

        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (root == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(nameTagPosition.position);

        if (screenPos.z < 0 || CheckPosVisible(screenPos))
        {
            name_object.enabled = false;
            image.SetActive(false);
        }
        else
        {
            name_object.enabled = true;
            image.SetActive(true);
            rectTransform.position = screenPos;
        }
    }
    public bool CheckPosVisible(Vector3 screenPos)
    {
        return screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;
    }
}