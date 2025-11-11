using System;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    private const int payoutRoundAmount = 2;
    
    [field: SerializeField] public TMP_Text redColorField { get; private set; }
    [field: SerializeField] public TMP_Text blackColorField { get; private set; }
    [field: SerializeField] public TMP_Text greenColorField { get; private set; }
    [field: SerializeField] public TMP_Text numbersField { get; private set; }
    [field: SerializeField] public TMP_Text creditsField { get; private set; }
    [field: SerializeField] public TMP_Text betSizeField { get; private set; }
    [field: SerializeField] public TMP_Dropdown chosenColorField { get; private set; }
    [field: SerializeField] public TMP_InputField chosenNumberField { get; private set; }
    [field: SerializeField] public UpgradeGridBehavior upgradeGrid { get; private set; }
    [field: SerializeField] public DisplayDialogue dialogueMenuParent { get; private set; }
    [SerializeField] private GameObject mainMenuParent;
    [SerializeField] private GameObject shopMenuParent;

    private void Start()
    {
        shopMenuParent.GetComponentInChildren<UpgradeGridBehavior>().Redraw();
    }

    public void UnloadAllMenus()
    {
        mainMenuParent.SetActive(false);
        shopMenuParent.SetActive(false);
    }
    
    public void LoadMainMenu()
    {
        UnloadAllMenus();
        mainMenuParent.SetActive(true);
    }

    public void LoadShop()
    {
        UnloadAllMenus();
        shopMenuParent.SetActive(true);
    }

    public void FixChosenNumberValue()
    {
        chosenNumberField.text = Mathf.Max(Mathf.Min(GameManager.rouletteSlotAmount - 1, int.Parse(chosenNumberField.text)), 0).ToString();
    }

    public void UpdateAllFields()
    {
        UpdateColorFields();
        UpdateNumbersField();
        UpdateCreditsField();
        UpdateBetSizeField();
    }

    public void UpdateColorFields()
    {
        UpdateRedColorField();
        UpdateBlackColorField();
        UpdateGreenColorField();
    }

    public void UpdateRedColorField() => redColorField.text = $"Red: {GameManager.Instance.RedSlotAmount}/{GameManager.rouletteSlotAmount} - Payout: x{MathF.Round(GameManager.Instance.RedPayout, payoutRoundAmount)}";

    public void UpdateBlackColorField() => blackColorField.text = $"Black: {GameManager.Instance.BlackSlotAmount}/{GameManager.rouletteSlotAmount} - Payout: x{MathF.Round(GameManager.Instance.BlackPayout, payoutRoundAmount)}";

    public void UpdateGreenColorField() => greenColorField.text = $"Green: {GameManager.Instance.GreenSlotAmount}/{GameManager.rouletteSlotAmount} - Payout: x{MathF.Round(GameManager.Instance.GreenPayout, payoutRoundAmount)}";

    public void UpdateNumbersField()
    {
        string text = "Numbers:\n";
        for (int i = 0; i < GameManager.rouletteSlotAmount; i++)
        {
            text += $"{GameManager.Instance.numbers[i].color.ToString()[0]}{GameManager.Instance.numbers[i].num} ";
        }
        text += $"\nPayout: x{MathF.Round(GameManager.Instance.NumberPayout, payoutRoundAmount)}";
        numbersField.text = text;
    }

    public void UpdateCreditsField() => creditsField.text = $"Credits: {(GameManager.Instance.CurrencyAmount > 1000000 ? GameManager.Instance.CurrencyAmount.ToString("E2") : GameManager.Instance.CurrencyAmount)}";

    public void UpdateBetSizeField() => betSizeField.text = $"Bet Size: {(GameManager.Instance.BetSize > 1000000 ? GameManager.Instance.BetSize.ToString("E2") : GameManager.Instance.BetSize)}";
}