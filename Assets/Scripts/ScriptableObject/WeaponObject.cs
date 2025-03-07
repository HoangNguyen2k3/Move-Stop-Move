using UnityEngine;

[CreateAssetMenu(fileName = "WeaponObj", menuName = "ScriptableObject/WeaponObj")]
public class WeaponObject : ScriptableObject
{
    public GameObject weaponThrow;
    public GameObject weaponHold;

    [Header("Only In Weapon")]
    public GameObject touchSomething;
    public float speedMove;
    public float speedRotate;
    public bool isTurning = true;
    public float range;
}
