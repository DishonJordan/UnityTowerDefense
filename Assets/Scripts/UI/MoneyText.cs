using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    public Text text;
    private Bank bank;

    private void Start()
    {
        bank = Bank.instance;
        if(bank == null)
        {
            Debug.LogError("MoneyText could not find Bank instance!");
        }
    }

    void Update()
    {
        if(bank != null)
        {
            text.text = "$" + bank.Money;
        }
        else
        {
            text.text = "No Bank";
        }
    }
}
