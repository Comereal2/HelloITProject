using System;
using System.Numerics;
using UnityEngine;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public System.Random rng = new();
    public AudioManager AudioManager;
    public BigInteger currencyAmount = 10;
    
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject playerDealer;
    [field: SerializeField] public GameplayUI gameplayUI { get; private set; }
    
    private DisplayDialogue dialogueBox;
    private Camera camera;
    
    private void Awake()
    {
        Instance = this;
        dialogueBox = gameplayUI.dialogueMenuParent;
    }

    private void Start()
    {
        camera = Camera.main;
        gameplayUI.UnloadAllMenus();
        DisablePlayerInput();
        
        SlowMove(camera.gameObject, camera.transform.forward * 12f, 2f, true, () =>
        {
            gameplayUI.gameObject.SetActive(true);
            dialogueBox.DisplayText("Hey, dev here. We uhhh kinda ran out of budget for a 3d game because of these fancy AAA assets, so let me just...", 15, 0,
                () => {
            playerDealer.transform.LookAt(camera.transform);
            SlowMove(playerDealer, Vector3.Lerp(playerDealer.transform.position, camera.transform.position, 0.7f), 2f, false, 
        () => SlowStretch(playerDealer, new Vector3(2.3f, 1, 1), 1f, true,
    () => dialogueBox.DisplayText("L O A D I M ", 2, 0, 
() => {
                                                                        dialogueBox.DisplayText("L O A D I N G", 2, 9,
() => dialogueBox.DisplayText("Now THIS is professional UI. Uhhhh, I guess I should give you a tutorial... Up on the top you got the quit button, to the right you got the shop, rest is the roulette and you can probably figure everything out. Your smart rihgt?", 15, 0, EnablePlayerInput));
                                    gameplayUI.LoadMainMenu();})));});});}

    private void OnDestroy()
    {
        Instance = null;
    }
    
    public void DisablePlayerInput()
    {
        eventSystem.SetActive(false);
    }

    public void EnablePlayerInput()
    {
        eventSystem.SetActive(true);
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    
    public static async void SlowMove(GameObject obj, Vector3 newPos, float time, bool setToRelativePos = false, [CanBeNull] Action invokeOnEnd = null)
    {
        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = setToRelativePos ? startPos + newPos : newPos;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            if (!Application.isPlaying) return;
            float t = elapsedTime / time;
            obj.transform.position = Vector3.Lerp(startPos, targetPos, t);
            await Task.Delay(20);
            elapsedTime += Time.deltaTime;
        }

        obj.transform.position = targetPos;
        invokeOnEnd?.Invoke();
    }
    
    public static async void SlowStretch(GameObject obj, Vector3 newScale, float time, bool setToRelativeScale = false, [CanBeNull] Action invokeOnEnd = null)
    {
        Vector3 startScale = obj.transform.localScale;
        Vector3 targetScale = setToRelativeScale ? Vector3.Scale(startScale, newScale) : newScale;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            if (!Application.isPlaying) return;
            float t = elapsedTime / time;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            await Task.Delay(20);
            elapsedTime += Time.deltaTime;
        }

        obj.transform.localScale = targetScale;
        invokeOnEnd?.Invoke();
    }
}
