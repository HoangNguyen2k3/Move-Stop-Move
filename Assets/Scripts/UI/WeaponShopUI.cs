using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopUI : MonoBehaviour
{
    public WeaponShop[] weaponShops;

    [SerializeField] private TextMeshProUGUI name_weapon;
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private Image image_Weapon;
    [SerializeField] private TextMeshProUGUI paramWeapon;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Button btn_purchase;

    private int current_page;
    private int max_page;

    public static EventHandler eventEquipWeapon;
    private void Start()
    {
        /*        if (!PlayerPrefs.HasKey("EquippedWeapon"))
                {
                    PlayerPrefs.SetString("EquippedWeapon", weaponShops[0].nameWeapon);
                    PlayerPrefs.SetString("WeaponStatus_" + weaponShops[0].nameWeapon, "Purchased");
                }*/
        max_page = weaponShops.Length;
        current_page = 0;
        //        LoadWeaponStatus();
        SettingShopUI();
    }
    private void LoadWeaponStatus()
    {
        foreach (var weapon in weaponShops)
        {
            string status = PlayerPrefs.GetString("WeaponStatus_" + weapon.nameWeapon, "NotPurchased");
            weapon.status = status == "Purchased";
            /*            if (PlayerPrefs.GetString("EquippedWeapon") == weapon.nameWeapon)
                        {
                            weapon.param_Attack += " (Equipped)";
                        }*/
        }
    }
    public void SettingShopUI()
    {
        name_weapon.text = weaponShops[current_page].name;
        if (weaponShops[current_page].status)
        {
            status.text = "(Unlock)";
            btn_purchase.interactable = false;
        }
        else
        {
            status.text = "(Lock)";
            btn_purchase.interactable = true;
        }
        image_Weapon.sprite = weaponShops[current_page].imageWeapon;
        paramWeapon.text = weaponShops[current_page].param_Attack;
        price.text = weaponShops[current_page].price.ToString();
    }
    public void LeftArrowPressed()
    {
        if (current_page == 0)
        {
            return;
        }
        current_page--;
        SettingShopUI();
    }
    public void RightArrowPressed()
    {
        if (current_page == (max_page - 1))
        {
            return;
        }
        current_page++;
        SettingShopUI();
    }
}
