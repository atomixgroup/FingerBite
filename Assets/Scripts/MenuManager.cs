using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class MenuManager : MonoBehaviour
{
    public static bool isMuted = false;
    public static int score;
    public static float attackingSpeed;
    public static float attackingSpeedMax;
    public static float attackingSpeedMin;
    public Sprite muted;
    public Sprite soundOn;
    public Image muteButton;
    public GameObject playText;
    public GameObject ScoreIcon;
    public GameObject scoreboard;
    public GameObject playModeCanvas;

    public GameObject loadPanelCanvas;

    public GameObject leaderBoardCanvas;
    public GameObject exitCanvas;
    public GameObject InputSection;
    public GameObject exitBtnObject;
    public Text inputNameText;

   

    void Start()
    {
        if (ObscuredPrefs.GetString("unique") == "")
        {
            ObscuredPrefs.SetString("unique",Md5Sum(System.DateTime.Now+"AKufRTXukcAaiT6hvyxdJ10ac8X2iIqZ"));
        }
        playModeCanvas.SetActive(false);
        isMuted = ObscuredPrefs.GetBool("isMuted");
        int max = ObscuredPrefs.GetInt("maxScore");
        if (score > 0)
        {
            scoreboard.GetComponent<Text>().text = score.ToString() + "\nHIGH SCORE: " + max.ToString();
        }
        else
        {
            scoreboard.GetComponent<Text>().text = "HIGH SCORE\n" + max.ToString();
        }

        if (isMuted)
        {
            muteButton.sprite = muted;
            AudioListener.pause = true;
        }
        else
        {
            muteButton.sprite = soundOn;
            AudioListener.pause = false;
        }
        
    }


    public void PlayPressed()
    {
       
        playText.SetActive(false);

        ScoreIcon.SetActive(false);
        
        scoreboard.SetActive(false);
        
        muteButton.enabled = false;
        if (ObscuredPrefs.GetString("name") == "")
        {
            InputSection.SetActive(true);
        }
        else
        {
            playModeCanvas.SetActive(true);
        }

        muteButton.enabled = false;
        exitBtnObject.SetActive(false);

    }

    public void MutePressed()
    {
        if (isMuted)
        {
            AudioListener.pause = false;
            isMuted = false;
            ObscuredPrefs.SetBool("isMuted", false);
            muteButton.sprite = soundOn;
        }
        else
        {
            AudioListener.pause = true;
            isMuted = true;
            ObscuredPrefs.SetBool("isMuted", true);
            muteButton.sprite = muted;
        }
    }

    public void ModeSelected(int sender)
    {
        if (ObscuredPrefs.GetString("name") != "")
        {
            
        
        switch (sender)
        {
            case 0:
                ObscuredPrefs.SetInt("type",1);
                attackingSpeed = 1.0f;
                attackingSpeedMin = attackingSpeed;
                attackingSpeedMax = 2.5f;
                break;
            case 1:
                ObscuredPrefs.SetInt("type",2);

                attackingSpeed = 1.5f;
                attackingSpeedMin = attackingSpeed;

                attackingSpeedMax = 3.5f;

                break;
            case 2:
                ObscuredPrefs.SetInt("type",3);

                attackingSpeed = 2.0f;
                attackingSpeedMin = attackingSpeed;
                attackingSpeedMax = 4.5f;

                break;
        }
        

        FadeOut fadeOut = loadPanelCanvas.GetComponent<FadeOut>();
        fadeOut.onFinishCallback += goGame;
        fadeOut.startFadeOut();

        }

       

    }

    void goGame()
    {
        SceneManager.LoadScene(1);

    }

    public void SelectionBack()
    {
        playModeCanvas.SetActive(false);
        playText.SetActive(true);
        muteButton.enabled = true;
        ScoreIcon.SetActive(true);
        scoreboard.SetActive(true);
        playText.SetActive(true);
        leaderBoardCanvas.SetActive(false);
        exitBtnObject.SetActive(true);
    }
    public void SelectLeaderBoard()
    {
        leaderBoardCanvas.SetActive(true);
        GameObject.FindGameObjectWithTag("LeaderBoard").GetComponent<LeaderBoard>().refreshData();
        playText.SetActive(false);
        ScoreIcon.SetActive(false);

        muteButton.enabled = false;
        scoreboard.SetActive(false);
        playText.SetActive(false);
    }

    public void saveName()
    {
        if (inputNameText.text != "")
        {
            ObscuredPrefs.SetString("name",inputNameText.text);
            InputSection.SetActive(false);
            playModeCanvas.SetActive(true);
        }
    }
    public  string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
 
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
 
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
 
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
 
        return hashString.PadLeft(32, '0');

    }

    public void Exit()
    {
        exitCanvas.SetActive(true);
        playText.SetActive(false);

        ScoreIcon.SetActive(false);
        
        scoreboard.SetActive(false);
    }

    public void yesB()
    {
        Application.Quit();
    }
    public void noB()
    {
        exitCanvas.SetActive(false);
        playText.SetActive(true);

        ScoreIcon.SetActive(true);
        
        scoreboard.SetActive(true);
    }
}