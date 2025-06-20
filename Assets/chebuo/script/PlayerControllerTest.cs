using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    public static float time;
    public static bool isGame = false;
    public static bool isGoal = false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGame) return;
        if (isGame&&!isGoal)
        {
            time += Time.deltaTime;
        }
        //Debug.Log(time);
        if (!UDPSensorReceiver.isConnect) return;
        Debug.Log("tomato");
        rb.AddForce(Vector3.back * -5);
        if (rb.velocity.magnitude > 40)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 40);
        }
        rb.AddForce(Vector3.back * UDPSensorReceiver.balance_x / 4);
        rb.AddForce(Vector3.back * UDPSensorReceiver.balance_x / 4);
        rb.AddForce(Vector3.right * UDPSensorReceiver.balance_z / 4);
        rb.AddForce(Vector3.right * UDPSensorReceiver.balance_z / 4);
        
        if (UDPSensorReceiver.stop)
        {
            rb.AddForce(0, 0, 0);
        }
       /* if (UDPSensorReceiver.isJump)
        {
            rb.AddForce(Vector3.up*10);
        }*/

        Transform transform = this.transform;
        Vector3 worldAngle = transform.eulerAngles;
        if (Mathf.Abs(UDPSensorReceiver.balance_z) < 60)
        {
            worldAngle.y = UDPSensorReceiver.balance_z;
            transform.eulerAngles = worldAngle;
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
