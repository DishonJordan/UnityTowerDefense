using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("Turrets")]
    public GameObject[] turrets;

    [Header("Buttons")]
    public Button[] turretButtons;
    public TextMeshProUGUI[] turretPriceTexts;

    [Header("Sprites")]
    public Sprite canPurchaseSprite;
    public Sprite cannotPurchaseSprite;

    private Turret[] turretScripts;
    private int[] prices;

    private void Start()
    {
        turretScripts = new Turret[turrets.Length];
        prices = new int[turrets.Length];

        /* Sets the prices in the Shop UI */
        for (int i = 0; i < turrets.Length; i++)
        {
            turretScripts[i] = turrets[i].GetComponent<Turret>();
            prices[i] = turretScripts[i].purchaseCost;
            turretPriceTexts[i].SetText("$" + prices[i]);
        }
    }

    private void Update()
    {
        for (int i = 0; i < turretButtons.Length; i++)
        {
            turretButtons[i].GetComponent<Image>().sprite = Bank.instance.CanWithdrawMoney(prices[i]) ? canPurchaseSprite : cannotPurchaseSprite;
        }
    }
}
