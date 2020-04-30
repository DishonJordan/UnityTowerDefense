using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("Turrets")]
    public GameObject[] turrets;

    [Header("Sprites")]
    public Sprite canPurchaseSprite;
    public Sprite cannotPurchaseSprite;

    private BuildManager buildManager;
    private TextMeshProUGUI[] turretPriceTexts;
    private Button[] turretButtons;
    private Turret[] turretScripts;
    private int[] prices;

    private void Start()
    {
        turretButtons = GetComponentsInChildren<Button>();
        buildManager = transform.parent.parent.parent.GetComponent<BuildManager>();

        turretPriceTexts = new TextMeshProUGUI[turrets.Length];
        turretScripts = new Turret[turrets.Length];
        prices = new int[turrets.Length];

        int count = 0;
        foreach (Button b in turretButtons)
        {
            turretPriceTexts[count] = b.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            b.onClick.RemoveAllListeners();
            GameObject t = turrets[count];
            b.onClick.AddListener(() => buildManager.BuildTurret(t));
            count++;
        }

        /* Sets the prices in the Shop UI */
        for (int i = 0; i < turrets.Length; i++)
        {
            turretScripts[i] = turrets[i].GetComponentInChildren<Turret>();
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
