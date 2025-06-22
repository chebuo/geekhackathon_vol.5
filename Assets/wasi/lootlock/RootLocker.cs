using UnityEngine;

/// <summary>
/// RootLocker の簡易実装。PlayerPrefsを使ってJSON文字列を保存・読み込み。
/// 実際のアプリではクラウドやファイルベースの保存に差し替えてください。
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