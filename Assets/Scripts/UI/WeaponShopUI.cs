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

    [SerializeField] private GameObject purchasedWeapon;
    [SerializeField] private GameObject notPurchaseWeapon;

    [SerializeField] private PurchaseCustomWeapon customWeapon;

    private int current_page;
    private int start_page;
    private int max_page;

    [SerializeField] private GameObject button_ads;

    public static event EventHandler<WeaponObject> OnWeaponPurchase;


    /*    private void Start()
        {
            if (!PlayerPrefs.HasKey("EquippedWeapon"))
            {
                PlayerPrefs.SetString("EquippedWeapon", weaponShops[0].nameWeapon);
                PlayerPrefs.SetString("WeaponStatus_" + weaponShops[0].nameWeapon, "Equipped");
            }
            max_page = weaponShops.Length;
            current_page = 0;
            LoadWeaponStatus();
            for (int i = 0; i < max_page; i++)
            {
                if (weaponShops[i].status == ApplicationVariable.notPurchase_status)
                {
                    current_page = i; break;
                }
            }
            SettingShopUI();
        }*/
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("EquippedWeapon"))
        {
            PlayerPrefs.SetString("EquippedWeapon", weaponShops[0].nameWeapon);
            PlayerPrefs.SetString("WeaponStatus_" + weaponShops[0].nameWeapon, "Equipped");
        }
        max_page = weaponShops.Length;
        current_page = 0;
        LoadWeaponStatus();
        for (int i = 0; i < max_page; i++)
        {
            if (weaponShops[i].status == ApplicationVariable.notPurchase_status)
            {
                current_page = i; break;
            }
        }
        start_page = current_page;
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
        if (currentWeapon.status == ApplicationVariable.notPurchase_status)
        {
            if (start_page == current_page)
            {
                button_ads.SetActive(true);
                btn_purchase.interactable = true;

            }
            else
            {
                btn_purchase.interactable = false;
                button_ads.SetActive(false);
            }
            purchasedWeapon.SetActive(false);
            notPurchaseWeapon.SetActive(true);
            LockWeapon();
        }
        else
        {
            customWeapon.weapon = currentWeapon;
            customWeapon.ChangeWeapon();
            notPurchaseWeapon.SetActive(false);
            purchasedWeapon.SetActive(true);
        }
    }

    private void LockWeapon()
    {
        var currentWeapon = weaponShops[current_page];

        name_weapon.text = currentWeapon.nameWeapon;
        image_Weapon.GetComponent<MeshFilter>().mesh = currentWeapon.imageWeapon.GetComponent<MeshFilter>().sharedMesh;
        image_Weapon.GetComponent<MeshRenderer>().materials = currentWeapon.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials;
        paramWeapon.text = currentWeapon.param_Attack;

        if (currentWeapon.status == ApplicationVariable.purchase_status)
        {
            status.text = "(Unlock)";
            price.text = "EQUIP";
            icon_coin.gameObject.SetActive(false);
        }
        else if (currentWeapon.status == ApplicationVariable.notPurchase_status)
        {
            status.text = "(Lock)";
            price.text = currentWeapon.price.ToString();
            icon_coin.gameObject.SetActive(true);
        }
        else if (currentWeapon.status == ApplicationVariable.eqquipped_status)
        {
            status.text = "(Unlock)";
            price.text = "EQUIPPED";
            icon_coin.gameObject.SetActive(false);
        }
        if (start_page != current_page)
        {
            status.text = "(Unlock " + weaponShops[current_page - 1].nameWeapon + " First)";
        }
    }

    public void LeftArrowPressed()
    {
        if (current_page > 0)
        {
            current_page--;
            SettingShopUI();
        }
    }

    public void RightArrowPressed()
    {
        if (current_page < max_page - 1 && current_page >= 0)
        {
            current_page++;
            SettingShopUI();
        }
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
    public void SelectWeapon(WeaponShop weapon, float num)
    {
        foreach (var weapon_check in weaponShops)
        {
            if (weapon_check.status == ApplicationVariable.eqquipped_status)
            {
                weapon_check.status = ApplicationVariable.purchase_status;
                PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
            }
        }
        PlayerPrefs.SetString("EquipCurrentWeapon", PurchaseCustomWeapon.lastWeaponShop.nameWeapon);



        player.characterPlayer.current_Weapon = weapon.weapon;
        if (num == 0) { TakeColorWeaponFromDatabase(); }
        else
        {
            for (int i = 0; i < player.characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
            {
                player.characterPlayer.skin_current_weapon.material[i].color = weapon.skinWeapon[(int)num].GetComponent<MeshRenderer>().sharedMaterials[i].color;
            }

        }
        player.TakeInfoHoldWeapon();
    }
    public void TakeColorWeaponFromDatabase()
    {
        if (!PlayerPrefs.HasKey("EquipCurrentWeapon")) { return; }
        string name_weapon = PlayerPrefs.GetString("EquipCurrentWeapon");
        for (int i = 0; i < player.characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
        {
            string key = "Color_" + name_weapon + "_custom_" + i;
            string hexColor = PlayerPrefs.GetString(key);

            if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
            {
                player.characterPlayer.skin_current_weapon.material[i].color = newColor;
            }
        }

    }
    public void SetAgain(string a)
    {
        foreach (var weapon_check in weaponShops)
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerPrefs.HasKey(weapon_check.nameWeapon + " select_button" + i))
                {
                    if (PlayerPrefs.GetString(weapon_check.nameWeapon + " select_button" + i) == "Equip" && a != (weapon_check.nameWeapon + " select_button" + i))
                    {
                        PlayerPrefs.SetString(weapon_check.nameWeapon + " select_button" + i, "UnEquip");
                    }
                }
            }
        }
    }
}
