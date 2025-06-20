using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class WiimoteRotationReceiver : MonoBehaviour
{
    private UdpClient udp;
    private Thread thread;

    public float[] deviceRoll = new float[2];
    public float[] devicePitch = new float[2];
    public float[] deviceYaw = new float[2];

    public GameObject wii1;
    public GameObject wii2;


    void Start()
    {
        udp = new UdpClient(9000);
        thread = new Thread(ReceiveData);
        thread.IsBackground = true;
        thread.Start();
    }

    void Update()
    {
        if (wii1 != null)
        {
            wii1.transform.rotation = Quaternion.Euler(devicePitch[0], deviceYaw[0], deviceRoll[0]);
        }

        if (wii2 != null)
        {
            wii2.transform.rotation = Quaternion.Euler(devicePitch[1], deviceYaw[1], deviceRoll[1]);
        }
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

                if (parts.Length == 4 &&
                    parts[0].StartsWith("Device") &&
                    int.TryParse(parts[0].Substring(6), out int deviceId) &&
                    float.TryParse(parts[1], out float r) &&
                    float.TryParse(parts[2], out float p) &&
                    float.TryParse(parts[3], out float y))
                {
                    int index = deviceId - 1;
                    if (index >= 0 && index < 2)
                    {
                        deviceRoll[index] = r;
                        devicePitch[index] = p;
                        deviceYaw[index] = y;
                    }
                }
            }
            catch (SocketException ex)
            {
                Debug.Log("UDPóMƒGƒ‰[: " + ex.Message);
                break;
            }
        }
    }

    void OnApplicationQuit()
    {
        udp?.Close();
        thread?.Abort();
    }
}