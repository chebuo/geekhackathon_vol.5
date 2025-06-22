using System.Collections;
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

    [Header("角速度しきい値")]
    public float velocityThreshold = 120f;

    [Header("加速倍率（角速度 × この値）")]
    public float accelerationMultiplier = 0.01f;

    [Header("最大速度")]
    public float maxSpeed = 10f;

    [Header("振った後のクールダウン時間（秒）")]
    public float cooldownDuration = 1.0f;

    [Header("前進とみなす猶予時間（秒）")]
    public float gracePeriod = 0.3f;

    private float leftCooldownTimer = 0f;
    private float rightCooldownTimer = 0f;

    public bool leftshake = false;
    public bool rightshake = false;

    private float leftShakeTime = -10f;
    private float rightShakeTime = -10f;

    Animator animator;
    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
        udp = new UdpClient(9000);
        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();
        animator= gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // クールダウンの更新
        if (leftCooldownTimer > 0f) leftCooldownTimer -= Time.deltaTime;
        if (rightCooldownTimer > 0f) rightCooldownTimer -= Time.deltaTime;

        // ピッチ角速度の計算
        Vector3 euler1 = currentEuler[0];
        p = euler1.x;
        pitchSpeed1 = Mathf.Abs(p - prevP) / Time.deltaTime;
        prevP = p;

        Vector3 euler2 = currentEuler[1];
        p2 = euler2.x;
        pitchSpeed2 = Mathf.Abs(p2 - prevP2) / Time.deltaTime;
        prevP2 = p2;

        // フラグ初期化
        leftshake = false;
        rightshake = false;

        // しきい値超え ＆ クールダウン完了 → フラグON
        if (pitchSpeed1 >= velocityThreshold && leftCooldownTimer <= 0f)
        {
            leftshake = true;
            leftCooldownTimer = cooldownDuration;
            leftShakeTime = Time.time;
        }

        if (pitchSpeed2 >= velocityThreshold && rightCooldownTimer <= 0f)
        {
            rightshake = true;
            rightCooldownTimer = cooldownDuration;
            rightShakeTime = Time.time;
        }

        // 両振りの猶予判定
        bool bothShakenWithinGrace =
            Mathf.Abs(leftShakeTime - rightShakeTime) <= gracePeriod;

        // 加速処理
        if (bothShakenWithinGrace)
        {
            float avgSpeed = (pitchSpeed1 + pitchSpeed2) / 2f;
            rb.AddForce(target.transform.forward * avgSpeed * accelerationMultiplier, ForceMode.Force);
            Debug.Log($"前進加速: {avgSpeed:F1}");
            animator.SetBool("boolD", true);
        }
        else if (leftshake)
        {
            rb.AddForce(-target.transform.right * pitchSpeed1 * accelerationMultiplier, ForceMode.Force);
            Debug.Log($"左加速: {pitchSpeed1:F1}");
            animator.SetBool("boolL", true);
        }
        else if (rightshake)
        {
            rb.AddForce(target.transform.right * pitchSpeed2 * accelerationMultiplier, ForceMode.Force);
            Debug.Log($"右加速: {pitchSpeed2:F1}");
            animator.SetBool("boolR", true);
        }

        // 速度制限
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
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
