using System.Collections.Generic;
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
    private List<UpgradeData> upgrades;
    
    private float upgradeRowAmount = 2;
    private float upgradeColAmount = 2;

    private void Awake()
    {
        upgrades = new(upgradeList.upgrades);
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
                UpgradeData upgrade = upgrades[GameManager.Instance.rng.Next(upgrades.Count)];
                UpgradeSlotRefs upgradeSlot = Instantiate(upgradeTemplatePrefab.gameObject, transform).GetComponent<UpgradeSlotRefs>();
                upgradeSlots.Add(upgradeSlot);
                upgradeSlot.transform.localScale = new(scale, scale);
                upgradeSlot.transform.localPosition = new ((j + 0.5f - upgradeColAmount / 2) * upgradeTemplateWidth * scale, (i + 0.5f - upgradeRowAmount / 2) * upgradeTemplateHeight * scale);
                upgradeSlot.HeldUpgrade = upgrade;
            }
        }
    }
}
