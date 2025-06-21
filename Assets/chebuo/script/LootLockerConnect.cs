using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LootLockerConnect : MonoBehaviour
{
    public string leaderboardID = "31439";
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      
        if (PlayerControllerTest.isGoal && PlayerControllerTest.isGame&&i==0)
        {
            StartCoroutine(SendScore((int)PlayerControllerTest.time));
            i++;
        }

    }
    IEnumerator SendScore(int score)
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("login");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("no");
                done = true;
            }
        });
        yield return new WaitUntil(()=>done);
        string PlayerID = PlayerPrefs.GetString("PlayerID");
        done = false;
        LootLockerSDKManager.SubmitScore("PlayerID", score, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("スコアアップロード成功！");
            }
            else
            {
                Debug.Log("スコアアップロード失敗: " + response.errorData?.message);
            }
            done = true;
        });
        yield return new WaitUntil(() => done);
        PlayerControllerTest.isGame = false;
        SceneManager.LoadScene("TitleScene");
    }
}
