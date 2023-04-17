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

    // private float upgrade1Price => GetLevelCost(level) * upgrade1Multiplier;
    //
    // private float upgrade2Price => GetLevelCost(level) * upgrade2Multiplier;

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
        return upgrade1Multiplier;
    }

    private float GetUpgradeMultiplier2()
    {
        return upgrade2Multiplier / 100f;
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
        if (gameManager.balance >= currentCash)
        {
            gameManager.balance -= currentCash;
            upgrade1Button.interactable = false;
            currentCash += config.upgrade1Multiplier;
            UpdateUI();
        }
    }

    public void BuyUpgrade2()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.balance >= currentCash)
        {
            gameManager.balance -= currentCash;
            upgrade2Button.interactable = false;
            currentCash += config.upgrade2Multiplier;
            UpdateUI();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void UpdateProgressBar()
    {
        currentProgress += Time.deltaTime / delay;

        if (currentProgress >= 1.0f)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            float excessProgress = currentProgress - 1.0f;

            currentCash += GetCashPerRound();

            gameManager.AddToBalance(currentCash);

            currentCash = 0f;

            currentProgress = excessProgress;
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
        delay = config.delay;

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