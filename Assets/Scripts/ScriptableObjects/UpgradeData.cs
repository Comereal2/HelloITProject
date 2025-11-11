using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Apgrejd", menuName = "Horrible Design TM/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        public string upgradeName = "Upgrade";
        public float upgradeCost = 10;
        [Tooltip("Increased by this amount(Cost * (1 + upgradeScale)")] public float upgradeScale = 0.1f;
        public Sprite upgradeSprite;

        public void LoadValues(UpgradeData values)
        {
            upgradeName = values.upgradeName;
            upgradeCost = values.upgradeCost;
            upgradeScale = values.upgradeScale;
            upgradeSprite = values.upgradeSprite;
        }
    }
}