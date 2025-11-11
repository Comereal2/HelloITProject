using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Apgrejd List", menuName = "Horrible Design TM/Upgrade List")]
    public class UpgradeList : ScriptableObject
    {
        [SerializeField] public List<UpgradeData> upgrades;
    }
}