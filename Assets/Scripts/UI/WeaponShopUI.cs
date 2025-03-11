using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopUI : MonoBehaviour
{
    [Header("Special Shop")]
    public WeaponShop[] specialShop;
    [SerializeField] private GameObject specialShopObj1;
    [SerializeField] private GameObject specialShopObj2;
    [SerializeField] private GameObject normalshopObj;
    [Header("Normal Shop")]
    public WeaponShop[] weaponShops;


    [SerializeField] private TextMeshProUGUI name_weapon;
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private GameObject image_Weapon;
    [SerializeField] private TextMeshProUGUI paramWeapon;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Button btn_purchase;
    [SerializeField] private Image icon_coin;
    [SerializeField] private PlayerController player;

    private int current_page;
    private int max_page;

    public static event EventHandler<WeaponObject> OnWeaponPurchase;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("EquippedWeapon"))
        {
            PlayerPrefs.SetString("EquippedWeapon", weaponShops[0].nameWeapon);
            PlayerPrefs.SetString("WeaponStatus_" + weaponShops[0].nameWeapon, "Equipped");
        }
        max_page = weaponShops.Length;
        current_page = 0;
        LoadWeaponStatus();
        SettingShopUI();
    }

    private void LoadWeaponStatus()
    {
        foreach (var weapon in weaponShops)
        {
            string status = PlayerPrefs.GetString("WeaponStatus_" + weapon.nameWeapon, ApplicationVariable.notPurchase_status);
            weapon.status = status;
        }
    }

    public void SettingShopUI()
    {
        var currentWeapon = weaponShops[current_page];

        name_weapon.text = currentWeapon.nameWeapon;
        image_Weapon.GetComponent<MeshFilter>().mesh = currentWeapon.imageWeapon.GetComponent<MeshFilter>().sharedMesh;
        image_Weapon.GetComponent<MeshRenderer>().materials = currentWeapon.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials;
        paramWeapon.text = currentWeapon.param_Attack;

        if (currentWeapon.status == ApplicationVariable.purchase_status)
        {
            status.text = "(Unlock)";
            btn_purchase.interactable = true;
            price.text = "EQUIP";
            icon_coin.gameObject.SetActive(false);
        }
        else if (currentWeapon.status == ApplicationVariable.notPurchase_status)
        {
            status.text = "(Lock)";
            btn_purchase.interactable = true;
            price.text = currentWeapon.price.ToString();
            icon_coin.gameObject.SetActive(true);
        }
        else if (currentWeapon.status == ApplicationVariable.eqquipped_status)
        {
            status.text = "(Unlock)";
            btn_purchase.interactable = false;
            price.text = "EQUIPPED";
            icon_coin.gameObject.SetActive(false);
        }
    }

    public void LeftArrowPressed()
    {
        if (current_page > 0)
        {
            current_page--;
            SettingShopUI();
        }
        else if (current_page == 0)
        {
            current_page--;
            specialShopObj1.SetActive(true);
            specialShopObj2.SetActive(false);
            normalshopObj.SetActive(false);
            SettingSpecialShop();
        }
        else if (current_page == -1)
        {
            current_page--;
            specialShopObj1.SetActive(false);
            specialShopObj2.SetActive(true);
            normalshopObj.SetActive(false);
            SettingSpecialShop();
        }
    }

    public void RightArrowPressed()
    {
        if (current_page < max_page - 1 && current_page >= 0)
        {
            current_page++;
            SettingShopUI();
        }
        else if (current_page == -1)
        {
            current_page++;
            specialShopObj1.SetActive(false);
            specialShopObj2.SetActive(false);
            normalshopObj.SetActive(true);
            SettingShopUI();

        }
        else if (current_page == -2)
        {
            specialShopObj1.SetActive(true);
            specialShopObj2.SetActive(false);
            normalshopObj.SetActive(false);
            current_page++;
            SettingSpecialShop();
        }
    }
    public void SettingSpecialShop()
    {

    }
    public void OnPurchaseOrEqquip()
    {
        var currentWeapon = weaponShops[current_page];
        float coin = PlayerPrefs.GetFloat(ApplicationVariable.COIN);
        if (currentWeapon.status == ApplicationVariable.notPurchase_status)
        {
            if (coin >= weaponShops[current_page].price)
            {
                coin -= weaponShops[current_page].price;
                PlayerPrefs.SetFloat(ApplicationVariable.COIN, coin);
                foreach (var weapon_check in weaponShops)
                {
                    if (weapon_check.status == ApplicationVariable.eqquipped_status)
                    {
                        weapon_check.status = ApplicationVariable.purchase_status;
                        PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
                    }
                }
                PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Equipped");
                currentWeapon.status = ApplicationVariable.eqquipped_status;
                OnWeaponPurchase?.Invoke(this, currentWeapon.weapon);
                player.characterPlayer.current_Weapon = currentWeapon.weapon;
                player.TakeInfoHoldWeapon();
            }
            else
            {
                //Lam tiep canh bao khong du tien
            }
        }
        else if (currentWeapon.status == ApplicationVariable.purchase_status)
        {

            foreach (var weapon_check in weaponShops)
            {
                if (weapon_check.status == ApplicationVariable.eqquipped_status)
                {
                    weapon_check.status = ApplicationVariable.purchase_status;
                    PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
                }
            }
            PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Equipped");
            currentWeapon.status = ApplicationVariable.eqquipped_status;
            OnWeaponPurchase?.Invoke(this, currentWeapon.weapon);
            player.characterPlayer.current_Weapon = currentWeapon.weapon;
            player.TakeInfoHoldWeapon();
        }
        SettingShopUI();
    }
    /*    public void OnPurchaseOrEquip()
        {
            var currentWeapon = weaponShops[current_page];

            if (currentWeapon.status == "NotPurchased")
            {
                PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Purchased");
                currentWeapon.status = "Purchased";
            }
            else if (currentWeapon.status == "Purchased")
            {
                PlayerPrefs.SetString("EquippedWeapon", currentWeapon.nameWeapon);

                foreach (var weapon in weaponShops)
                {
                    PlayerPrefs.SetString("WeaponStatus_" + weapon.nameWeapon, "Purchased");
                    weapon.status = "Purchased";
                }

                PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Equipped");
                currentWeapon.status = "Equipped";
            }

            SettingShopUI();
        }*/
}
