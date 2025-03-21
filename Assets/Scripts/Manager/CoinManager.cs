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
        PlayerPrefs.SetFloat("Coin", 5500);
    }
    private void OnEnable()
    {
        WeaponShopUI.OnWeaponPurchase += WeaponShopUI_OnWeaponPurchase;
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

    private void WeaponShopUI_OnWeaponPurchase(object sender, WeaponObject weapon)
    {
        numCurrentCoin = PlayerPrefs.GetFloat("Coin");
        numCoinUI.text = numCurrentCoin.ToString();
    }

    public void AddingCoin()
    {
        numCurrentCoin = PlayerPrefs.GetFloat("Coin");
        if (ZombieManager.Instance != null)
        {
            addCurrentCoin = ZombieManager.Instance.num_coin;

        }
        else
        {
            addCurrentCoin = GameManager.Instance.num_coin;
        }
        numCurrentCoin += addCurrentCoin;
        numCoinUI.text = numCurrentCoin.ToString();
        PlayerPrefs.SetFloat("Coin", numCurrentCoin);
    }
    public bool PurchaseSomething(float price)
    {
        if (numCurrentCoin >= price)
        {
            numCurrentCoin -= price;
            PlayerPrefs.SetFloat("Coin", numCurrentCoin);
            numCoinUI.text = numCurrentCoin.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void MinusCoin(float price)
    {
        numCurrentCoin -= price;
        PlayerPrefs.SetFloat("Coin", numCurrentCoin);
        numCoinUI.text = numCurrentCoin.ToString();
    }
    private void OnDisable()
    {
        WeaponShopUI.OnWeaponPurchase -= WeaponShopUI_OnWeaponPurchase;
    }
}
