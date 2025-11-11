using System.Numerics;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotRefs : MonoBehaviour
{
    private string purchaseSfxKey = "Purchase";
    
    [SerializeField] public Image upgradeSprite;
    [SerializeField] public TMP_Text costTextBox;

    private UpgradeData _heldUpgrade;

    public UpgradeData HeldUpgrade
    {
        get => _heldUpgrade;
        set
        {
            _heldUpgrade = value;
            Refresh();
        }
    }

    public void Purchase()
    {
        if ((BigInteger)HeldUpgrade.upgradeCost > GameManager.Instance.currencyAmount) return;
        GameManager.Instance.AudioManager.TryPlayAudio(purchaseSfxKey);
        HeldUpgrade.upgradeCost += HeldUpgrade.upgradeScale * HeldUpgrade.upgradeCost;
        GameManager.Instance.gameplayUI.upgradeGrid.RefreshPrices(HeldUpgrade);
        HeldUpgrade = null;
    }

    public void Refresh()
    {
        if (!HeldUpgrade)
        {
            upgradeSprite.sprite = null;
            costTextBox.text = "Restock Required";
            return;
        }
        upgradeSprite.sprite = HeldUpgrade.upgradeSprite;
        costTextBox.text = ((int)HeldUpgrade.upgradeCost).ToString();
    }
}
