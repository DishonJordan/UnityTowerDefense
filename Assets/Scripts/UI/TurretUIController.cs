using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class TurretUIController : MonoBehaviour
{
    [Header("Turret")]
    public GameObject turret;

    [Header("Buttons")]
    public Button sellButton;
    public Button upgradeButton;
    public Button repairButton;
    public List<Button> exitButtons;

    [Header("Sprites")]
    public Sprite canPurchaseSprite;
    public Sprite cannotPurchaseSprite;

    [Header("Stats Panel")]
    public TextMeshProUGUI turretNameText;
    public TextMeshProUGUI statDamageText;
    public TextMeshProUGUI statSpeedText;
    public TextMeshProUGUI statRangeText;
    public TextMeshProUGUI statHealthText;

    private Turret _turret;
    private List<int> prices;
    private List<Button> buttons;
    private List<TextMeshProUGUI> texts;
    private List<Image> images;

    private void Start()
    {
        _turret = turret.GetComponent<Turret>();
        Assert.IsNotNull(turret, "Turret UI could not find turret.");

        buttons = new List<Button> { sellButton, upgradeButton, repairButton };
        prices = new List<int> { _turret.sellCost, _turret.upgradeCost, _turret.repairCost };
        texts = buttons.Select(button => button.GetComponentInChildren<TextMeshProUGUI>(true)).ToList();
        images = buttons.Select(button => button.GetComponent<Image>()).ToList();

        SetButtonPriceText();
        SetStatsPanelText();
        LinkButtonsToTurret();
    }

    private void SetStatsPanelText()
    {
        turretNameText.text = _turret.turretName;
        statDamageText.text = "DMG: " + _turret.turretProjectile.GetComponent<Projectile>().projectileDamage;
        statSpeedText.text = "SPD: " + _turret.fireRate;
        statRangeText.text = "RNG: " + _turret.fireRange;
        statHealthText.text = "HP: " + _turret.health + "/" + _turret.maxHealth;
    }

    private void LinkButtonsToTurret()
    {
        // Remove existing listeners
        sellButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        repairButton.onClick.RemoveAllListeners();
        exitButtons.ForEach(button => button.onClick.RemoveAllListeners());

        // Add new listeners
        sellButton.onClick.AddListener(() => _turret.RequestModification(Task.Type.Sell));
        upgradeButton.onClick.AddListener(() => _turret.RequestModification(Task.Type.Upgrade));
        repairButton.onClick.AddListener(() => _turret.RequestModification(Task.Type.Repair));
        exitButtons.ForEach(button => button.onClick.AddListener(_turret.DisableTurretUI));
    }

    private void SetButtonPriceText()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            switch (i)
            {
                case 0: // Sell Case
                    texts[i].SetText("-$" + prices[i]);
                    break;
                case 1: // Upgrade Case
                    if (_turret.nextUpgrade == null)
                    {
                        texts[i].SetText("MAX");
                    }
                    else
                    {
                        texts[i].SetText("$" + prices[i]);
                    }
                    break;
                default:
                    texts[i].SetText("$" + prices[i]);
                    break;
            }
        }
    }

    private void Update()
    {
        for (int i = 1; i < buttons.Count; i++)
        {
            switch (i)
            {
                case 1:
                    if (_turret.nextUpgrade == null)
                    {
                        images[i].sprite = cannotPurchaseSprite;
                    }
                    else
                    {
                        images[i].sprite = Bank.instance.CanWithdrawMoney(prices[i]) ? canPurchaseSprite : cannotPurchaseSprite;
                    }
                    break;
                default:
                    images[i].sprite = Bank.instance.CanWithdrawMoney(prices[i]) ? canPurchaseSprite : cannotPurchaseSprite;
                    break;
            }
        }
        statHealthText.text = "HP: " + _turret.health + "/" + _turret.maxHealth;
    }

    public void ChangeButtonInteractivity(bool status)
    {
        sellButton.interactable = status;
        upgradeButton.interactable = status;
        repairButton.interactable = status;
    }
}
