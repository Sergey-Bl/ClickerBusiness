using TMPro;
using UnityEngine;

public class BalanceManager : MonoBehaviour

{
    public TextMeshProUGUI balanceText;
    public float balance { get; private set; }

    public void AddToBalance(float amount)
    {
        balance += amount;
        UpdateBalanceText();
    }

    public bool RemoveFromBalance(float amount)
    {
        if (!(balance >= amount)) return false;
        balance -= amount;
        UpdateBalanceText();
        return true;
    }

    public void UpdateBalanceText()
    {
        balanceText.text = "$" + balance.ToString("0.00");
    }
}