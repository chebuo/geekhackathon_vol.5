using System.Diagnostics;
using UnityEngine;
using System.IO;

public class BalanceBoardConnector : MonoBehaviour
{
    string balanceWalkerPath = Application.streamingAssetsPath+"/BalanceWiiBoard/wiiboardSender.exe";

    public void ConnectBoard()
    {
        if (File.Exists(balanceWalkerPath))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(balanceWalkerPath)
            {
                UseShellExecute = true // .exe を起動するだけで十分
            };

            Process.Start(startInfo);
            UnityEngine.Debug.Log("BalanceWalker を起動しました。Bluetooth接続を自動で行ってください。");
        }
        else
        {
            UnityEngine.Debug.LogError("BalanceWalker.exe が見つかりません: " + balanceWalkerPath);
        }
    }
}
