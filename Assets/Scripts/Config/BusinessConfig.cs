using UnityEngine;

[CreateAssetMenu(fileName = "New Business Config", menuName = "Game/Business Config")]
public class BusinessConfig : ScriptableObject
{
    [field: SerializeField] public float baseCost { get; private set; }
    [field: SerializeField] public string businessName { get; private set; }
    [field: SerializeField] public float baseCashPerRound { get; private set; }
    [field: SerializeField] public int upgrade1Multiplier { get; private set; }
    [field: SerializeField] public int upgrade2Multiplier { get; private set; }
    [field: SerializeField] public int maxLevel { get; private set; }
    [field: SerializeField] public float delay { get; private set; }
}