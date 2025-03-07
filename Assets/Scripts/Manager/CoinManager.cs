using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float numCurrentCoin = 0;
    public float addCurrentCoin = 0;
    [SerializeField] private TextMeshProUGUI numCoinUI;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetFloat("Coin", 0);
        }
        numCurrentCoin = PlayerPrefs.GetFloat("Coin");
        numCoinUI.text = numCurrentCoin.ToString();
    }
    public void AddingCoin()
    {
        numCurrentCoin = PlayerPrefs.GetFloat("Coin");
        addCurrentCoin = GameManager.Instance.num_coin;
        numCurrentCoin += addCurrentCoin;
        numCoinUI.text = numCurrentCoin.ToString();
        PlayerPrefs.SetFloat("Coin", numCurrentCoin);
    }
}
