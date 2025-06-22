using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Stick_acceleration : MonoBehaviour
{
    private UdpClient udp;
    private Thread receiveThread;
    private bool running = true;

    private float p, p2;
    private float prevP = 0f;
    private float prevP2 = 0f;

    private float pitchSpeed1 = 0f;
    private float pitchSpeed2 = 0f;

    private Vector3[] currentEuler = new Vector3[2];

    [Header("加速対象のオブジェクト")]
    public GameObject target;
    private Rigidbody rb;

    [Header("ピッチ速度のしきい値（これを超えたら反応）")]
    public float velocityThreshold = 130f;

    [Header("加速の強さに乗算する係数")]
    public float forceMultiplier = 0.0001f;

    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
        udp = new UdpClient(9000);
        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        // 各デバイスの角速度を計算
        Vector3 euler1 = currentEuler[0];
        p = euler1.x;
        pitchSpeed1 = Mathf.Abs(p - prevP) / Time.deltaTime;
        prevP = p;

        Vector3 euler2 = currentEuler[1];
        p2 = euler2.x;
        pitchSpeed2 = Mathf.Abs(p2 - prevP2) / Time.deltaTime;
        prevP2 = p2;

        bool leftShake = pitchSpeed1 >= velocityThreshold;
        bool rightShake = pitchSpeed2 >= velocityThreshold;

        // 加速判定（左右 or 前）
        if (leftShake && rightShake)
        {
            float avgSpeed = (pitchSpeed1 + pitchSpeed2) / 2f;
            rb.AddForce(target.transform.forward * avgSpeed * forceMultiplier, ForceMode.Impulse);
            Debug.Log($"両振り → 前進（強さ: {avgSpeed:F1}）");
        }
        else if (leftShake)
        {
            rb.AddForce(-target.transform.right * pitchSpeed1 * forceMultiplier, ForceMode.Impulse);
            Debug.Log($"左振り → 左に加速（強さ: {pitchSpeed1:F1}）");
        }
        else if (rightShake)
        {
            rb.AddForce(target.transform.right * pitchSpeed2 * forceMultiplier, ForceMode.Impulse);
            Debug.Log($"右振り → 右に加速（強さ: {pitchSpeed2:F1}）");
        }

        Debug.Log($"角速度 | 左: {pitchSpeed1:F1}, 右: {pitchSpeed2:F1}");
    }

    private void ReceiveLoop()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        while (running)
        {
            try
            {
                byte[] data = udp.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                string[] parts = message.Split(',');

                if (parts.Length == 4 &&
                    parts[0].StartsWith("Device") &&
                    int.TryParse(parts[0].Substring(6), out int id) &&
                    float.TryParse(parts[1], out float roll) &&
                    float.TryParse(parts[2], out float pitch) &&
                    float.TryParse(parts[3], out float yaw))
                {
                    int index = id - 1;
                    if (index >= 0 && index < 2)
                        currentEuler[index] = new Vector3(pitch, yaw, roll);
                }
            }
            catch (SocketException ex)
            {
                Debug.Log("UDP受信エラー: " + ex.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        udp?.Close();
        receiveThread?.Join();
    }
}
