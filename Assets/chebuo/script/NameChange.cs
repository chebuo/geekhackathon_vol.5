using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
public class NameChange : MonoBehaviour
{
    public string leaderboardID = "31440";
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
        StartCoroutine(SendName());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SendName()
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
                Debug.Log("プレイヤー名" + name);
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
