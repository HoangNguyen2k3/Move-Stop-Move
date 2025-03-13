using UnityEngine;

public class SpecialWeaponShopCanvas : MonoBehaviour
{
    [SerializeField] private GameObject[] activeWeapon;
    [SerializeField] private GameObject weaponCustom;
    private int num_weapon_choose;
    [SerializeField] private bool isHammer = true;
    private void OnEnable()
    {
        if (!isHammer)
        {
            num_weapon_choose = PlayerPrefs.GetInt("num_weapon_choose_candy", 2);
        }
        else
        {
            num_weapon_choose = PlayerPrefs.GetInt("num_weapon_choose_hammer", 2);
        }

        for (int i = 0; i < activeWeapon.Length; i++)
        {
            if (i == num_weapon_choose)
            {
                activeWeapon[i].SetActive(true);
            }
            else
            {
                activeWeapon[i].SetActive(false);
            }
        }
    }

}
