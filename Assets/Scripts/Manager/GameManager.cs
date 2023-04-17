using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Manager
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI balanceText;

        public BusinessConfig config;
        private List<Business> businesses = new List<Business>();
        public BusinessConfig[] businessConfigs;
        public GameObject businessPrefab;
        public Transform businessListTransform;

        public float balance;

        private void Start()
        {
            balance = 2f;
            UpdateBalanceText();
            foreach (var t in businessConfigs)
            {
                GameObject businessObject = Instantiate(businessPrefab, businessListTransform);
                Business business = businessObject.GetComponent<Business>();
                business.Setup(t, this);
                businesses.Add(business);
            }

            businesses[0].BuyLevel();
        }

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

        private void UpdateBalanceText()
        {
            balanceText.text = "$" + balance.ToString("0.00");
        }
        
    }
    
    
}