using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class GameplayScript : MonoBehaviour
{
    [SerializeField]
    Text recordText;
    public int record;
    public GameObject prefabText;

    [SerializeField]
    public Text[] slotText;

    [SerializeField]
    public GameObject[] numbers;

    char[] random_num;
    int plus = 0;
    int minus = 0;
    int guessTime = 0;
    public Text lastGuess;
    public Text timeText;
    public float countTime;
    public bool timeEnd;

    public Text winText;
    public Text winText2;
    public Text winText3;
    public Button popupExitBtn;
    public GameObject gameScene;
    public GameObject gameOverPopup;
    public GameObject gameWinPopup;
    [SerializeField]
    public Text gameModeText;
    public float easyTime;
    public float mediumTime;
    public float hardTime;
    public float gameTime;
    void SetGameMode()
    {

        if (GameLevel.gameLevel == "Easy")
        {
            gameTime = easyTime;
            gameModeText.text = "Easy";
            gameModeText.GetComponent<Text>().color = Color.green;
        }
        else if (GameLevel.gameLevel == "Medium")
        {
            gameTime = mediumTime;
            gameModeText.text = "Medium";
            gameModeText.GetComponent<Text>().color = Color.yellow;
        }
        else if (GameLevel.gameLevel == "Hard")
        {
            gameTime = hardTime;
            gameModeText.text = "Hard";
            gameModeText.GetComponent<Text>().color = Color.red;

        }

    }

    void Start()
    {

        record = PlayerPrefs.GetInt("score",999);
        
        recordText.text = record.ToString();
        SetGameMode();
        countTime = gameTime;
        timeEnd = false;

        random_num = GenerateNum().ToCharArray();
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(random_num[i]);
        }
    }

    void Update()
    {
        if (!timeEnd)
        {
            TimeCount();
            return;
        }
    }

    public void OnClick()
    {
        string buttonValue = EventSystem.current.currentSelectedGameObject.name;
        for (int i = 0; i < slotText.Length; i++)
        {
            if(slotText[i].text == "")
            {
                slotText[i].text = numbers[int.Parse(buttonValue)].GetComponentInChildren<Text>().text;
                numbers[int.Parse(buttonValue)].SetActive(false);
                break;
            }
            
        }
        
    }

    public void Refresh()
    {
        string buttonValue_ = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        string slotValue = EventSystem.current.currentSelectedGameObject.name;

        if(slotText[int.Parse(slotValue)].text != "")
        {
            slotText[int.Parse(slotValue)].text = "";
            numbers[int.Parse(buttonValue_)].SetActive(true);
            return;
        }

    }

    public void CheckValues()
    {

        if (int.Parse(slotText[0].text.ToString()) == int.Parse(random_num[0].ToString()))
            plus = plus + 1;

        else if (int.Parse(slotText[0].text.ToString()) == int.Parse(random_num[1].ToString()) || int.Parse(slotText[0].text.ToString()) == int.Parse(random_num[2].ToString()) || int.Parse(slotText[0].text.ToString()) == int.Parse(random_num[3].ToString()))
            minus = minus + 1;

        if (int.Parse(slotText[1].text.ToString()) == int.Parse(random_num[1].ToString()))
            plus = plus + 1;

        else if (int.Parse(slotText[1].text.ToString()) == int.Parse(random_num[0].ToString()) || int.Parse(slotText[1].text.ToString()) == int.Parse(random_num[2].ToString()) || int.Parse(slotText[1].text.ToString()) == int.Parse(random_num[3].ToString()))
            minus = minus + 1;

        if (int.Parse(slotText[2].text.ToString()) == int.Parse(random_num[2].ToString()))
            plus = plus + 1;

        else if (int.Parse(slotText[2].text.ToString()) == int.Parse(random_num[0].ToString()) || int.Parse(slotText[2].text.ToString()) == int.Parse(random_num[1].ToString()) || int.Parse(slotText[2].text.ToString()) == int.Parse(random_num[3].ToString()))
            minus = minus + 1;

        if (int.Parse(slotText[3].text.ToString()) == int.Parse(random_num[3].ToString()))
            plus = plus + 1;

        else if (int.Parse(slotText[3].text.ToString()) == int.Parse(random_num[0].ToString()) || int.Parse(slotText[3].text.ToString()) == int.Parse(random_num[1].ToString()) || int.Parse(slotText[3].text.ToString()) == int.Parse(random_num[2].ToString()))
            minus = minus + 1;


    }

    public void TimeCount()
    {
        countTime -= Time.deltaTime;
        timeText.text = Math.Round(countTime).ToString();
        if (countTime < 0)
        {
            timeEnd = true;
            CheckForGameOver();
            return;
        }

    }

    public void Check()
    {
        if (slotText[0].text.ToString() == "" || slotText[1].text.ToString() == "" || slotText[2].text.ToString() == "" || slotText[3].text.ToString() == "")
        {
            SSTools.ShowMessage("Please fill all boxes", SSTools.Position.bottom, SSTools.Time.threeSecond);
            return;
        }

        if (int.Parse(slotText[0].text.ToString()) == 0)
        {
            SSTools.ShowMessage("You can not put 0 at first box", SSTools.Position.bottom, SSTools.Time.threeSecond);
            return;
        }

        else
            CheckValues();
        guessTime++;

        prefabText = Instantiate(prefabText, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        prefabText.GetComponent<Text>().text = guessTime + ". " + slotText[0].text + slotText[1].text + slotText[2].text + slotText[3].text + " |" + "+" + plus + "-" + minus + "| ";
        prefabText.transform.SetParent(GameObject.Find("Content").transform, false);
        lastGuess.text = "Last Guess:  " + guessTime + ". " + slotText[0].text + slotText[1].text + slotText[2].text + slotText[3].text + " |" + "+" + plus + "-" + minus + "| ";

        if (plus == 4)
        {
           
            CheckForGameWin();
            
                
        }
        else
            minus = 0;
            plus = 0;


    }

    public void CheckForGameOver()
    {
        if (countTime < 0)
        {
            
            gameOverPopup.SetActive(true);
            gameScene.SetActive(false);
            gameWinPopup.SetActive(false);

        }
    }

    public void CheckForGameWin()
    {
        String guessedNum = new String(random_num);

        if (plus == 4)
        {
            PlayerPrefs.SetInt("score", guessTime);
            if (record < guessTime)
            {
                PlayerPrefs.SetInt("score", record);
            }
            timeEnd = true;
            winText3.text = "Best Score  : " + PlayerPrefs.GetInt("score").ToString();
            winText.text = "Number Was : " + guessedNum;
            winText2.text = "Guess Times : " + guessTime.ToString();
           
            gameWinPopup.SetActive(true);
            gameOverPopup.SetActive(false);
            gameScene.SetActive(false);
        }
    }

    public string GenerateNum()
    {
        int digit1 = UnityEngine.Random.Range(1, 10);

        int digit2 = UnityEngine.Random.Range(0, 10);
        while (digit2 == digit1)
            digit2 = UnityEngine.Random.Range(0, 10);

        int digit3 = UnityEngine.Random.Range(0, 10);
        while (digit3 == digit2 || digit3 == digit1)
            digit3 = UnityEngine.Random.Range(0, 10);

        int digit4 = UnityEngine.Random.Range(0, 10);
        while (digit4 == digit3 || digit4 == digit2 || digit4 == digit1)
            digit4 = UnityEngine.Random.Range(0, 10);

        return digit1.ToString() + digit2.ToString() + digit3.ToString() + digit4.ToString();
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
