using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_stic : MonoBehaviour
{
    public WiimoteRotationReceiver receiver;
    public int deviceIndex = 0; // ← デバイス番号（0 or 1）
    public Rigidbody rb;
    public float accelerationScale = 1.0f;

    void FixedUpdate()
    {
        if (receiver != null && rb != null)
        {
            Vector3 delta = receiver.devices[deviceIndex].DeltaEuler;
            Vector3 force = new Vector3(delta.y, 0f, 0f) * accelerationScale;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }
}
