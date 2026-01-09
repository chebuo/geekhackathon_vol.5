using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    public Transform target;
    public string replayKey = "replay_001";
    public float playInterval = 0.033f; // 約30fps

    private ReplayData replayData;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadReplay(replayKey);
            Debug.Log("Lキーでリプレイを開始しました");
        }

        if (!isPlaying || replayData == null || replayData.frames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= playInterval)
        {
            timer = 0f;
            if (currentFrame < replayData.frames.Count)
            {
                var frame = replayData.frames[currentFrame];
                target.position = frame.position;
                target.rotation = frame.rotation;
                target.localScale = frame.scale;
                currentFrame++;
            }
            else
            {
                isPlaying = false;
                Debug.Log("リプレイ終了");
            }
        }
    }

    public void LoadReplay(string key)
    {
        string json = RootLocker.Load(key);
        if (!string.IsNullOrEmpty(json))
        {
            replayData = JsonUtility.FromJson<ReplayData>(json);
            currentFrame = 0;
            timer = 0f;
            isPlaying = true;
        }
        else
        {
            Debug.LogWarning("リプレイデータが見つかりません：" + key);
        }
    }
}
