using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "ScriptableObject/CharacterObject")]
public class CharaterObj : ScriptableObject
{
    public WeaponObject current_Weapon;
    public float coolDownAttack = 2f;
    public float beginRange = 0.5f;

    public DataCurrentWeapon skin_current_weapon;
    public ClotherShop[] skinClother;
    public FullSkinObject fullSkinPlayer;
    public string status_fullskin;

    public ClotherShop[] hats;
    public ClotherShop[] pants;
    public ClotherShop[] shield;

    public void InitPlayer()
    {
        foreach (var item in hats)
        {
            if (item.status == "Selected")
            {
                skinClother[0] = item;
                break;
            }
        }
        foreach (var item in pants)
        {
            if (item.status == "Selected")
            {
                skinClother[1] = item;
                break;
            }
        }
        foreach (var item in shield)
        {
            if (item.status == "Selected")
            {
                skinClother[2] = item;
                break;
            }
        }
    }
}
