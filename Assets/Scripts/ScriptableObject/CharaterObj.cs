using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "ScriptableObject/CharacterObject")]
public class CharaterObj : ScriptableObject
{
    public WeaponObject current_Weapon;
    public float coolDownAttack = 2f;
    public float beginRange = 0.5f;

    public DataCurrentWeapon skin_current_weapon;
    public ClothPlayerObject skinClother;
}
