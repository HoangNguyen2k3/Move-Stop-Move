using UnityEngine;

public class SaveDataInZombieMode : MonoBehaviour
{
    [SerializeField] private PermParamAdd permParamZombieMode;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("firstSaveZombieMode")) { return; }
        LoadPermParam();
    }
    private void OnApplicationQuit()
    {
        SavePermParam();
    }
    private void LoadPermParam()
    {
        WrapperPermParm wrapperPermParm = SavingData.LoadData(new WrapperPermParm(), ApplicationVariable.PATH_PERM_PARAM);
        permParamZombieMode.num_add_shield = wrapperPermParm.num_add_shield;
        permParamZombieMode.num_add_speed = wrapperPermParm.num_add_speed;
        permParamZombieMode.num_add_range = wrapperPermParm.num_add_range;
        permParamZombieMode.num_max_throw = wrapperPermParm.num_max_throw;
        permParamZombieMode.price_current_shield = wrapperPermParm.price_current_shield;
        permParamZombieMode.price_current_speed = wrapperPermParm.price_current_speed;
        permParamZombieMode.price_current_range = wrapperPermParm.price_current_range;
        permParamZombieMode.price_current_throw = wrapperPermParm.price_current_throw;
    }
    private void SavePermParam()
    {
        WrapperPermParm wrapperPermParm = new WrapperPermParm
        {
            num_add_shield = permParamZombieMode.num_add_shield,
            num_add_speed = permParamZombieMode.num_add_speed,
            num_add_range = permParamZombieMode.num_add_range,
            num_max_throw = permParamZombieMode.num_max_throw,
            price_current_shield = permParamZombieMode.price_current_shield,
            price_current_speed = permParamZombieMode.price_current_speed,
            price_current_range = permParamZombieMode.price_current_range,
            price_current_throw = permParamZombieMode.price_current_throw
        };
        SavingData.SaveData(wrapperPermParm, ApplicationVariable.PATH_PERM_PARAM);
    }
}
