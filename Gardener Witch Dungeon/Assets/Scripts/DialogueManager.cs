using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // format: [(name)] indicate a character, line after is the dialogue
    [SerializeField] TextAsset textFile;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] float timeBetweenCharacters;
    [SerializeField] string currentLine;
    [SerializeField] int index;
    [SerializeField] InputActionReference nextInput;
    [SerializeField] bool dialoguePlaying;
    [SerializeField] bool endReached;
    string text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = textFile.text;
        Advance();
        nextInput.action.performed += Advance;
        Debug.Log(textFile.text.Contains('\n'));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator DisplayText()
    {
        dialoguePlaying = true;
        while (index < currentLine.Length)
        {
            if (!dialoguePlaying) break;
            dialogueText.text += currentLine[index];
            index++;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
        dialoguePlaying = false;
    }
    public void Advance(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (dialoguePlaying)
        {
            dialogueText.text = currentLine;
            dialoguePlaying = false;
        }
        else if (!endReached)
        {
            int endIndex = text.IndexOf('\n');
            Debug.Log(endIndex);
            if (endIndex == -1)
            {
                endIndex = text.Length - 1;
                endReached = true;
                currentLine = text[..endIndex];
            }
            else
            {
                currentLine = text[..endIndex];
                text = text.Substring(endIndex + 1);
            }
            string charName = "";
            if (currentLine[0] == '[')
            {
                charName = currentLine.Substring(1, currentLine.IndexOf(']') - 1);
                currentLine = currentLine.Substring(currentLine.IndexOf(']') + 1);
            }
            nameText.text = charName;
            dialogueText.text = "";
            index = 0;
            StartCoroutine(DisplayText());
        }
    }
}
