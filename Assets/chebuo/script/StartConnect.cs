using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class StartConnect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string exePath = Application.streamingAssetsPath + "/BalanceWiiBoard/wiiboardSender.exe";
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(exePath)
            {
                UseShellExecute = false,
                //CreateNoWindow=true
            };
            Process.Start(startInfo);
            UnityEngine.Debug.Log("setuzoku");        }
        catch(System.Exception ex)
        {
            UnityEngine.Debug.Log(ex.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
