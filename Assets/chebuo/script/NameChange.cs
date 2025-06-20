using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
public class NameChange : MonoBehaviour
{
    public string leaderboardID = "31440";
    int score;
    public string name="usako";
    InputField inputfield;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        inputfield=GetComponent<InputField>();
        text= GetComponent<Text>();
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
    }
}
