// using UnityEngine;
//
// public class BusinessManager : MonoBehaviour
// {
//     [SerializeField] private Business[] businesses;
//     [SerializeField] private UnityEngine.UIElements.ProgressBar[] progressBars;
//
//     [SerializeField] private Business businessPrefab;
//     [SerializeField] private UnityEngine.UIElements.ProgressBar progressBarPrefab;
//     [SerializeField] private Transform businessParent;
//     private int numBusinesses = 0;
//     // ...
//
//     private void Start()
//     {
//         for (int i = 0; i < numBusinesses; i++)
//         {
//             // Create a new Business object
//             Business newBusiness = Instantiate(businessPrefab, businessParent);
//             // Assign the corresponding ProgressBar
//             newBusiness = Instantiate(progressBarPrefab, newBusiness.transform);
//             // ...
//         }
//     }
// }