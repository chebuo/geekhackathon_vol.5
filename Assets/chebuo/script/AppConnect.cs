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
                UseShellExecute = true // .exe ‚ğ‹N“®‚·‚é‚¾‚¯‚Å\•ª
            };

            Process.Start(startInfo);
            UnityEngine.Debug.Log("BalanceWalker ‹N“®BluetoothÚ‘±");
        }
        else
        {
            UnityEngine.Debug.LogError("BalanceWalker.exe ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ: " + balanceWalkerPath);
        }
    }
}
