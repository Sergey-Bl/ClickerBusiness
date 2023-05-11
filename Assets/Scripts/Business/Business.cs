using System;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using UnityEngine.UI;
using System.Collections;

public class Business : MonoBehaviour
{
    public float currentCash { get; private set; }
    public float currentProgress { get; private set; }
    public float upgrade2Bonus { get; private set; }

    [SerializeField] private string businessName;
    [SerializeField] private int level = 1;
    [SerializeField] private float baseCost;
    [SerializeField] private float baseCashPerRound;
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int upgrade1Multiplier;
    [SerializeField] private int upgrade2Multiplier;
    [SerializeField] private float delay = 3f;

    [SerializeField] private Slider slider;

    [SerializeField] private bool isUpgrade1Upgraded;
    [SerializeField] private bool isUpgrade2Upgraded;

    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private TextMeshProUGUI _levelLabel;
    [SerializeField] private TextMeshProUGUI _cashLabel;
    [SerializeField] private TextMeshProUGUI _costLabel;
    [SerializeField] private TextMeshProUGUI _upgradeMultiplier1;
    [SerializeField] private TextMeshProUGUI _upgradeMultiplier2;

    [SerializeField] private Button upgrade1Button;
    [SerializeField] private Button upgrade2Button;
    [SerializeField] private Button levelButton;

    private BalanceManager _balanceManager;

    private void Awake()

    {
        _balanceManager = FindObjectOfType<BalanceManager>();
    }

    private void Start()
    {
        UpdateProgressBar();
        UpdateUI();

        if (level >= 1)
        {
            StartCoroutine(CollectCash());
        }

        _upgradeMultiplier1.text = upgrade1Multiplier.ToString("+ 0");
        _upgradeMultiplier2.text = upgrade2Multiplier.ToString("0") + "%";
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

    public void UpdateUI()
    {
        _nameLabel.text = businessName;
        _levelLabel.text = "Level: " + level.ToString();
        _cashLabel.text = "$" + currentCash.ToString("0.00");
        _costLabel.text = "Level UP: $" + GetLevelCost(level).ToString("0.00");
        _cashLabel.text += "\n+" + upgrade2Bonus.ToString("0") + "%";
    }

    public float GetCashPerRound()
    {
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

    private float GetLevelCost(int level)
    {
        return (level + 1) * baseCost;
    }

    public void BuyUpgrade1()
    {
        upgrade1Button.interactable = false;
        currentCash += upgrade1Multiplier;
        isUpgrade1Upgraded = true;
        UpdateUI();
    }

    public void BuyUpgrade2()
    {
        upgrade2Button.interactable = false;
        upgrade2Bonus = currentCash * (1 + upgrade2Multiplier / 100);
        isUpgrade2Upgraded = true;
        UpdateUI();
    }

    public void UpdateProgressBar()
    {
        currentProgress += Time.deltaTime / delay;

        if (currentProgress >= 1.0f)
        {
            _balanceManager.AddToBalance(currentCash + upgrade2Bonus);

            currentCash = 0f;
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
            UpdateUI();
        }
    }

    public void BuyLevel()
    {
        if (!_balanceManager.RemoveFromBalance(GetLevelCost(level)) || level >= maxLevel) return;
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