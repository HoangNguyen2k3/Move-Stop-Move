using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float numCurrentCoin = 0;
    public float addCurrentCoin = 0;
    [SerializeField] private TextMeshProUGUI numCoinUI;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetFloat("Coin", 0);
        }
    }
    private void OnEnable()
    {
        WeaponShopUI.OnWeaponPurchase += WeaponShopUI_OnWeaponPurchase;
    }

    private void WeaponShopUI_OnWeaponPurchase(object sender, WeaponObject weapon)
    {
        numCurrentCoin = PlayerPrefs.GetFloat("Coin");
        numCoinUI.text = numCurrentCoin.ToString();
    }

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
    private void OnDisable()
    {
        WeaponShopUI.OnWeaponPurchase -= WeaponShopUI_OnWeaponPurchase;
    }
}
