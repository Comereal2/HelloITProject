using System;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
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
}