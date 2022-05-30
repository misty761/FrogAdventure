using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // UI
    public GameObject title_UI;
    public GameObject gameover_UI;
    public GameObject cleared_UI;
    // player
    PlayerControl player;
    // player life
    public int playerLifeMax = 3;
    public int playerLife;
    public Text textPlayerLife;
    // stage
    public Text textStage;
    public int currentStage;
    public int maxStage = 8;
    // enemy speed offset
    public float offsetEnemySpeed;
    // time
    float time;
    public Text textTime;
    // state
    public enum State
    {
        Title,
        Playing,
        Cleared,
        GameOver
    }
    public State state;
    // Ad
    GoogleAd googleAd;
    // score
    int scoreCurrent;
    float timeTop;
    public Text textScoreCurrent;
    public Text textScoreTop;
    // audio source
    AudioSource audioSource;
    // development
    public bool isDeveloping;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("GameManager : GameObject already exist!");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FirstInitilize();
        Initialize();
    }

    void FirstInitilize()
    {
        // enmeny speed offset
        offsetEnemySpeed = 0f;
        // find player
        player = FindObjectOfType<PlayerControl>();
        // state is title
        state = State.Title;
        // stage = 1
        currentStage = 1;
        // player life = 3
        playerLife = playerLifeMax;  
        // time = 0
        time = 0f;
        textTime.text = "Time : " + (int)time;
        // audio source
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        // Title, Playing, Game over UI
        SetUI();
        // 플레이어 라이프 표시
        DisplayPlayerLife();
        // score
        scoreCurrent = 0;
        textScoreCurrent.text = "" + scoreCurrent;
        //PlayerPrefs.SetInt("ScoreTop", 0);              // reset top score
        timeTop = PlayerPrefs.GetFloat("ScoreTop", 1000f);
        textScoreTop.text = "Top : " + (int)timeTop;
        // stage
        textStage.text = "Stage " + currentStage;
        // joystick reset
        Joystick joystick = FindObjectOfType<Joystick>();
        joystick.Initialize();
        // player scale sets to 1f
        player.scale = 1f;
    }    

    // Update is called once per frame
    void Update()
    {
        // return
        if (state != State.Playing) return;

        // time
        time += Time.deltaTime;
        textTime.text = "Time : " + (int)time;

        // 다음 스테이지로 이동(개발용)
        if (Input.GetKeyUp(KeyCode.KeypadPlus) && isDeveloping)
        {
            NextLevel();
        }
    }

    void SetUI()
    {
        if (state == State.Title)
        {
            title_UI.SetActive(true);
            gameover_UI.SetActive(false);
            cleared_UI.SetActive(false);
        }
        else if (state == State.Playing)
        {
            title_UI.SetActive(false);
            gameover_UI.SetActive(false);
            cleared_UI.SetActive(false);
        }
        else if (state == State.GameOver)
        {
            title_UI.SetActive(false);
            gameover_UI.SetActive(true);
            cleared_UI.SetActive(false);
        }
        else if (state == State.Cleared)
        {
            title_UI.SetActive(false);
            gameover_UI.SetActive(false);
            cleared_UI.SetActive(true);
        }
    }

    public void StartGame()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.audioClick, player.transform.position, 1f);
        state = State.Playing;
        SetUI();
    }

    public void StartStage()
    {
        state = State.Playing;
        Initialize();
    }

    /*
    public void ContinueGame()
    {
        if (currentStage == 1)
        {
            StartNewGame startNewGame = FindObjectOfType<StartNewGame>();
            startNewGame.ButtonPressed();
        }
        else
        {
            Retry();
        }
    }
    */

    public void NextLevel()
    {
        // top score
        TopScore();
        // sound
        SoundManager.instance.PlaySound(SoundManager.instance.audioFanfare);
        // stage ++
        if (currentStage < maxStage)
        {
            currentStage++;   
        }
        // load the next scene
        SceneManager.LoadScene("Scene" + currentStage);
        // set stage text
        textStage.text = "Stage " + currentStage;        
    }

    public void GameOver()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.audioGameOver, player.transform.position, 1f);

        state = State.GameOver;
        
        SetUI();
    }

    public void Retry()
    {
        FirstInitilize();
        StartStage();
        SceneManager.LoadScene("Scene" + currentStage);
    }

    void DisplayPlayerLife()
    {
        textPlayerLife.text = "x " + playerLife;
    }

    public void ReducePlayerLife()
    {
        Vector2 pos = player.transform.position;
        // sound
        PlaySound(SoundManager.instance.audioGameOver, 1f);
        // show AD
        googleAd = FindObjectOfType<GoogleAd>();
        print("google ad is loaded : " + googleAd.isLoaded);
        // Ad is loaded
        if (googleAd.isLoaded)
        {
            googleAd.ShowAd();
        }
        // Ad is not loaded
        else
        {
            ContinueGame();
        } 
    }

    public void ContinueGame()
    {
        // load new scene
        SceneManager.LoadScene("Scene" + currentStage);
        // player life --
        playerLife--;
        DisplayPlayerLife();
        // start stage
        StartStage();
    }

    public void AddLife()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.audioLifeUp, player.transform.position, 1f);
        playerLife++;
        DisplayPlayerLife();
    }

    public void AddScore(int point)
    {
        SoundManager.instance.PlaySound(SoundManager.instance.audioScore, player.transform.position, 1f);
        scoreCurrent += point;
        textScoreCurrent.text = "" + scoreCurrent;
    }

    void TopScore()
    {
        if (time < timeTop)
        {
            PlaySound(SoundManager.instance.audioFanfare, 1f);
            timeTop = time;
            PlayerPrefs.SetFloat("ScoreTop", timeTop);
            textScoreTop.text = "Top : " + (int) timeTop;  
        }
    }

    public void PlaySound(AudioClip audioClip, float v = 1f)
    {
        if (SoundManager.instance.isSoundOn)
        {
            audioSource.PlayOneShot(audioClip, v);
        }
    }
}
