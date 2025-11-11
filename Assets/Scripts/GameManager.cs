using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System.Threading.Tasks;
using JetBrains.Annotations;
using ScriptableObjects;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public enum SlotColor
    {
        Red = 0,
        Black = 1,
        Green = 2
    }

    public enum Stats
    {
        BetSize,
        RedAmount,
        RedPayout,
        BlackAmount,
        BlackPayout,
        GreenAmount,
        GreenPayout,
        AddNumber,
        NumberPayout,
    }

    public struct Number
    {
        public int num;
        public SlotColor color;

        public Number(int num, SlotColor color)
        {
            this.num = num;
            this.color = color;
        }
    }
    
    
    public const int rouletteSlotAmount = 37;
    
    public static GameManager Instance { get; private set; }
    
    public System.Random rng = new();
    public AudioManager AudioManager;
    
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject playerDealer;
    [SerializeField] private Button gambleButton;
    [SerializeField] private FloatingText floatingTextPrefab;
    [field: SerializeField] public GameplayUI gameplayUI { get; private set; }
    
    private DisplayDialogue dialogueBox;
    private Camera camera;

    public Number[] numbers { get; private set; } = new Number[rouletteSlotAmount];

    private BigInteger _currencyAmount = 10;
    private BigInteger _betSize = 1;
    private int _redSlotAmount = 18;
    private float _redPayout = 2;
    private int _blackSlotAmount = 18;
    private float _blackPayout = 2;
    private int _greenSlotAmount = 1;
    private float _greenPayout = 38;
    private float _numberPayout = 38;

    public BigInteger CurrencyAmount
    {
        get => _currencyAmount;
        set
        {
            _currencyAmount = value;
            gameplayUI.UpdateCreditsField();
        }
    }

    public BigInteger BetSize
    {
        get => _betSize;
        set
        {
            _betSize = value;
            gameplayUI.UpdateBetSizeField();
        }
    }
    
    public int RedSlotAmount
    {
        get => _redSlotAmount;
        set
        {
            if (_redSlotAmount + value > rouletteSlotAmount || _redSlotAmount + value < 0) return;
            int offset = value - _redSlotAmount;
            _redSlotAmount = value;
            int removedBlackAmount = Mathf.Min(_blackSlotAmount, offset);
            _blackSlotAmount -= removedBlackAmount;
            int removedGreenAmount = offset - removedBlackAmount;
            _greenSlotAmount -= removedGreenAmount;
            for (int i = 0; i < rouletteSlotAmount; i++)
            {
                if (removedBlackAmount == 0) break;
                if (removedBlackAmount < 0)
                {
                    if (numbers[i].color == SlotColor.Red)
                    {
                        numbers[i].color = SlotColor.Black;
                        removedBlackAmount++;
                    }
                }
                else
                {
                    if (numbers[i].color == SlotColor.Black)
                    {
                        numbers[i].color = SlotColor.Red;
                        removedBlackAmount--;
                    }
                }
            }

            for (int i = 0; i < rouletteSlotAmount; i++)
            {
                if (removedGreenAmount == 0) return;
                if (removedGreenAmount < 0)
                {
                    if (numbers[i].color == SlotColor.Red)
                    {
                        numbers[i].color = SlotColor.Green;
                        removedGreenAmount++;
                    }
                }
                else
                {
                    if (numbers[i].color == SlotColor.Green)
                    {
                        numbers[i].color = SlotColor.Red;
                        removedGreenAmount--;
                    }
                }
            }
            gameplayUI.UpdateColorFields();
            gameplayUI.UpdateNumbersField();
        }
    }

    public float RedPayout
    {
        get => _redPayout;
        set
        {
            _redPayout = value;
            gameplayUI.UpdateRedColorField();
        }
    }

    public int BlackSlotAmount
    {
        get => _blackSlotAmount;
        set
        {
            if (_blackSlotAmount + value > rouletteSlotAmount || _blackSlotAmount + value < 0) return;
            int offset = value - _blackSlotAmount;
            _blackSlotAmount = value;
            int removedRedAmount = Mathf.Min(_redSlotAmount, offset);
            _redSlotAmount -= removedRedAmount;
            int removedGreenAmount = offset - removedRedAmount;
            _greenSlotAmount -= removedGreenAmount;
            for (int i = 0; i < rouletteSlotAmount; i++)
            {
                if (removedRedAmount == 0) break;
                if (removedRedAmount < 0)
                {
                    if (numbers[i].color == SlotColor.Black)
                    {
                        numbers[i].color = SlotColor.Red;
                        removedRedAmount++;
                    }
                }
                else
                {
                    if (numbers[i].color == SlotColor.Red)
                    {
                        numbers[i].color = SlotColor.Black;
                        removedRedAmount--;
                    }
                }
            }

            for (int i = 0; i < rouletteSlotAmount; i++)
            {
                if (removedGreenAmount == 0) return;
                if (removedGreenAmount < 0)
                {
                    if (numbers[i].color == SlotColor.Black)
                    {
                        numbers[i].color = SlotColor.Green;
                        removedGreenAmount++;
                    }
                }
                else
                {
                    if (numbers[i].color == SlotColor.Green)
                    {
                        numbers[i].color = SlotColor.Black;
                        removedGreenAmount--;
                    }
                }
            }
            gameplayUI.UpdateColorFields();
            gameplayUI.UpdateNumbersField();
        }
    }
    
    public float BlackPayout
    {
        get => _blackPayout;
        set
        {
            _blackPayout = value;
            gameplayUI.UpdateBlackColorField();
        }
    }

    public int GreenSlotAmount
    {
        get => _greenSlotAmount;
        set
        {
            if (_greenSlotAmount + value > rouletteSlotAmount || _greenSlotAmount + value < 0) return;
            int offset = value - _greenSlotAmount;
            _greenSlotAmount = value;
            int removedRedAmount = Mathf.Min(_redSlotAmount, offset);
            _redSlotAmount -= removedRedAmount;
            int removedBlackAmount = offset - removedRedAmount;
            _blackSlotAmount -= removedBlackAmount;
            for (int i = 0; i < rouletteSlotAmount; i++)
            {
                if (removedRedAmount == 0) break;
                if (removedRedAmount < 0)
                {
                    if (numbers[i].color == SlotColor.Green)
                    {
                        numbers[i].color = SlotColor.Red;
                        removedRedAmount++;
                    }
                }
                else
                {
                    if (numbers[i].color == SlotColor.Red)
                    {
                        numbers[i].color = SlotColor.Green;
                        removedRedAmount--;
                    }
                }
            }

            for (int i = 0; i < rouletteSlotAmount; i++)
            {
                if (removedBlackAmount == 0) return;
                if (removedBlackAmount < 0)
                {
                    if (numbers[i].color == SlotColor.Green)
                    {
                        numbers[i].color = SlotColor.Black;
                        removedBlackAmount++;
                    }
                }
                else
                {
                    if (numbers[i].color == SlotColor.Black)
                    {
                        numbers[i].color = SlotColor.Green;
                        removedBlackAmount--;
                    }
                }
            }
            gameplayUI.UpdateColorFields();
            gameplayUI.UpdateNumbersField();
        }
    }

    public float GreenPayout
    {
        get => _greenPayout;
        set
        {
            _greenPayout = value;
            gameplayUI.UpdateGreenColorField();
        }
    }

    public float NumberPayout
    {
        get => _numberPayout;
        set
        {
            _numberPayout = value;
            gameplayUI.UpdateNumbersField();
        }
    }
    
    private void Awake()
    {
        numbers[0] = new (0, SlotColor.Green);
        for (int i = 1; i < rouletteSlotAmount; i++)
        {
            numbers[i] = new (i, i % 2 == 0 ? SlotColor.Black : SlotColor.Red);
        }
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
                                    gameplayUI.LoadMainMenu();})));});});
        gameplayUI.UpdateAllFields();
    }

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

    public void ModifyStat(UpgradeInfo info)
    {
        switch (info.stat)
        {
            case Stats.BetSize:
                BetSize += (BigInteger)info.amount;
                break;
            case Stats.RedAmount:
                RedSlotAmount += (int)info.amount;
                break;
            case Stats.RedPayout:
                RedPayout += info.amount;
                break;
            case Stats.BlackAmount:
                BlackSlotAmount += (int)info.amount;
                break;
            case Stats.BlackPayout:
                BlackPayout += info.amount;
                break;
            case Stats.GreenAmount:
                GreenSlotAmount += (int)info.amount;
                break;
            case Stats.GreenPayout:
                GreenPayout += info.amount;
                break;
            case Stats.AddNumber:
                for (int i = rouletteSlotAmount - 1; i > -1; i--)
                {
                    if (numbers[i].num != (int)info.amount)
                    {
                        numbers[i].num = (int)info.amount;
                        gameplayUI.UpdateNumbersField();
                        break;
                    }
                }
                break;
            case Stats.NumberPayout:
                NumberPayout += info.amount;
                break;
        }
    }

    public void Spin()
    {
        if (gameplayUI.chosenNumberField.text == String.Empty)
        {
            dialogueBox.DisplayText("Hey, uhh, you need to put in a number to gamble on. This machine is rigged in your favor because you get to gamble on both, but still.", 15);
            return;
        }

        if (CurrencyAmount < BetSize)
        {
            dialogueBox.DisplayText("Oh no. You... somehow lost. That is... Impressive. Well, you cant get any more spins, so just quit out of the game and try again!", 15);
            return;
        }
        CurrencyAmount -= BetSize;
        Number result = numbers[rng.Next(rouletteSlotAmount)];
        BigInteger amountRecieved = 0;
        TMP_Text floatingText = Instantiate(floatingTextPrefab.gameObject, gameplayUI.transform).GetComponent<TMP_Text>();
        floatingText.transform.position = gambleButton.transform.position;
        floatingText.text = result.num.ToString();
        switch (result.color)
        {
            case SlotColor.Red:
                if ((int)result.color == gameplayUI.chosenColorField.value) amountRecieved += BetSize * (BigInteger)(RedPayout * 100) / 100;
                floatingText.color = Color.red;
                break;
            case SlotColor.Black:
                if ((int)result.color == gameplayUI.chosenColorField.value) amountRecieved += BetSize * (BigInteger)(BlackPayout * 100) / 100;
                floatingText.color = Color.black;
                break;
            case SlotColor.Green:
                if ((int)result.color == gameplayUI.chosenColorField.value) amountRecieved += BetSize * (BigInteger)(GreenPayout * 100) / 100;
                floatingText.color = Color.green;
                break;
        }

        if (result.num == int.Parse(gameplayUI.chosenNumberField.text))
        {
            amountRecieved += BetSize * (BigInteger)(NumberPayout * 100) / 100;
        }

        CurrencyAmount += amountRecieved;
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
