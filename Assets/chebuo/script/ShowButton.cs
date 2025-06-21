using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowButton : MonoBehaviour
{
    public GameObject panel;
    public GameObject ranking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowToggle()
    {
        panel.SetActive(!panel.activeSelf);
    }
    public void RankingToggle()
    {
        ranking.SetActive(!ranking.activeSelf);
    }
}
