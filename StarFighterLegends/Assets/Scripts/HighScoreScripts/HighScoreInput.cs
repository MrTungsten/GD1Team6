using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreInput : MonoBehaviour
{
    private HighScoreTableScript highScoreTableScript;
    private GameObject table;
    private Transform playerInputBox;
    private Transform playerNameInput;
    private char playerFirstInitial;
    private char playerSecondInitial;
    private char playerThirdInitial;
    private char[] listOfAlphabet = new char[]
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' 
    };
    private char currentlySelectedChar = 'A';
    private string playerName = "";
    private bool submitted = false;

    private void Awake()
    {

        playerNameInput = transform.Find("NameInputScreen");

        playerInputBox = playerNameInput.Find("PlayerNameInput");
        playerInputBox.GetComponent<TextMeshProUGUI>().text = playerName;

        highScoreTableScript = GameObject.FindAnyObjectByType<HighScoreTableScript>();
        table = GameObject.Find("Table");

        if (highScoreTableScript.ReturnValidHighScore())
        {
            table.SetActive(false);

            foreach (Button button in listOfAlphas)
            {
                button.onClick.AddListener(() => { AddAlpha(button); });
            }

            listOfAlphas[0].Select();
        }
        else
        {
            table.SetActive(true);
            highScoreTableScript.SetIsViewing();
            gameObject.SetActive(false);
        }
        
    }

    private void Update()
    {
        ActivateHighScoreTable();
    }

    private void ActivateHighScoreTable()
    {
        if (submitted)
        {
            table.SetActive(true);
            highScoreTableScript.AddHighScoreEntry(ScoreManager.Instance.GetTotalScore(), playerName);
            highScoreTableScript.SetIsViewing();
            gameObject.SetActive(false);
        }
    }

    private void AddAlpha(Button button)
    {
        if (!(playerName.Length >= 3))
        {
            playerInputBox.GetComponent<TextMeshProUGUI>().text = playerName;
        }
        else
        {
            submitted = true;
        }
    }
}
