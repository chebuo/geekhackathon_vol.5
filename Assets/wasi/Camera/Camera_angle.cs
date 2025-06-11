using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_angle : MonoBehaviour
{
    public Transform target; // プレイヤー
    public Vector3 offset = new Vector3(0, 2, -5); // カメラの相対的な位置

    void Update()
    {
        // オフセットをターゲットのローカル回転を考慮して適用
        Vector3 rotatedOffset = target.rotation * offset;

        // カメラの位置を更新
        transform.position = target.position + rotatedOffset;

        // カメラがターゲットの方向を見るように設定
        transform.LookAt(target);
    }
}
