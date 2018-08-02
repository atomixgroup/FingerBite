using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject fingerPanel;
    public GameObject youLosePanel;
    public Vector3 fingerPos;
    public Vector3 ScreenToWorldFingerPos = new Vector3(0, 0, -1);
    public bool isTouched = false;
    public Animator plant1;
    public Animator plant2;
    float[] timeArray = new float[] {1f, 2f, 3f, 4f, 5f};
    private float waitTime;
    private Animator attackingPlant;
    private bool isAttacking;
    public bool isCollided;
    public Text scoreboard;
    public static int max;
    public AudioSource zombieSound;
    public GameObject wtf1;
    public GameObject wtf2;
    private GameObject wtf;
    public AudioSource attackAudio;
    private float adCountTime = 60f;
    public bool available = false;
    public GameObject FingerMask;
    private Vector2 center;
    public SpeedSlider SpeedSlider;
    public IndicatorCreater IndicatorCreater;
    private AdsController _adsController;
    private bool firstTimeShowAd = true;
    public GameObject ExitCanvas;


    void Start()
    {
        _adsController = GetComponent<AdsController>();
        center = new Vector2(Screen.width / 2, Screen.height / 2.5f);
        youLosePanel.SetActive(false);
        wtf1.SetActive(false);
        wtf2.SetActive(false);
        Time.timeScale = 1f;
        MenuManager.score = 0;
        max = ObscuredPrefs.GetInt("maxScore");
        isCollided = false;
        isAttacking = false;
        plant1.SetFloat("Speed", MenuManager.attackingSpeed);
        plant2.SetFloat("Speed", MenuManager.attackingSpeed);
        Vector3 FPos = Camera.main.ScreenToWorldPoint(center);
        FPos.z = 0f;
        FingerMask.transform.position = FPos;
        FingerMask.transform.localScale = new Vector3(Screen.height / 2 / 10, Screen.height / 2 / 10, 1);
    }

    void Update() 
    {
        if (Input.touchCount <= 0)
        {
            if (isAttacking)
            {
                fingerPanel.SetActive(false);
                isTouched = false;
            }
            else
            {
                fingerPanel.SetActive(true);
                isTouched = false;
            }
        }
        else
        {
            float actualDistance = Vector2.Distance(center, Input.GetTouch(0).position);
            float maxDistance = Screen.height / 4;
            if (actualDistance < maxDistance)
            {
                fingerPanel.SetActive(false);
                fingerPos = Input.GetTouch(0).position;
                isTouched = true;
                ScreenToWorldFingerPos = Camera.main.ScreenToWorldPoint(fingerPos);
                if (ScreenToWorldFingerPos.x <= 0)
                {
                    attackingPlant = plant1;
                }
                else
                {
                    attackingPlant = plant2;
                }
                ExitCanvas.SetActive(false);

            }
            else
            {
                fingerPanel.SetActive(true);
                ExitCanvas.SetActive(true);
                isTouched = false;
            }
        }

        if (isCollided)
        {
            isTouched = false;
            fingerPanel.SetActive(false);
            youLosePanel.SetActive(true);
            if (MenuManager.score > max)
            {
                ObscuredPrefs.SetInt("maxScore", MenuManager.score);
            }
            
        }

        scoreboard.text = MenuManager.score.ToString();
        waitTime = timeArray[Random.Range(0, timeArray.Length)];

        AdShow();
    }

    private void AdShow()
    {
        
        if (adCountTime <= 0)
        {
            adCountTime = 0;
            available = true;
           
        }
        else
        {
            adCountTime -= Time.deltaTime;
        }
    }

    public void ShowVideo()
    {
        if (firstTimeShowAd)
        {  
            _adsController.onFinishCallback += OnVideoAdResponse;
            _adsController.showVideoAd();
        }
     
    }

    private void OnVideoAdResponse()
    {

//        print("video response true");
        firstTimeShowAd = false;
        isCollided = false; 
        fingerPanel.SetActive(true);
        youLosePanel.SetActive(false);
        GameObject.FindGameObjectWithTag("Continue").SetActive(false);
    }
    private void FixedUpdate()
    {
        if (isTouched && !isAttacking && !isCollided)
        {
            isAttacking = true;
            StartCoroutine(TriggerAttack());
        }
    }

    IEnumerator TriggerAttack()
    {
        plant1.SetFloat("Speed", MenuManager.attackingSpeed);
        plant2.SetFloat("Speed", MenuManager.attackingSpeed);
        while (true)
        {
            for (float i = 0; i < waitTime; i += .2f)
            {
                yield return new WaitForSeconds(.2f);
                if (!isTouched)
                {
                    isAttacking = false;
                    break;
                }
            }

            if (!isAttacking) break;
            attackingPlant.SetTrigger("Fire");
            Animator daFaqPlant = attackingPlant;
            zombieSound.mute = true;
            attackAudio.Play();
            isAttacking = false;
            yield return new WaitForSeconds(0.8f);
            zombieSound.mute = false;
            zombieSound.Play();
            if (!isCollided)
            {
                MenuManager.score++;
                LevelSetter();

                if (daFaqPlant == plant1)
                {
                    wtf = wtf1;
                    wtf.SetActive(true);
                }
                else
                {
                    wtf = wtf2;
                    wtf.SetActive(true);
                }

                yield return new WaitForSeconds(0.8f);
                wtf.SetActive(false);
            }

            break;
        }
    }

    public void PlayAgain()
    {
        firstTimeShowAd = true;
        if (available)
        {
        
            if (AdsController.bannerAvailable)
            {
                _adsController.showBannerAd();
                available = false;
                adCountTime = 60;
                return;
            }
        }
        

        MenuManager.score = 0;
       

        
        LevelSetter(); 

        isCollided = false;
        youLosePanel.SetActive(false);
        fingerPanel.SetActive(true);
//        IndicatorCreater.SetNumberOfCirles(1,0);
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void LevelSetter()
    {
        
        IndicatorCreater.SetNumberOfCirles(MenuManager.score);
        SpeedSlider.setSliderValue();
    }

    

}