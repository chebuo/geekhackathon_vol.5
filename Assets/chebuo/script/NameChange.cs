using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
public class NameChange : MonoBehaviour
{
    public string leaderboardID = "31440";
    public string name="usako";
    public GameObject nameObj;
    InputField inputfield;
    // Start is called before the first frame update
    void Start()
    {
        inputfield=nameObj.GetComponent<InputField>();
    }
    public void ChangeName()
    {
        name = inputfield.text;
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
