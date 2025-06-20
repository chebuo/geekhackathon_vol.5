using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
public class NameChange : MonoBehaviour
{
    public string leaderboardID = "31440";
    int score;
    public string name="usako";
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeName()
    {
        StartCoroutine(SendName(score));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SendName(int score)
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("login");
                done = true;
            }
            else
            {
                Debug.Log("no");
                done = true;
            }
        });
        yield return new WaitUntil(() => done);
        string playerID = PlayerPrefs.GetString("PlayerID");
        done = false;
        LootLockerSDKManager.SetPlayerName(name,(response) =>
        {
            if(response.success)
            {
                Debug.Log("ƒvƒŒƒCƒ„[–¼" + name);
            }
            else
            {
                Debug.Log("no");
            }
            done = true;
        });
        yield return new WaitUntil(() => done);
        string PlayerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("scoresosin");
            }
            else
            {
                Debug.Log("no");
            }
            done = true;
        });
        yield return new WaitUntil(() => done);
    }
}
