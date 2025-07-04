using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UIElements;

public class UDPSensorReceiver : MonoBehaviour
{
    UdpClient udp;
    Thread thread;
    int port = 27335;

    bool hasLoggedConnection = false;
    public static float topLeft = 0f;
    public static float topRight = 0f;
    public static float bottomLeft = 0f;
    public static float bottomRight = 0f;
    float weight;
    public static float balance_x;
    public static float balance_z;

    public static float jumpforce;
    object lockObj = new object();
    public static bool isConnect;
    public static bool isJump;
    public static bool stop;
    public static bool isRide=false;
    float jumping=0;
    void Start()
    {
        udp = new UdpClient(port);
        thread = new Thread(new ThreadStart(ReceiveData));
        thread.IsBackground = true;
        thread.Start();
    }

    void Update()
    {
        if (isConnect && !hasLoggedConnection)
        {
            Debug.Log("バランスボードに接続されました！");
            hasLoggedConnection = true;
        }
        balance_x = 0f;
        balance_z = 0f;
       
        lock (lockObj)
        {          
            balance_x = (-(topLeft+topRight) + (bottomLeft+bottomRight))/2; // 左右バランス       
            balance_z = ((topRight + bottomRight) - (topLeft + bottomLeft)) / 2; // 前後バランス
        }
      
        if (weight < 15f)//jumpしたかどうか
        {
            if(isRide)isJump = true;
        }
        else
        {
            isJump = false;
        }
        if (isJump)//乗っているかどうか
        {
            Debug.Log(isJump);
            jumping += Time.deltaTime;
            if (jumping > 2)
            {
                isRide = false;
            }
        }
        else
        {
            jumping = 0;
            isRide= true;
        }

        if (!isRide)
        {
            isJump= false;
        }

        if (topLeft - topRight < 5 && topLeft - bottomLeft < 5 && topRight - bottomRight < 5 && bottomLeft - bottomRight < 5)//重心がとれているかどうか
        {
            stop=true;
        }
        else
        {
            stop = false;
        }
        Debug.Log(weight);
        //Debug.Log($"Received TL:{topLeft} TR:{topRight} BL:{bottomLeft} BR:{bottomRight}");
        //Debug.Log(isJump);
        //rb.AddForce(balance / 5, 0, 0);
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
                    weight = (topLeft + topRight + bottomLeft + bottomRight) / 4;
                    /*if (weight > 10)
                    {
                        isRide = true;
                    }
                    else
                    {
                        isRide = false;
                    }*/
                    topLeft = RoundDown(topLeft);
                    topRight=RoundDown(topRight);
                    bottomLeft = RoundDown(bottomLeft);
                    bottomRight = RoundDown(bottomRight);
                }
                isConnect = true;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
                isConnect = false;
            }
        }
    }
    float RoundDown(float boardedge)//乗っていないときに出る値の切り捨て
    {
        if (Mathf.Abs(boardedge) < 10)
        {
            boardedge = 0;
        }
        return boardedge;
    }
    void OnApplicationQuit()
    {
        if (thread != null && thread.IsAlive) thread.Abort();
        if (udp != null) udp.Close();
    }
}
