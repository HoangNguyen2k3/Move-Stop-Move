using UnityEngine;
using UnityEngine.UI;

public class FirstPageShop : MonoBehaviour
{
    [SerializeField] private GameObject current_GameObjectChoose;
    [SerializeField] private GameObject main_image_weapon;
    [SerializeField] private GameObject[] color_part_weapon;
    [SerializeField] private WeaponShop currentWeapon;
    [SerializeField] private GameObject button_purchase;
    [SerializeField] private GameObject colorBar;
    public bool isChangeColorWeapon = false;
    public void OnChangeType()
    {
        Mesh mesh = current_GameObjectChoose.GetComponent<MeshFilter>().mesh;
        Material[] materials = current_GameObjectChoose.GetComponent<MeshRenderer>().materials;
        main_image_weapon.GetComponent<MeshFilter>().mesh = mesh;
        main_image_weapon.GetComponent<MeshRenderer>().materials = materials;
        if (isChangeColorWeapon)
        {
            colorBar.SetActive(true);
            button_purchase.SetActive(false);
            for (int i = 0; i < color_part_weapon.Length; i++)
            {
                color_part_weapon[i].GetComponent<Image>().color = materials[i].color;
            }
        }
        else
        {
            colorBar.SetActive(false);
            button_purchase.SetActive(true);
        }
    }
}
