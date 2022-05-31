using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class LeaderBoard : MonoBehaviour
{
    string LEADER_BOARD_ID = "Cxxxxxxxxxxxxxx";

    // Start is called before the first frame update

    private void Awake()
    {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        //.EnableSavedGames()
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        //.RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        //.RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        //.RequestIdToken()
        .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_TouchDown = new EventTrigger.Entry();
        entry_TouchDown.eventID = EventTriggerType.PointerDown;
        entry_TouchDown.callback.AddListener((data) => { TouchDown(); });
        eventTrigger.triggers.Add(entry_TouchDown);

        EventTrigger.Entry entry_TouchUp = new EventTrigger.Entry();
        entry_TouchUp.eventID = EventTriggerType.PointerUp;
        entry_TouchUp.callback.AddListener((data) => { TouchUp(); });
        eventTrigger.triggers.Add(entry_TouchUp);

        //PlayerPrefs.SetInt("ScoreTop", 0);
        //GameManager.instance.TopScore();

        LogInPlayGames();

    }

    public void LogInPlayGames()
    {
        //이미 인증된 사용자는 바로 로그인 성공됩니다.
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                }
                else
                {
                    Debug.Log("Login Fail");

                    PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) => {
                        // handle results
                        Debug.Log("handle results : " + result);
                    });
                }
            });
    }

    public void TouchDown()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.audioClick, Vector3.zero, 1f);
    }

    public void TouchUp()
    {
        RankButtonClick();
    }

    public void RankButtonClick()
    {
        Social.localUser.Authenticate(AuthenticateHandler);
    }

    void AuthenticateHandler(bool isSuccess)
    {
        if (isSuccess)
        {
            float highScore = PlayerPrefs.GetFloat("ScoreTop", 1000f);
            Social.ReportScore((long)highScore, LEADER_BOARD_ID, (bool success) =>
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI(LEADER_BOARD_ID);
                Debug.Log("Show Leader Board UI : " + success);
            });
            
        }
        else
        {
            // login failed
            Debug.Log("Login failed to Google Play Games");
        }
    }
}
