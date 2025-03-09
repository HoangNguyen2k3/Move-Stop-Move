using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        WeaponShopUI.OnWeaponPurchase += WeaponShopUI_OnWeaponPurchase;
    }

    private void WeaponShopUI_OnWeaponPurchase(object sender, WeaponObject e)
    {
        Debug.Log("Oke bro");
        player.characterPlayer.current_Weapon = e;
        Debug.Log(player.characterPlayer.current_Weapon.name + " " + e.name);
        player.TakeInfoHoldWeapon();
    }
    private void OnDisable()
    {
        WeaponShopUI.OnWeaponPurchase -= WeaponShopUI_OnWeaponPurchase;
    }
}
