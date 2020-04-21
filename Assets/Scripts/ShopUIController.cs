using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject tile;
    public GameObject[] turrets; //Will all be the same price because all turrets are referencing the first one
    public GameObject[] buttons;
    public GameObject[] priceTexts;

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
            priceTexts[i].GetComponent<Text>().text = "$" + prices[i];
        }
    }

    private void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = Bank.instance.CanWithdrawMoney(prices[i]) ? canPurchaseSprite : cannotPurchaseSprite;
        }
    }
}
