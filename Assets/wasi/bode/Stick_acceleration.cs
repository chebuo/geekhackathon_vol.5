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

    [Header("�����Ώۂ̃I�u�W�F�N�g")]
    public GameObject target;
    private Rigidbody rb;

    [Header("�p���x�������l")]
    public float velocityThreshold = 120f;

    [Header("�����{���i�p���x �~ ���̒l�j")]
    public float accelerationMultiplier = 0.01f;

    [Header("�ő呬�x")]
    public float maxSpeed = 10f;

    [Header("�U������̃N�[���_�E�����ԁi�b�j")]
    public float cooldownDuration = 1.0f;

    [Header("�O�i�Ƃ݂Ȃ��P�\���ԁi�b�j")]
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
        // �N�[���_�E���̍X�V
        if (leftCooldownTimer > 0f) leftCooldownTimer -= Time.deltaTime;
        if (rightCooldownTimer > 0f) rightCooldownTimer -= Time.deltaTime;

        // �s�b�`�p���x�̌v�Z
        Vector3 euler1 = currentEuler[0];
        p = euler1.x;
        pitchSpeed1 = Mathf.Abs(p - prevP) / Time.deltaTime;
        prevP = p;

        Vector3 euler2 = currentEuler[1];
        p2 = euler2.x;
        pitchSpeed2 = Mathf.Abs(p2 - prevP2) / Time.deltaTime;
        prevP2 = p2;

        // �t���O������
        leftshake = false;
        rightshake = false;

        // �������l���� �� �N�[���_�E������ �� �t���OON
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

        // ���U��̗P�\����
        bool bothShakenWithinGrace =
            Mathf.Abs(leftShakeTime - rightShakeTime) <= gracePeriod;

        // ��������
        if (bothShakenWithinGrace)
        {
            float avgSpeed = (pitchSpeed1 + pitchSpeed2) / 2f;
            rb.AddForce(target.transform.forward * avgSpeed * accelerationMultiplier, ForceMode.Force);
            Debug.Log($"�O�i����: {avgSpeed:F1}");
            animator.SetBool("boolD", true);
        }
        else if (leftshake)
        {
            rb.AddForce(-target.transform.right * pitchSpeed1 * accelerationMultiplier, ForceMode.Force);
            Debug.Log($"������: {pitchSpeed1:F1}");
            animator.SetBool("boolL", true);
        }
        else if (rightshake)
        {
            rb.AddForce(target.transform.right * pitchSpeed2 * accelerationMultiplier, ForceMode.Force);
            Debug.Log($"�E����: {pitchSpeed2:F1}");
            animator.SetBool("boolR", true);
        }

        // ���x����
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
                Debug.Log("UDP��M�G���[: " + ex.Message);
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
