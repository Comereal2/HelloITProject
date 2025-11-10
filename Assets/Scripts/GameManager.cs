using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameplayUI;
    
    private void Awake()
    {
        Instance = this;
        gameplayUI.SetActive(false);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void SnapToGame()
    {
        gameplayUI.SetActive(true);
    }
}
