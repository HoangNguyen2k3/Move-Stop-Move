using System;

public static class EventManager
{
    public static event EventHandler<int> OnChangePart;
    public static event EventHandler<WeaponShop> OnChangeWeaponCustom;

    public static void ChangePart(int part)
    {
        OnChangePart?.Invoke(null, part);
    }

    public static void ChangeWeaponCustom(WeaponShop weapon)
    {
        OnChangeWeaponCustom?.Invoke(null, weapon);
    }
}
