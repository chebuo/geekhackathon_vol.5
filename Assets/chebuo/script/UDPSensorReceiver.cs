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

    float topLeft = 0f;
    float topRight = 0f;
    float bottomLeft = 0f;
    float bottomRight = 0f;
    float weight;
    object lockObj = new object();
    bool isJump;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        udp = new UdpClient(port);
        thread = new Thread(new ThreadStart(ReceiveData));
        thread.IsBackground = true;
        thread.Start();
    }

    void Update()
    {
        float balance = 0f;
        lock (lockObj)
        {
            balance = topRight - topLeft; // 簡易的な左右バランス
            weight=(topLeft+topRight+bottomLeft+bottomRight)/ 4;
        }
        if (weight < 2f) 
        {
            isJump = true;
        }
        else
        {
            isJump=false;
        }
        Debug.Log(isJump);
        rb.AddForce(balance / 5, 0, 0);
        //transform.position = new Vector3(balance, 0, 0);
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
                byte[] data = udp.Receive(ref remoteEP);
                string message = Encoding.ASCII.GetString(data);
                // 例: "TL:12.3,TR:11.8,BL:10.0,BR:9.9"
                string[] sensors = message.Split(',');

                lock (lockObj)
                {
                    topLeft = float.Parse(sensors[0].Split(':')[1]);
                    topRight = float.Parse(sensors[1].Split(':')[1]);
                    bottomLeft = float.Parse(sensors[2].Split(':')[1]);
                    bottomRight = float.Parse(sensors[3].Split(':')[1]);
                }

                //Debug.Log($"Received TL:{topLeft} TR:{topRight} BL:{bottomLeft} BR:{bottomRight}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("UDP Receive Error: " + ex.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (thread != null && thread.IsAlive) thread.Abort();
        if (udp != null) udp.Close();
    }
}
