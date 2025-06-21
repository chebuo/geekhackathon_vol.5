using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;

public class Ranking : MonoBehaviour
{
    public string leaderboardID = "31439";
    [SerializeField] GameObject rankItemPrefab;
    [SerializeField] Transform rankParent;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void ShowRanking()
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
                Debug.Log("dekita");
                foreach(Transform child in rankParent)
                {
                    Destroy(child.gameObject);
                }
                foreach (var item in response.items)
                {
                    GameObject rankObj = Instantiate(rankItemPrefab, rankParent);
                    Text text = rankObj.GetComponent<Text>();
                    if (text)
                    {
                        string name = string.IsNullOrEmpty(item.player.name) ? "usako" : item.player.name;
                        text.text = $"{item.rank}ˆÊ {name} {item.score}";
                    }
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
