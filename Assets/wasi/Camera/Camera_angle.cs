using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_angle : MonoBehaviour
{
    public Transform target; // �v���C���[
    public Vector3 offset = new Vector3(0, 2, -5); // �J�����̑��ΓI�Ȉʒu

    void Update()
    {
        // �I�t�Z�b�g���^�[�Q�b�g�̃��[�J����]���l�����ēK�p
        Vector3 rotatedOffset = target.rotation * offset;

        // �J�����̈ʒu���X�V
        transform.position = target.position + rotatedOffset;

        // �J�������^�[�Q�b�g�̕���������悤�ɐݒ�
        transform.LookAt(target);
    }
}
