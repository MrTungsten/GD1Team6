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
    private List<Button> listOfAlphas = new List<Button>();
    private string name = "";
    private bool submitted = false;

    private void Awake()
    {

        playerNameInput = transform.Find("NameInputScreen");

        playerInputBox = playerNameInput.Find("PlayerNameInput");
        playerInputBox.GetComponent<TextMeshProUGUI>().text = name;

        highScoreTableScript = GameObject.FindAnyObjectByType<HighScoreTableScript>();
        table = GameObject.Find("Table");

        if (highScoreTableScript.ReturnValidHighScore())
        {
            table.SetActive(false);

            foreach (RectTransform children in GameObject.Find("HighScoreButtons").transform)
            {
                listOfAlphas.Add(children.GetComponent<Button>());
            }

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
            highScoreTableScript.AddHighScoreEntry(ScoreManager.Instance.GetHighScore(), name);
            highScoreTableScript.SetIsViewing();
            gameObject.SetActive(false);
        }
    }

    private void AddAlpha(Button button)
    {
        if (!(name.Length >= 3))
        {
            name += button.GetComponentInChildren<TextMeshProUGUI>().text;
            playerInputBox.GetComponent<TextMeshProUGUI>().text = name;

            if (name.Length >= 3)
            {
                submitted = true;
            }
        }
        else
        {
            submitted = true;
        }
    }
}
