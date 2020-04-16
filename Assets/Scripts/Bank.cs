using System;
using UnityEngine;

public class Bank : MonoBehaviour
{
	[SerializeField]
	private int money;

	public int Money
	{
		get => money;

		private set
		{
			money = value;
			OnMoneyChanged?.Invoke(money);
		}
	}

	public Action<int> OnMoneyChanged = delegate { };

	public bool CanDepositMoney(int amount) => !(amount < 0);
	public bool CanWithdrawMoney(int amount) => !(amount < 0 || amount > money);

	public bool DepositMoney(int amount)
	{
		if (!CanDepositMoney(amount))
			return false;

		Money += amount;
		return true;
	}

	public bool WithdrawMoney(int amount)
	{
		if (!CanWithdrawMoney(amount))
			return false;

		Money -= amount;
		return true;
	}

}
