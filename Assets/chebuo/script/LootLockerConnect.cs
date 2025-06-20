using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerConnect : MonoBehaviour
{
    public string leaderboardID = "31439";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPlayerRanking());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GetPlayerRanking()
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
        LootLockerSDKManager.GetMemberRank(leaderboardID, PlayerID, (response) =>
        {
            if (response.success)
            {
                Debug.Log($"‡ˆÊ:{response.rank}/{response.score}");
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
