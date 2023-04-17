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

    [SerializeField] public BusinessConfig config;

    public float currentProgress = 0f;
    public bool isCollectingCash = false;

    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI cashLabel;
    [SerializeField] private TextMeshProUGUI costLabel;
    
    [SerializeField] private Button upgrade1Button;
    [SerializeField] private Button upgrade2Button;
    [SerializeField] private Button levelButton;

    private float upgrade1Price
    {
        get
        {
            return GetLevelCost(level) * upgrade1Multiplier;
        }
    }

    private float upgrade2Price
    {
        get
        {
            return GetLevelCost(level) * upgrade2Multiplier;
        }
    }

    private float GetLevelCost(int level)
    {
        return (level + 1) * baseCost;
    }

    public float GetCashPerRound()
    {
        return level * baseCashPerRound * (1 + GetUpgradeMultiplier1() + GetUpgradeMultiplier2());
    }

    private float GetUpgradeMultiplier1()
    {
        float multiplier = 0f;
        // TODO: Implement logic for getting upgrade 1 multiplier
        return multiplier;
    }

    private float GetUpgradeMultiplier2()
    {
        float multiplier = 0f;
        // TODO: Implement logic for getting upgrade 2 multiplier
        return multiplier;
    }

    private void Start()
    {
        UpdateProgressBar();
        UpdateUI();

        if (level >= 1)
        {
            StartCoroutine(CollectCash());
        }
        else
        {
            isCollectingCash = false;
        }
    }

    private void Update()
    {
        if (isCollectingCash)
        {
            return;
        }
    }

    public void UpdateUI()
    {
        nameLabel.text = businessName;
        levelLabel.text = "Level: " + level.ToString();
        cashLabel.text = "Cash: $" + currentCash.ToString("0.00");
        costLabel.text = "Upgrade Cost: $" + GetLevelCost(level).ToString("0.00");
    }

    public void BuyUpgrade1()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.balance >= upgrade1Price)
        {
            gameManager.balance -= upgrade1Price;
            upgrade1Button.interactable = false;
            currentCash += config.upgradeBenefit;
            UpdateUI();
        }
    }

    public void BuyUpgrade2()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.balance >= upgrade2Price)
        {
            gameManager.balance -= upgrade2Price;
            upgrade2Button.interactable = false;
            UpdateUI();
        }
    }

    public void UpdateProgressBar()
    {
        currentProgress += Time.deltaTime / delay;

        if (currentProgress >= 1.0f)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            // Calculate excess progress
            float excessProgress = currentProgress - 1.0f;

            // Add excess progress to current cash
            currentCash += GetCashPerRound();

            // Add current cash to gameManager balance
            gameManager.AddToBalance(currentCash);

            // Reset current cash to zero
            currentCash = 0f;

            // Reset current progress to the excess progress value
            currentProgress = excessProgress;
        }

        // Set progress bar value
        float progressValue = currentProgress;
        if (slider != null)
        {
            slider.value = progressValue;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator CollectCash()
    {
        while (level < 1)
        {
            yield return null;
        }

        isCollectingCash = true;

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
            currentProgress = 0f;
            UpdateUI();
        }
    }

    public void Setup(BusinessConfig config, GameManager manager)
    {
        businessName = config.businessName;
        baseCost = config.baseCost;
        baseCashPerRound = config.baseCashPerRound;
        upgrade1Multiplier = config.upgrade1Multiplier;
        upgrade2Multiplier = config.upgrade2Multiplier;
        maxLevel = config.maxLevel;

        UpdateUI();
    }

    public void BuyLevel()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.RemoveFromBalance(GetLevelCost(level)) && level < maxLevel)
        {
            level++;
            UpdateUI();
            if (level == 1 && !isCollectingCash)
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