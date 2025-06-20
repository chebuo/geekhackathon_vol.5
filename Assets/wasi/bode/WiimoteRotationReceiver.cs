using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class WiimoteRotationReceiver : MonoBehaviour
{
    private UdpClient udp;
    private Thread receiveThread;

    [System.Serializable]
    public class WiimoteData
    {
        public float roll;
        public float pitch;
        public float yaw;

        public Vector3 CurrentEuler => new Vector3(pitch, yaw, roll);
        public Vector3 PreviousEuler;
        public Vector3 DeltaEuler;
        public GameObject targetObject;
    }

    public WiimoteData[] devices = new WiimoteData[2];

    void Start()
    {
        // ‰Šú‰»
        for (int i = 0; i < devices.Length; i++)
            devices[i] = new WiimoteData();

        udp = new UdpClient(9000);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        for (int i = 0; i < devices.Length; i++)
        {
            var device = devices[i];

            Vector3 current = device.CurrentEuler;
            device.DeltaEuler = current - device.PreviousEuler;
            device.PreviousEuler = current;

            if (device.targetObject != null)
                device.targetObject.transform.rotation = Quaternion.Euler(current);
        }
    }

    private void ReceiveData()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
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
                    if (index >= 0 && index < devices.Length)
                    {
                        devices[index].roll = r;
                        devices[index].pitch = p;
                        devices[index].yaw = y;
                    }
                }
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("UDPóMƒGƒ‰[: " + ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        udp?.Close();
        if (receiveThread != null && receiveThread.IsAlive)
            receiveThread.Abort();
    }
}