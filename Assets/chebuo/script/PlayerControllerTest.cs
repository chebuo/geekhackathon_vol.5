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
        rb.AddForce(Vector3.back*UDPSensorReceiver.balance_x);
        rb.AddForce(Vector3.back*UDPSensorReceiver.balance_x);
        rb.AddForce(Vector3.right * UDPSensorReceiver.balance_z);
        rb.AddForce(Vector3.right * UDPSensorReceiver.balance_z);
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
    }
}
