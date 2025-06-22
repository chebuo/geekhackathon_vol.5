using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    public static float time;
    public static bool isGame = false;
    public static bool isGoal = false;
    Rigidbody rb;
    private int rotation_sub=80;
    private int rotation_rimit = 40;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isGame) return;
        if (isGame && !isGoal)
        {
            time += Time.deltaTime;
        }

        if (!UDPSensorReceiver.isConnect) return;

        // センサーデータのログ出力
        Debug.Log("balance_x: " + UDPSensorReceiver.balance_x);
        Debug.Log("balance_z: " + UDPSensorReceiver.balance_z);
        Debug.Log("stop: " + UDPSensorReceiver.stop);
        // Debug.Log("isJump: " + UDPSensorReceiver.isJump); // 必要に応じて

        //rb.AddForce(this.transform.forward * 5);
        if (rb.velocity.magnitude > 40)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 40);
        }

        rb.AddForce(this.transform.forward * -UDPSensorReceiver.balance_x / 4);

        // 横方向の移動を削除し、角度に反映
        Transform transform = this.transform;
        Vector3 worldAngle = transform.eulerAngles;

        if (Mathf.Abs(-UDPSensorReceiver.balance_z) > rotation_rimit)
        {
            worldAngle.y -= -UDPSensorReceiver.balance_z / rotation_sub;
            transform.eulerAngles = worldAngle;
        }

        if (UDPSensorReceiver.stop)
        {
            rb.AddForce(Vector3.zero);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("goal"))
        {
            isGame = false;
            isGoal = true;
        }
        if (col.CompareTag("gate"))
        {
            time += 5;
        }
    }
}