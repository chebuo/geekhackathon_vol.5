using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LootLockerConnect : MonoBehaviour
{
    public string leaderboardID = "31439";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPlayerRanking(20));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GetPlayerRanking(int score)
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
        });
        yield return new WaitUntil(() => done);
        done = false;
        LootLockerSDKManager.GetMemberRank(leaderboardID, PlayerID, (response) =>
        {
            if (response.success)
            {
                Debug.Log($"順位:{response.rank}/{response.score}");
            }
            else
            {
                Debug.Log("sippai");
            }
            done = true;
        });
        yield return new WaitUntil(() => done);
    }
}
