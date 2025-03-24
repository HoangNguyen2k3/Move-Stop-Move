using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterWrapper
{
    public string status_fullskin;
    public float cooldown_attack;
    public float beginRange;
}
[System.Serializable]
public class HatWrap
{
    public string[] status;
}
[System.Serializable]
public class PantWrap
{
    public string[] status;
}
[System.Serializable]
public class ShieldWrap
{
    public string[] status;
}
[System.Serializable]
public class WrapperFullSkin
{
    public string[] status;
}
[System.Serializable]
public class WrapperPermParm
{
    public int num_add_shield;
    public float num_add_speed;
    public float num_add_range;
    public float num_max_throw;

    public float price_current_shield;
    public float price_current_speed;
    public float price_current_range;
    public float price_current_throw;
}
public static class SavingData
{
    public static void SaveData<T>(T data, string filename)
    {
        string path = Application.persistentDataPath + "/" + filename;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Data saved to: " + path);
    }

    public static T LoadData<T>(T defaultData, string filename)
    {
        string path = Application.persistentDataPath + "/" + filename;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
        return defaultData;
    }
}