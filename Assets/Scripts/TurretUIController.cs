using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject turret;
    public Button[] buttons;

    public TextMeshProUGUI[] texts;

    [Header("Sprites")]
    public Sprite canPurchaseSprite;
    public Sprite cannotPurchaseSprite;

    private Turret turretScript;
    private int[] prices;

    private void Start()
    {
        prices = new int[buttons.Length];

        turretScript = turret.GetComponent<Turret>();
        if (turretScript != null)
        {
            prices[0] = turretScript.sellCost;
            prices[1] = turretScript.upgradeCost;
            prices[2] = turretScript.repairCost;

            for (int i = 0; i < texts.Length; i++) {
                if (i == 0)
                {
                    texts[i].SetText("-$" + prices[i]);
                }
                else
                {
                    texts[i].SetText("$" + prices[i]);
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = Bank.instance.CanWithdrawMoney(prices[i]) ? canPurchaseSprite : cannotPurchaseSprite;
        }
    }
}
