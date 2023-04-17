using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "New Business Config", menuName = "Game/Business Config")]
    public class BusinessConfig : ScriptableObject
    {
        public float baseIncome;
     
        public float baseCost;
        public string businessName;
        public float baseCashPerRound;
        public float upgrade1Multiplier;
        public float upgrade2Multiplier;
        public int maxLevel;
        public float upgradeBenefit;
    }
}