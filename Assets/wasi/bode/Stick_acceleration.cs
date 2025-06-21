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

    private float y, r, p;
    private float y2, r2, p2;

    private float prevP = 0f;
    private float prevP2 = 0f;

    [Header("デバイスとして操作対象のオブジェクト")]
    public GameObject device1;
    public GameObject device2;
    public GameObject boad_r;
    public GameObject boad_l;

    [Header("ピッチ角の差分しきい値（加速のトリガー）")]
    public float pitchThreshold = 30f; // 角度差のしきい値（例: 30度）

    [Header("加える力の大きさ（インパルス）")]
    public float forceMagnitude = 10f;

    private Vector3[] currentEuler = new Vector3[2];

    void Start()
    {
        udp = new UdpClient(9000);
        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        // Device1 処理
        if (device1 != null)
        {
            Vector3 euler = currentEuler[0];
            p = euler.x;
            y = euler.y;
            r = euler.z;

            device1.transform.rotation = Quaternion.Euler(euler);

            float deltaP = Mathf.Abs(p - prevP);
            if (deltaP >= pitchThreshold)
            {
                Rigidbody rb = boad_r.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(boad_r.transform.forward * forceMagnitude, ForceMode.Impulse);
                    Debug.Log("Device1 に加速");
                }
            }

            prevP = p;
        }

        // Device2 処理
        if (device2 != null)
        {
            Vector3 euler = currentEuler[1];
            p2 = euler.x;
            y2 = euler.y;
            r2 = euler.z;

            device2.transform.rotation = Quaternion.Euler(euler);

            float deltaP2 = Mathf.Abs(p2 - prevP2);
            if (deltaP2 >= pitchThreshold)
            {
                Rigidbody rb = boad_l.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(boad_l.transform.forward * forceMagnitude, ForceMode.Impulse);
                    Debug.Log("Device2 に加速");
                }
            }

            prevP2 = p2;
        }

        Debug.Log($"Device1: Pitch={p}, Yaw={y}, Roll={r}");
        Debug.Log($"Device2: Pitch={p2}, Yaw={y2}, Roll={r2}");
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
                Debug.Log("UDP受信中のエラー: " + ex.Message);
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
