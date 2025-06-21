using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class WiimoteRotationReceiver : MonoBehaviour
{
    private UdpClient udp;
    private Thread receiveThread;
    private bool running = true;

    public GameObject device1;
    public GameObject device2;

    private Vector3[] currentEuler = new Vector3[2];

    void Start()
    {
        udp = new UdpClient(9000); // ポート番号は送信側と一致させる
        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        if (device1 != null)
            device1.transform.rotation = Quaternion.Euler(currentEuler[0]);

        if (device2 != null)
            device2.transform.rotation = Quaternion.Euler(currentEuler[1]);
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