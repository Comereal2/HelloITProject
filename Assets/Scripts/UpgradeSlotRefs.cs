using System.Numerics;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotRefs : MonoBehaviour
{
    private string purchaseSfxKey;
    
    [SerializeField] public Image upgradeSprite;
    [SerializeField] public TMP_Text costTextBox;

    private UpgradeData _heldUpgrade;

    public UpgradeData HeldUpgrade
    {
        get => _heldUpgrade;
        set
        {
            _heldUpgrade = value;
            if (!_heldUpgrade)
            {
                upgradeSprite.sprite = null;
                costTextBox.text = "Restock Required";
                return;
            }
            upgradeSprite.sprite = value.upgradeSprite;
            costTextBox.text = ((int)value.upgradeCost).ToString();
        }
    }

    public void Purchase()
    {
        if ((BigInteger)HeldUpgrade.upgradeCost > GameManager.Instance.currencyAmount) return;
        GameManager.Instance.AudioManager.TryPlayAudio(purchaseSfxKey);
        HeldUpgrade.upgradeCost += HeldUpgrade.upgradeScale * HeldUpgrade.upgradeCost;
        HeldUpgrade = null;
    }
}
