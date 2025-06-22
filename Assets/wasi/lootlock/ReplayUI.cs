using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayUI : MonoBehaviour
{
    public ReplayRecorder recorder;

    public void OnSaveReplay()
    {
        recorder.SaveReplay("replay_001");
        Debug.Log("ƒŠƒvƒŒƒC•Û‘¶Š®—¹I");
    }
}
