using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPSensorReceiver : MonoBehaviour
{
    UdpClient udp;
    Thread thread;
    int port = 27335;
    string lastMessage = "";

    void Start()
    {
        udp = new UdpClient(port);
        thread = new Thread(new ThreadStart(ReceiveData));
        thread.IsBackground = true;
        thread.Start();
    }
    void Update()
    {
        if (!string.IsNullOrEmpty(lastMessage))
        {
            string[] sensors = lastMessage.Split(',');
            float tl = float.Parse(sensors[0].Split(':')[1]);
            float tr = float.Parse(sensors[1].Split(':')[1]);

            float balance = tr - tl; // ŠÈˆÕƒoƒ‰ƒ“ƒX
            transform.position = new Vector3(balance, 0, 0);
        }
    }

    void ReceiveData()
    {
        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
            byte[] data = udp.Receive(ref remoteEP);
            lastMessage = Encoding.ASCII.GetString(data);
            Debug.Log("Received: " + lastMessage);
        }
    }

    void OnApplicationQuit()
    {
        if (thread != null) thread.Abort();
        udp.Close();
    }
}
