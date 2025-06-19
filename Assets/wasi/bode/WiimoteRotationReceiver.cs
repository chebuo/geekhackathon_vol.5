using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class WiimoteRotationReceiver : MonoBehaviour
{
    private UdpClient udp;
    private Thread thread;

    // デバイスごとの回転データを保持
    public float[] deviceRoll = new float[2];   // インデックス0:デバイス1, 1:デバイス2
    public float[] devicePitch = new float[2];
    public float[] deviceYaw = new float[2];

    void Start()
    {
        udp = new UdpClient(9000);
        thread = new Thread(ReceiveData);
        thread.IsBackground = true;
        thread.Start();
    }

    private void ReceiveData()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        while (true)
        {
            try
            {
                byte[] data = udp.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                string[] parts = message.Split(',');

                // Device[N],roll,pitch,yaw の形式でデータを受信
                if (parts.Length == 4 &&
                    parts[0].StartsWith("Device") &&
                    int.TryParse(parts[0].Substring(6), out int deviceId) &&
                    float.TryParse(parts[1], out float r) &&
                    float.TryParse(parts[2], out float p) &&
                    float.TryParse(parts[3], out float y))
                {
                    // デバイスIDは1から始まるので、配列のインデックスに合わせて-1
                    int index = deviceId - 1;
                    if (index >= 0 && index < 2)
                    {
                        deviceRoll[index] = r;
                        devicePitch[index] = p;
                        deviceYaw[index] = y;
                        Debug.Log($"Device{deviceId}: Roll={r:F2}, Pitch={p:F2}, Yaw={y:F2}");
                    }
                }
            }
            catch (SocketException ex)
            {
                Debug.Log("UDP受信エラー: " + ex.Message);
                break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        udp?.Close();
        thread?.Abort();
    }
}