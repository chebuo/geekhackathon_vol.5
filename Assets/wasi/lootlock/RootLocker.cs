using UnityEngine;

/// <summary>
/// RootLocker �̊ȈՎ����BPlayerPrefs���g����JSON�������ۑ��E�ǂݍ��݁B
/// ���ۂ̃A�v���ł̓N���E�h��t�@�C���x�[�X�̕ۑ��ɍ����ւ��Ă��������B
/// </summary>
public static class RootLocker
{
    public static void Save(string key, string json)
    {
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static string Load(string key)
    {
        return PlayerPrefs.GetString(key, "");
    }
}