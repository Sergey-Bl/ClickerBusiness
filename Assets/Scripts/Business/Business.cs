using DefaultNamespace;
using DefaultNamespace.Manager;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using UnityEngine.UI;
using System.Collections;

public class Business : MonoBehaviour
{
    [SerializeField] private string businessName;

    [SerializeField] private int level = 1;
    public float currentCash = 0f;

    [SerializeField] private float baseCost = 0f;
    [SerializeField] private float baseCashPerRound = 0f;
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private float upgrade1Multiplier = 0f;
    [SerializeField] private float upgrade2Multiplier = 0f;
    [SerializeField] private float delay = 0f;
    [SerializeField] private Slider slider;

    public float currentProgress = 0f;
    public float upgrade2Bonus = 0f;

    [SerializeField] private bool isUpgrade1Upgraded = false;
    [SerializeField] private bool isUpgrade2Upgraded = false;

    [SerializeField]
    private BalanceManager balanceManager;
    

    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI cashLabel;
    [SerializeField] private TextMeshProUGUI costLabel;
    [SerializeField] private TextMeshProUGUI upgradeMultiplier1;
    [SerializeField] private TextMeshProUGUI upgradeMultiplier2;

    [SerializeField] private Button upgrade1Button;
    [SerializeField] private Button upgrade2Button;
    [SerializeField] private Button levelButton;

    private float GetLevelCost(int level)
    {
        return (level + 1) * baseCost;
    }

    public float GetCashPerRound()
    {
        // return level * baseCashPerRound + (GetUpgradeMultiplier1() + GetUpgradeMultiplier2());
        float cashPerRound = level * baseCashPerRound;

        if (isUpgrade1Upgraded)
        {
            cashPerRound += GetUpgradeMultiplier1();
        }

        if (isUpgrade2Upgraded)
        {
            cashPerRound += GetUpgradeMultiplier2();
        }

        return cashPerRound;
    }

    private float GetUpgradeMultiplier1()
    {
        return upgrade1Multiplier;
    }

    private float GetUpgradeMultiplier2()
    {
        return upgrade2Multiplier;
    }

    private void Start()
    {
        UpdateProgressBar();
        UpdateUI();

        if (level >= 1)
        {
            StartCoroutine(CollectCash());
        }
        
        upgradeMultiplier1.text = upgrade1Multiplier.ToString("+ 0.00");;
        upgradeMultiplier2.text = upgrade1Multiplier.ToString("0.00") + "%";
    
    }

    public void UpdateUI()
    {
        nameLabel.text = businessName;
        levelLabel.text = "Level: " + level.ToString();
        cashLabel.text = "$" + currentCash.ToString("0.00");
        costLabel.text = "Level UP: $" + GetLevelCost(level).ToString("0.00");
        cashLabel.text += "\n+" + upgrade2Bonus.ToString("0.00") + "%";
    }

    public void BuyUpgrade1()
    {
        // GameManager gameManager = FindObjectOfType<GameManager>();
        // gameManager.balance -= 0;
        upgrade1Button.interactable = false;
        currentCash += upgrade1Multiplier;
        isUpgrade1Upgraded = true;
        UpdateUI();
    }

    public void BuyUpgrade2()
    {
        // GameManager gameManager = FindObjectOfType<GameManager>();
        // gameManager.balance -= currentCash;
        upgrade2Button.interactable = false;
        // currentCash += upgrade2Multiplier / 100;
        upgrade2Bonus = currentCash * (1 + upgrade2Multiplier / 100); // calculate upgrade2 bonus as a percentage of current cash
        isUpgrade2Upgraded = true;
        UpdateUI();
    }
    

    // ReSharper disable Unity.PerformanceAnalysis
    public void UpdateProgressBar()
    {
        currentProgress += Time.deltaTime / delay;

        if (currentProgress >= 1.0f)
        {
            BalanceManager balanceManager = FindObjectOfType<BalanceManager>();

            // BalanceManager balanceManager = gameObject.GetComponent<BalanceManager>();
            balanceManager.AddToBalance(currentCash + upgrade2Bonus); // добавляем текущий доход и бонус от upgrade2

            currentCash = 0f;
            // upgrade2Bonus = 0f; // обнуляем бонус

            currentProgress = 0f;
        }

        float progressValue = currentProgress;
        if (slider != null)
        {
            slider.value = progressValue;
        }
    }


    public IEnumerator CollectCash()
    {
        while (level < 1)
        {
            yield return null;
        }

        // isCollectingCash = true;

        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < delay)
            {
                elapsedTime += Time.deltaTime;
                currentProgress = elapsedTime / delay;
                UpdateProgressBar();
                yield return null;
            }

            currentCash += GetCashPerRound();
            // currentProgress = 0f;
            UpdateUI();
        }
    }

    public void Setup(BusinessConfig config, PrefabCreatorManager manager)
    {
        businessName = config.businessName;
        baseCost = config.baseCost;
        baseCashPerRound = config.baseCashPerRound;
        upgrade1Multiplier = config.upgrade1Multiplier;
        upgrade2Multiplier = config.upgrade2Multiplier;
        maxLevel = config.maxLevel;
        delay = config.delay;

        UpdateUI();
    }

    public void BuyLevel()
    {
        BalanceManager balanceManager = FindObjectOfType<BalanceManager>();
        if (balanceManager.RemoveFromBalance(GetLevelCost(level)) && level < maxLevel)
        {
            level++;
            UpdateUI();
            if (level == 1)
            {
                StartCoroutine(CollectCash());
            }
            if (level == maxLevel)
            {
                levelButton.interactable = false;
            }
        }
    }
}