using UnityEngine;

public class move_stic : MonoBehaviour
{
    public WiimoteRotationReceiver receiver;
    public int deviceIndex = 0;              // 0 = Device1, 1 = Device2
    public Rigidbody rb;
    public float accelerationScale = 1.0f;

    void FixedUpdate()
    {
        if (receiver == null || rb == null) return;
        if (deviceIndex < 0 || deviceIndex >= 2) return;

        // 受信済みの角度（度数法）を取得
        //float pitch = receiver.devicePitch[deviceIndex];
        //float yaw = receiver.deviceYaw[deviceIndex];
        //float roll = receiver.deviceRoll[deviceIndex];

        // 必要なら Vector3 にまとめても OK
        //Vector3 euler = new Vector3(pitch, yaw, roll);

        // 例：ピッチで前後に加速
        //float forwardForce = Mathf.Sin(pitch * Mathf.Deg2Rad) * accelerationScale;
        //Vector3 force = new Vector3(0f, 0f, forwardForce);

        //rb.AddForce(force, ForceMode.Acceleration);
    }
}
