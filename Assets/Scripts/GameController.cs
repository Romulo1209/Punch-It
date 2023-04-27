using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Informations Holder")]
    [SerializeField] private MainCharacterProgression mainCharacterProgression;

    [Header("Money Informations")]
    [SerializeField] private int actualMoney = 0;

    [Header("Level Informations")]
    [SerializeField] private int actualLevel = 1;
    [SerializeField] private float actualXp;
    [SerializeField] private float actualLevelUpXP;
    [SerializeField] private float actualForce;
    [SerializeField] private float actualVelocity;
    [SerializeField] private int actualCarryCount;

    [Header("References")]
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] Image levelProgression;

    public static GameController Instance;

    //Getters & Setters
    public int ActualLevel { get { return actualLevel; } }
    public int ActualMoney { get { return actualMoney; } set { actualMoney = value; UpdateInformations(); } }

    private void Awake() => Instance = this;

    private void Start() {
        UpdateInformations();
    }

    int GetNextLevel() {
        var level = actualLevel;
        var maxLevel = mainCharacterProgression.MaxLevel;
        if (level < maxLevel) {
            level++;
            actualLevel = level;
            actualXp = 0;
            return level;
        }
        return maxLevel;
    }
    public void LevelUp()
    {
        GetNextLevel();
        UpdateInformations();
    }

    public void GetReward(EnemyScriptable enemy)
    {
        actualMoney += enemy.EnemyCoins;
        actualXp += enemy.EnemyXP;
        if(actualXp >= actualLevelUpXP) {
            LevelUp();
            return;
        }
        UpdateUI();
    }

    public void UpdateInformations()
    {
        if(actualLevel > mainCharacterProgression.MaxLevel)
            actualLevel = mainCharacterProgression.MaxLevel;
        if (actualLevel < 1)
            actualLevel = 1;

        actualLevelUpXP = mainCharacterProgression.ExpCurve.Evaluate(actualLevel - 1);
        actualForce = mainCharacterProgression.ForceCurve.Evaluate(actualLevel - 1);
        actualVelocity = mainCharacterProgression.VelocityCurve.Evaluate(actualLevel - 1) + ShopController.Instance.AdditionalVelocity;
        actualCarryCount = Mathf.RoundToInt(mainCharacterProgression.CarryCurve.Evaluate(actualLevel - 1)) + ShopController.Instance.AdditionalCarry;

        PlayerController.Instance.PlayerForce = actualForce;
        PlayerController.Instance.PlayerVelocity = actualVelocity;
        StackController.Instance.MaxStackCount = actualCarryCount;

        PlayerController.Instance.ChangeColor(actualLevel - 1);

        UpdateUI();
    }
    void UpdateUI()
    {
        moneyText.text = actualMoney.ToString();
        levelText.text = actualLevel.ToString();

        if (actualLevel < 6)
            levelProgression.fillAmount = (actualXp / actualLevelUpXP);
        else
            levelProgression.fillAmount = 1;
    }
}