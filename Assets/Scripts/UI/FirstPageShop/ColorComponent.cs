using UnityEngine;
using UnityEngine.UI;

public class ColorComponent : MonoBehaviour
{
    [SerializeField] private GameObject CustomInMenu;
    [SerializeField] private GameObject MainInMenu;
    [SerializeField] private int num_color;
    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<Image>().color = color;
        CustomInMenu.GetComponent<MeshRenderer>().materials[num_color].color = color;
        MainInMenu.GetComponent<MeshRenderer>().materials[num_color].color = color;
    }
}
