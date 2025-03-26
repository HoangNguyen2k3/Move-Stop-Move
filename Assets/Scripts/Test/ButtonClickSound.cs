using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        if (SoundManager.Instance == null) { return; }
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.button_click);
        });
    }
}
