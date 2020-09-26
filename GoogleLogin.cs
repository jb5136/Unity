using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// Text UI 사용
using UnityEngine.UI;
// 구글 플레이 연동
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GoogleLogin : MonoBehaviour
{
    bool bWait = false;
    public Text text;

    bool start = false;
    GameObject m_Login;
    GameObject m_start;
    float m_time = 0.0f;

    void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        m_Login = GameObject.Find("Canvas").transform.Find("Login").gameObject;
        m_start = GameObject.Find("Canvas").transform.Find("Start").gameObject;
        m_Login.SetActive(true);
        m_start.SetActive(false);
    }
    void Start()
    {

    }
    void Update()
    {
        if (start)
        {
            FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + "CharState.txt");
            if (fi.Exists)
            {
                LoadingSceneMng.LoadScene("Village");
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    LoadingSceneMng.LoadScene("CharacterSelect");
                }
            }
        }
    }

    public void OnLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    m_Login.SetActive(false);
                    m_start.SetActive(true);
                    start = true;
                }
                else
                {
                    Debug.Log("Fall");
                    text.text = "Fail";
                }
            });
        }
    }

    public void OnLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        text.text = "Logout";
    }

    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }
}