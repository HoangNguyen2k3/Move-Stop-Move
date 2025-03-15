using TMPro;
using UnityEngine;

public class PurchaseCustomWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] five_type_weapon;
    [SerializeField] private GameObject image_weapon;
    [SerializeField] private GameObject[] active_weapon;
    [SerializeField] private TextMeshProUGUI param_Weapon;

    [SerializeField] private ComponentOptColor component;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject TableColor;

    [HideInInspector] public WeaponShop weapon;

    [SerializeField] private RectTransform begin_Pos;
    public static WeaponShop lastWeaponShop;

    [SerializeField] private FirstPageShop custom;
    [SerializeField] private WeaponShopUI weaponUI;
    public static float num_weap = 2;

    [SerializeField] private TextMeshProUGUI text_purchaseButton;
    [SerializeField] private UIGeneratePress ui_show;
    public void ChangeWeapon()
    {
        lastWeaponShop = weapon;

        for (int i = 0; i < five_type_weapon.Length; i++)
        {
            five_type_weapon[i].GetComponent<MeshFilter>().mesh = weapon.skinWeapon[i].GetComponent<MeshFilter>().sharedMesh;
            five_type_weapon[i].GetComponent<MeshRenderer>().materials = weapon.skinWeapon[i].GetComponent<MeshRenderer>().sharedMaterials;
        }
        image_weapon.GetComponent<MeshFilter>().mesh = weapon.skinWeapon[2].GetComponent<MeshFilter>().sharedMesh;
        image_weapon.GetComponent<MeshRenderer>().materials = weapon.skinWeapon[2].GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < five_type_weapon.Length; i++)
        {
            if (i == 2) { active_weapon[i].SetActive(true); }
            else { active_weapon[i].SetActive(false); }
        }
        param_Weapon.text = weapon.param_Attack.ToString();
        component.ChangeComponent(weapon.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials.Length);
        TableColor.SetActive(false);
        custom.GetColorCustom(lastWeaponShop.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials.Length);
        if (begin_Pos)
            purchaseButton.GetComponent<RectTransform>().anchoredPosition = begin_Pos.anchoredPosition;
        custom.SetColorButton(2);
        CheckEqippedWeapon(2);
    }
    public void SelectWeapon()
    {
        if (num_weap == 4 || num_weap == 3) { return; }
        weaponUI.SelectWeapon(lastWeaponShop, num_weap);
        PlayerPrefs.SetString(lastWeaponShop.nameWeapon + " select_button" + num_weap, "Equip");
        weaponUI.SetAgain(lastWeaponShop.nameWeapon + " select_button" + num_weap);
        CheckEqippedWeapon((int)num_weap);
        ui_show.ShowAndHiddenGameObject();
    }
    public void CheckEqippedWeapon(int i)
    {

        if (PlayerPrefs.HasKey(lastWeaponShop.nameWeapon + " select_button" + i))
        {
            if (PlayerPrefs.GetString(lastWeaponShop.nameWeapon + " select_button" + i) == "Equip")
            {
                text_purchaseButton.text = "SELECTED";
                return;
            }
            else
            {
                text_purchaseButton.text = "SELECT";
                return;
            }
        }

    }
}
