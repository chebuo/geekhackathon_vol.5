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
                UseShellExecute = true // .exe ���N�����邾���ŏ\��
            };

            Process.Start(startInfo);
            UnityEngine.Debug.Log("BalanceWalker �N��Bluetooth�ڑ�");
        }
        else
        {
            UnityEngine.Debug.LogError("BalanceWalker.exe ��������܂���: " + balanceWalkerPath);
        }
    }
}
