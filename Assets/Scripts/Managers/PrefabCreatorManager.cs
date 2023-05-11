using System.Collections.Generic;
using UnityEngine;

    public class PrefabCreatorManager : MonoBehaviour
    {
        private List<Business> businesses = new List<Business>();
        
        [SerializeField] private BusinessConfig[] businessConfigs;
        
        [SerializeField] private GameObject businessPrefab;

        [SerializeField] private BalanceManager balanceManager;
        
        [SerializeField] private Transform businessListTransform;

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
