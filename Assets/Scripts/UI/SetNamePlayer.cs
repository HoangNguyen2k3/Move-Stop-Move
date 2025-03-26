using TMPro;
using UnityEngine;

public class SetNamePlayer : MonoBehaviour
{
    private TextMeshProUGUI namePlayer;

    private void Start()
    {
        namePlayer = GetComponent<TextMeshProUGUI>();
        namePlayer.text = PlayerPrefs.GetString("NamePlayer", "YOU");
    }
}
