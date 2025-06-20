using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    float count=3;
    GameObject counttext;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        counttext = GameObject.Find("countdown");
        text=counttext.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        if (count > 0)
        {
            text.text = "READY";
        }
        else
        {
            text.text = "GO";
            PlayerControllerTest.isGame = true;
        }
        if(count < -2)
        {
            text.enabled = false;
        }
    }
}
