using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Transform root;
    public Transform unit;
    public bool isSameColor = false;
    public PlayerController player;
    public TextMeshProUGUI text;
    public Image image;
    public bool isImage = true;
    private void Awake()
    {
        if (image != null || text != null)
        {
            if (isImage)
            {
                image = GetComponent<Image>();
            }
            else
            {
                text = GetComponent<TextMeshProUGUI>();
            }

        }
        //color = GetComponent<TextMeshPro>().color;
        if (GetComponentInParent<EnemiesHealth>())
        {
            root = GetComponentInParent<EnemiesHealth>().transform;
        }
        else
        {
            root = GetComponentInParent<PlayerController>().transform;
        }
    }
    private void Update()
    {
        if (image != null || text != null)
        {

            if (isImage)
            {
                if (image.color != player.current_Mesh.sharedMaterial.color)
                {
                    image.color = player.current_Mesh.sharedMaterial.color;
                }
            }
            else
            {
                if (text.color != player.current_Mesh.sharedMaterial.color)
                {
                    text.color = player.current_Mesh.sharedMaterial.color;
                }
            }
        }
        if (root == null)
        {
            Destroy(gameObject);
        }
    }
    public void AddOffset(float offsetAdd)
    {
        if (unit == null) { return; }
        unit.position += new Vector3(0, offsetAdd, 0);
    }
}
