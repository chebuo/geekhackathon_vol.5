using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_bode : MonoBehaviour
{
    public float bode_spead = 0.50f;
    private float bode_rotation_spead = 40.0f;
    public float bode_x;
    public float bode_y;
    public float bode_z;
    public float maxRotation = 30.0f; // 最大角度
    public float minRotation = -30.0f; // 最小角度
    private float nowRotation_x = 0f;
    private float nowRotation_z = 0f;

    Rigidbody rb;
    private void Start()
    {
       rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        bode_x = UDPSensorReceiver.balance_x;
        bode_z = UDPSensorReceiver.balance_z;
        if (UDPSensorReceiver.isJump)
        {
            bode_y += 0.1f;
        }
        else
        {
            bode_y = 0;
        }
        //Debug.Log(bode_x + "::" + bode_z);
        Vector3 velocity = new Vector3(bode_x, 0, bode_z);
        Vector3 direction = velocity.normalized;//ベクトルの向きを取得

        float distance = bode_spead * Time.deltaTime;
        Vector3 destination = transform.position + direction * distance;

        float bode_rol_x = bode_x * Time.deltaTime * bode_rotation_spead;
        float bode_rol_z = bode_z * Time.deltaTime * bode_rotation_spead;
        float bode_move = bode_z * bode_spead * Time.deltaTime;//縦方向の移動量の取得
        nowRotation_x += bode_rol_x;
        nowRotation_z += bode_rol_z;
        nowRotation_x = Mathf.Clamp(nowRotation_x, minRotation, maxRotation);//左右のボードの回転制限
        nowRotation_z = Mathf.Clamp(nowRotation_z, minRotation, maxRotation);//縦のボードの回転制限
        transform.localRotation = Quaternion.Euler(0, nowRotation_x, 0);//ボードの傾き
        if (UDPSensorReceiver.stop)
        {
            velocity = new Vector3(0, 0, 0);
        }
            transform.Translate(velocity);//移動
        rb.AddForce(0, bode_y, 0);
    }
}
