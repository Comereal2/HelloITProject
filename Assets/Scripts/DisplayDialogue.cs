using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDialogue : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text dialogueTextBox;

    private Color originalBgColor;
    private Color originalTextColor;

    private void Awake()
    {
        originalBgColor = background.color;
        originalTextColor = dialogueTextBox.color;
    }

    public async void DisplayText(string text, int charactersPerSecond, int startCharacterIndex = 0, [CanBeNull] Action endDialogueAction = null)
    {
        gameObject.SetActive(true);
        background.color = originalBgColor;
        dialogueTextBox.color = originalTextColor;
        dialogueTextBox.text = text;
        dialogueTextBox.maxVisibleCharacters = startCharacterIndex;
        int interval = 1000 / charactersPerSecond;

        while (dialogueTextBox.maxVisibleCharacters < text.Length)
        {
            dialogueTextBox.maxVisibleCharacters += 1;
            await Task.Delay(interval);
        }
        FadeOutDisplay(1);
        endDialogueAction?.Invoke();
    }

    public async void FadeOutDisplay(float time)
    {
        float elapsedTime = 0;
        Color bgColor = originalBgColor;
        Color textColor = originalTextColor;

        while (elapsedTime < time)
        {
            if (!Application.isPlaying) return;
            float t = elapsedTime / time;
            bgColor.a = Mathf.Lerp(originalBgColor.a, 0, t);
            textColor.a = Mathf.Lerp(originalTextColor.a, 0, t);
            background.color = bgColor;
            dialogueTextBox.color = textColor;
            await Task.Delay(20);
            elapsedTime += Time.deltaTime;
        }

        bgColor.a = 0;
        textColor.a = 0;
        background.color = bgColor;
        dialogueTextBox.color = textColor;
        gameObject.SetActive(false);
        dialogueTextBox.text = "";
        dialogueTextBox.maxVisibleCharacters = 0;
    }
}
