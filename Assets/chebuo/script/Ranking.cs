using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class Ranking : MonoBehaviour
{
    public string leaderboardID = "31439";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRanking());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GetRanking()
    {
        int count = 10;
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success) return;
            PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
            done = true;
        });
        yield return new WaitUntil(() => done);
        LootLockerSDKManager.GetScoreList(leaderboardID, count, 0, (response) =>
        {
            done = false;
            if (response.success)
            {
                foreach (var item in response.items)
                {
                    Debug.Log($"rank:{item.rank}name:{item.player.name}score:{item.score}");
                }
            }
            else
            {
                Debug.Log("zanen");
            }
            done = true;
        });
        yield return new WaitUntil(() => done);
    }
}
