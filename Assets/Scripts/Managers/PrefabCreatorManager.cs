using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Manager
{
    public class PrefabCreatorManager : MonoBehaviour
    {
        private List<Business> businesses = new List<Business>();
        public BusinessConfig[] businessConfigs;
        public GameObject businessPrefab;
        public Transform businessListTransform;

        [SerializeField]
        private BalanceManager balanceManager;

        private void Start()
        {
            balanceManager.UpdateBalanceText();
            foreach (var t in businessConfigs)
            {
                GameObject businessObject = Instantiate(businessPrefab, businessListTransform);
                Business business = businessObject.GetComponent<Business>();
                business.Setup(t, this);
                businesses.Add(business);
            }
        }
    }
}