// using DefaultNamespace.Manager;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
//
//
//     public class ProgressBar : MonoBehaviour
//     {
//         [SerializeField] private float delay = 3f;
//         [SerializeField] private Slider slider;
//
//         
//         public Business business;
//         public GameManager gameManager;
//
//         public void UpdateProgressBar()
//         {
//             business.currentProgress += Time.deltaTime / delay;
//
//             if (business.currentProgress >= 1.0f)
//             {
//                 // Calculate excess progress
//                 float excessProgress = business.currentProgress - 1.0f;
//
//                 // Add excess progress to current cash
//                 business.currentCash += business.GetCashPerRound() * excessProgress;
//
//                 // Add current cash to gameManager balance
//                 gameManager.AddToBalance(business.currentCash);
//
//                 // Reset current cash to zero
//                 business.currentCash = 0f;
//
//                 // Reset current progress to the excess progress value
//                 business.currentProgress = excessProgress;
//             }
//
//             // Set progress bar value
//             float progressValue = business.currentProgress;
//             if (slider != null)
//             {
//                 slider.value = progressValue;
//             }
//         }
//
//         public IEnumerator CollectCash()
//         {
//             business.isCollectingCash = true;
//
//             while (true)
//             {
//                 float elapsedTime = 0f;
//                 while (elapsedTime < delay)
//                 {
//                     elapsedTime += Time.deltaTime;
//                     business.currentProgress = elapsedTime / delay;
//                     UpdateProgressBar();
//                     yield return null;
//                 }
//
//                 business.currentCash += business.GetCashPerRound();
//                 business.currentProgress = 0f;
//                 business.UpdateUI();
//             }
//         }
//     }