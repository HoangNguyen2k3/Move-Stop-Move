//using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPageShop : MonoBehaviour
{
    [Header("Button Feature specific")]
    [SerializeField] private GameObject current_GameObjectChoose;
    [SerializeField] private GameObject main_image_weapon;
    [Header("Required")]
    [SerializeField] private GameObject[] color_part_weapon;
    [HideInInspector] public WeaponShop currentWeapon;


    [SerializeField] private GameObject button_purchase;
    [SerializeField] private GameObject image_ads;
    [SerializeField] private TextMeshProUGUI text;


    [SerializeField] private RectTransform begin_Pos;
    [SerializeField] private RectTransform custom_Pos;
    [SerializeField] private GameObject colorBar;
    public bool isChangeColorWeapon = false;
    [SerializeField] public int num_weapon;

    [SerializeField] private PurchaseCustomWeapon custom;


    /*    private void OnEnable()
        {
            EventManager.OnChangeWeaponCustom += ChaneWeaponCustom;
        }

        private void ChaneWeaponCustom(object sender, WeaponShop e)
        {
            currentWeapon = e;
            if (PlayerPrefs.HasKey("Color_" + currentWeapon.nameWeapon + "_custom_0"))
                GetColorCustom(currentWeapon.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials.Length);
        }*/
    private void Start()
    {
        custom = GetComponentInParent<PurchaseCustomWeapon>();
    }
    public void GetColorCustom(int num_color)
    {
        MeshRenderer meshRenderer = current_GameObjectChoose.GetComponent<MeshRenderer>();
        Material[] currentMaterials = meshRenderer.sharedMaterials;
        if (currentMaterials.Length < num_color)
        {
            Material[] newMaterials = new Material[num_color];
            for (int i = 0; i < num_color; i++)
            {
                newMaterials[i] = i < currentMaterials.Length
                    ? currentMaterials[i]
                    : new Material(Shader.Find("Standard"));
            }
            meshRenderer.materials = newMaterials;
        }
        for (int i = 0; i < num_color; i++)
        {
            string key = "Color_" + PurchaseCustomWeapon.lastWeaponShop.nameWeapon + "_custom_" + i;
            string hexColor = PlayerPrefs.GetString(key);

            if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
            {
                meshRenderer.materials[i].color = newColor;
            }
        }
    }


    public void OnChangeType()
    {
        if (num_weapon != 3 && num_weapon != 4)
            custom.CheckEqippedWeapon(num_weapon);
        if (num_weapon == 3 || num_weapon == 4)
        {
            image_ads.SetActive(true);
            text.text = "Unlock";
            text.color = Color.red;
        }
        else
        {
            text.color = Color.black;
            image_ads.SetActive(false);
        }
        PurchaseCustomWeapon.num_weap = num_weapon;
        Mesh mesh = current_GameObjectChoose.GetComponent<MeshFilter>().mesh;
        Material[] materials = current_GameObjectChoose.GetComponent<MeshRenderer>().materials;
        main_image_weapon.GetComponent<MeshFilter>().mesh = mesh;
        main_image_weapon.GetComponent<MeshRenderer>().materials = materials;
        if (isChangeColorWeapon)
        {
            colorBar.SetActive(true);
            button_purchase.GetComponent<RectTransform>().anchoredPosition = custom_Pos.anchoredPosition;
            ComponentOptColor.OnChangePart?.Invoke(this, 0);
            for (int i = 0; i < current_GameObjectChoose.GetComponent<MeshRenderer>().materials.Length; i++)
            {
                color_part_weapon[i].GetComponent<Image>().color = materials[i].color;
            }
        }
        else
        {
            colorBar.SetActive(false);
            button_purchase.GetComponent<RectTransform>().anchoredPosition = begin_Pos.anchoredPosition;
        }
    }
    /*    private void OnDisable()
        {
            EventManager.OnChangeWeaponCustom -= ChaneWeaponCustom;
        }*/
}
