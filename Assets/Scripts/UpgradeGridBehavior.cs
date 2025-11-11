using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjects;

public class UpgradeGridBehavior : MonoBehaviour
{
    private const int menuWidth = 1600;
    private const int menuHeight = 800;

    private const int upgradeTemplateWidth = 100;
    private const int upgradeTemplateHeight = 100;
    
    [SerializeField] private UpgradeSlotRefs upgradeTemplatePrefab;
    [SerializeField] private UpgradeList upgradeList;

    private List<UpgradeSlotRefs> upgradeSlots = new();
    private List<UpgradeData> upgrades = new();
    
    private float upgradeRowAmount = 2;
    private float upgradeColAmount = 2;

    private void Awake()
    {
        foreach (var u in upgradeList.upgrades)
        {
            var upgrade = ScriptableObject.CreateInstance<UpgradeData>();
            upgrade.LoadValues(u);
            upgrades.Add(upgrade);
        }
    }

    public void Redraw()
    {
        foreach (var slot in upgradeSlots)
        {
            Destroy(slot.gameObject);
        }
        upgradeSlots.Clear();
        
        float scale = Mathf.Min(menuWidth / (upgradeTemplateWidth * upgradeColAmount), menuHeight / (upgradeTemplateHeight * upgradeRowAmount));
        for (int i = 0; i < upgradeRowAmount; i++)
        {
            for (int j = 0; j < upgradeColAmount; j++)
            {
                UpgradeSlotRefs upgradeSlot = Instantiate(upgradeTemplatePrefab.gameObject, transform).GetComponent<UpgradeSlotRefs>();
                upgradeSlots.Add(upgradeSlot);
                upgradeSlot.transform.localScale = new(scale, scale);
                upgradeSlot.transform.localPosition = new ((j + 0.5f - upgradeColAmount / 2) * upgradeTemplateWidth * scale, (i + 0.5f - upgradeRowAmount / 2) * upgradeTemplateHeight * scale);
                RollSlot(upgradeSlot);
            }
        }
    }

    public void RefreshAllSlotsWithCost(int cost)
    {
        if (cost > GameManager.Instance.CurrencyAmount) return;
        GameManager.Instance.CurrencyAmount -= cost;
        foreach (var slot in upgradeSlots)
        {
            RollSlot(slot);
        }
    }
    
    public void RefreshPrices(UpgradeData upgrade)
    {
        foreach (var slot in upgradeSlots.Where(slot => slot.HeldUpgrade == upgrade))
        {
            slot.Refresh();
        }
    }

    public void RollSlot(UpgradeSlotRefs slot)
    {
        slot.HeldUpgrade = upgrades[GameManager.Instance.rng.Next(upgrades.Count)];
    }
}
