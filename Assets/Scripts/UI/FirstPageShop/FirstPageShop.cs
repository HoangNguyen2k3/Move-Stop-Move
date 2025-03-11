using UnityEngine;
using UnityEngine.UI;

public class FirstPageShop : MonoBehaviour
{
    [Header("Button Feature specific")]
    [SerializeField] private GameObject current_GameObjectChoose;
    [SerializeField] private GameObject main_image_weapon;
    [Header("Generate")]
    [SerializeField] private GameObject[] current_GameObjectVer2;
    [Header("Required")]
    [SerializeField] private GameObject[] color_part_weapon;
    [SerializeField] private WeaponShop currentWeapon;
    [SerializeField] private GameObject button_purchase;
    [SerializeField] private GameObject button_select_custem;
    [SerializeField] private GameObject colorBar;
    public bool isChangeColorWeapon = false;
    [SerializeField] private bool isHammer = true;
    [SerializeField] private int num_weapon;

    private void Start()
    {
        if (!isChangeColorWeapon) { return; }
        int num_color = color_part_weapon.Length;
        if (!PlayerPrefs.HasKey("Color_Candy_1") && !isHammer)
        {
            InitColorCandy();
        }
        else if (!PlayerPrefs.HasKey("Color_Hammer_1") && isHammer)
        {
            InitColorHammer();
        }
        if (!isHammer)
        {
            GetColorCandy(num_color);
        }
        else if (isHammer)
        {
            GetColorHammer(num_color);
        }
    }

    private void GetColorHammer(int num_color)
    {
        for (int i = 0; i < num_color; i++)
        {
            string hexColor = PlayerPrefs.GetString("Color_Hammer_" + i);
            if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
            {
                current_GameObjectChoose.GetComponent<MeshRenderer>().materials[i].color = newColor;
            }
            else
            {
                Debug.LogWarning("Invalid hex color: " + hexColor);
            }
        }
    }

    private void GetColorCandy(int num_color)
    {
        for (int i = 0; i < num_color; i++)
        {
            string hexColor = PlayerPrefs.GetString("Color_Candy_" + i);
            if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
            {
                current_GameObjectChoose.GetComponent<MeshRenderer>().materials[i].color = newColor;
            }
            else
            {
                Debug.LogWarning("Invalid hex color: " + hexColor);
            }
        }
    }

    public void InitColorCandy()
    {
        PlayerPrefs.SetString("Color_Candy_0", "#6CE075");
        PlayerPrefs.SetString("Color_Candy_1", "#1F9F29");
        PlayerPrefs.SetString("Color_Candy_2", "#330404");
    }
    public void InitColorHammer()
    {
        PlayerPrefs.SetString("Color_Candy_0", "#330404");
        PlayerPrefs.SetString("Color_Candy_1", "#330404");
    }
    public void OnChangeType()
    {
        if (isHammer)
        {
            PlayerPrefs.SetInt("num_weapon_choose_hammer", num_weapon);
        }
        else
        {
            PlayerPrefs.SetInt("num_weapon_choose_candy", num_weapon);
        }
        Mesh mesh = current_GameObjectChoose.GetComponent<MeshFilter>().mesh;
        Material[] materials = current_GameObjectChoose.GetComponent<MeshRenderer>().materials;
        main_image_weapon.GetComponent<MeshFilter>().mesh = mesh;
        main_image_weapon.GetComponent<MeshRenderer>().materials = materials;
        if (isChangeColorWeapon)
        {
            colorBar.SetActive(true);
            button_select_custem.SetActive(true);
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
            button_select_custem.SetActive(false);
        }
    }
    public void OnChangeTypeVer2(int num)
    {
        Mesh mesh = current_GameObjectVer2[num].GetComponent<MeshFilter>().mesh;
        Material[] materials = current_GameObjectVer2[num].GetComponent<MeshRenderer>().materials;
        main_image_weapon.GetComponent<MeshFilter>().mesh = mesh;
        main_image_weapon.GetComponent<MeshRenderer>().materials = materials;
        if (num == 0)
        {
            button_select_custem.SetActive(true);
            colorBar.SetActive(true);
            button_purchase.SetActive(false);
            for (int i = 0; i < color_part_weapon.Length; i++)
            {
                color_part_weapon[i].GetComponent<Image>().color = materials[i].color;
            }
        }
        else
        {
            button_select_custem.SetActive(false);
            colorBar.SetActive(false);
            button_purchase.SetActive(true);
        }
    }
}
