using TMPro;
using UnityEngine;

namespace DefaultNamespace.Manager
{
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
            if (balance >= amount)
            {
                balance -= amount;
                UpdateBalanceText();
                return true;
            }

            return false;
        }

        public void UpdateBalanceText()
        {
            balanceText.text = "$" + balance.ToString("0.00");
        }
    }
}