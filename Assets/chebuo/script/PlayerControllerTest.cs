using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UDPSensorReceiver.isConnect) return;
        rb.AddForce(Vector3.back * -5);
        if (rb.velocity.magnitude < 20)
        { 
            rb.AddForce(Vector3.back * UDPSensorReceiver.balance_x / 8);
            rb.AddForce(Vector3.back * UDPSensorReceiver.balance_x / 8);
            rb.AddForce(Vector3.right * UDPSensorReceiver.balance_z / 8);
            rb.AddForce(Vector3.right * UDPSensorReceiver.balance_z / 8);
        }
        if (UDPSensorReceiver.stop)
        {
            rb.AddForce(0, 0, 0);
        }
        if (UDPSensorReceiver.isJump)
        {
            float velocity_x = rb.velocity.x;
            float velocity_z = rb.velocity.z;
            rb.AddForce(Vector3.up*10);
        }

        Transform transform = this.transform;
        Vector3 worldAngle = transform.eulerAngles;
        if (Mathf.Abs(UDPSensorReceiver.balance_z) < 60)
        {
            worldAngle.y = UDPSensorReceiver.balance_z;
            transform.eulerAngles = worldAngle;
        }
    }
}
