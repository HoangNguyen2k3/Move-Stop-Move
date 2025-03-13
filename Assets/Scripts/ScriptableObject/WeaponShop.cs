using UnityEngine;

[CreateAssetMenu(fileName = "WeaponShop", menuName = "ScriptableObject/WeaponShop")]
public class WeaponShop : ScriptableObject
{
    public string nameWeapon;
    public string status;
    public WeaponObject weapon;
    public string required_item;
    public GameObject imageWeapon;
    public string param_Attack;
    public float price;

    public GameObject[] skinWeapon;
}
