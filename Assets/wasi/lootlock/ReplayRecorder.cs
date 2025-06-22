using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }
}

[System.Serializable]
public class ReplayData
{
    public List<TransformData> frames = new List<TransformData>();
}

public class ReplayRecorder : MonoBehaviour
{
    public Transform target;
    public float recordInterval = 0.033f; // 約30fps

    private float timer = 0f;
    private ReplayData replayData = new ReplayData();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            timer = 0f;
            replayData.frames.Add(new TransformData(target.position, target.rotation, target.localScale));
        }

        // Pキーが押されたら保存
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveReplay("replay_001");
            Debug.Log("Pキーが押されたのでリプレイを保存しました");
        }
    }

    public void SaveReplay(string key)
    {
        string json = JsonUtility.ToJson(replayData);
        RootLocker.Save(key, json);
    }
}
