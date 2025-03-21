using TMPro;
using UnityEngine;

public class UI_Special_Skill : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] paramSkill;
    [SerializeField] private TextMeshProUGUI[] price;
    [SerializeField] private GameObject[] button;
    [SerializeField] private GameObject[] maxSkill;
    [SerializeField] private PermParamAdd permParam;
    [SerializeField] private CoinManager coinManager;
    private float current_coin;
    public GameObject player;

    private void Start()
    {
        CheckAllWeapon();
        player.GetComponent<PlayerZombie>().CheckShieldIcon(permParam.num_add_shield, permParam.max_shield);
    }
    public void PurchaseShield()
    {
        current_coin = PlayerPrefs.GetFloat("Coin");
        if (current_coin >= permParam.price_current_shield && permParam.num_add_shield < permParam.max_shield)
        {
            coinManager.MinusCoin(permParam.price_current_shield);
            permParam.price_current_shield *= 2;
            permParam.num_add_shield += 1;
            player.GetComponent<PlayerZombie>().CheckShieldIcon(permParam.num_add_shield, permParam.max_shield);

        }
        CheckAllWeapon();
    }
    public void PurchaseSpeed()
    {
        current_coin = PlayerPrefs.GetFloat("Coin");
        if (current_coin >= permParam.price_current_speed && permParam.num_add_speed < permParam.max_speed)
        {
            coinManager.MinusCoin(permParam.price_current_speed);
            permParam.price_current_speed *= 2;
            permParam.num_add_speed += 10;
            player.GetComponent<PlayerZombie>().speed += 0.1f * player.GetComponent<PlayerZombie>().speed;
        }
        CheckAllWeapon();
    }
    public void PurchaseRange()
    {
        current_coin = PlayerPrefs.GetFloat("Coin");
        if (current_coin >= permParam.price_current_range && permParam.num_add_range < permParam.max_range)
        {
            coinManager.MinusCoin(permParam.price_current_range);
            permParam.price_current_range *= 2;
            permParam.num_add_range += 10;
            player.GetComponent<LevelManager>().LevelUpRange();
        }
        CheckAllWeapon();
    }


    public void PurchaseNumWeapon()
    {
        current_coin = PlayerPrefs.GetFloat("Coin");
        if (current_coin >= permParam.price_current_throw && permParam.num_max_throw < permParam.max_throw)
        {
            coinManager.MinusCoin(permParam.price_current_throw);
            permParam.price_current_throw *= 2;
            permParam.num_max_throw += 1;
        }
        CheckAllWeapon();
    }
    private void CheckNumRange(int num)
    {
        maxSkill[num].SetActive(true);
        button[num].SetActive(false);
    }
    public void CheckAllWeapon()
    {
        if (permParam.num_add_shield >= permParam.max_shield)
        {
            CheckNumRange(0);
        }
        price[0].text = permParam.price_current_shield.ToString();
        paramSkill[0].text = permParam.num_add_shield + " Times";
        if (permParam.num_add_speed >= permParam.max_speed)
        {
            CheckNumRange(1);
        }
        price[1].text = permParam.price_current_speed.ToString();
        paramSkill[1].text = "+" + permParam.num_add_speed + "% Speed";
        if (permParam.num_add_range >= permParam.max_range)
        {
            CheckNumRange(2);
        }
        price[2].text = permParam.price_current_range.ToString();
        paramSkill[2].text = "+" + permParam.num_add_range + "% Range";
        if (permParam.num_max_throw >= permParam.max_throw)
        {
            CheckNumRange(3);
        }
        paramSkill[3].text = "Max: " + permParam.num_max_throw;
        price[3].text = permParam.price_current_throw.ToString();
    }

}
