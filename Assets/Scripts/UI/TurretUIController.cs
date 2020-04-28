using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class TurretUIController : MonoBehaviour
{
    [Header("Turret")]
    private Turret turret;

    [Header("Buttons")]
    public Button sellButton;
    public Button upgradeButton;
    public Button repairButton;
    public List<Button> exitButtons;

    [Header("Sprites")]
    public Sprite canPurchaseSprite;
    public Sprite cannotPurchaseSprite;

    private List<int> prices;
    private List<Button> buttons;
    private List<TextMeshProUGUI> texts;
    public List<Image> images;

    private void Start()
    {
        turret = GetComponentInParent<Turret>();
        Assert.IsNotNull(turret, "Turret UI could not find turret.");

        buttons = new List<Button> { sellButton, upgradeButton, repairButton };
        prices = new List<int> { turret.sellCost, turret.upgradeCost, turret.repairCost };
        texts = buttons.Select(button => button.GetComponentInChildren<TextMeshProUGUI>(true)).ToList();
        images = buttons.Select(button => button.GetComponent<Image>()).ToList();

        SetButtonPriceText();
        LinkButtonsToTurret();
    }

    private void LinkButtonsToTurret()
    {
        // Remove existing listeners
        sellButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        repairButton.onClick.RemoveAllListeners();
        exitButtons.ForEach(button => button.onClick.RemoveAllListeners());

        // Add new listeners
        sellButton.onClick.AddListener(turret.SellTurret);
        upgradeButton.onClick.AddListener(turret.UpgradeTurret);
        repairButton.onClick.AddListener(turret.RepairTurret);
        exitButtons.ForEach(button => button.onClick.AddListener(turret.DisableTurretUI));
    }

    private void SetButtonPriceText()
    {
        for (int i = 0; i < texts.Count; i++)
        {
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

	private void Update()
	{
		for (int i = 1; i < buttons.Count; i++)
		{
			images[i].sprite = Bank.instance.CanWithdrawMoney(prices[i]) ? canPurchaseSprite : cannotPurchaseSprite;
		}
	}
}
