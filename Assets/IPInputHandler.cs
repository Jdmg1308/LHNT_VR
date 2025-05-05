using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IPInputHandler : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public WebSocketClient wsClient;

    public void Start()
    {
        ipInputField.text = wsClient.IP_address;
    }

    public void Connect()
    {
        string ip = ipInputField.text;
        if (ip != null && !ip.Contains(" ") && ip.Length > 0)
        {
            wsClient.ConnectToWebSocket(ip);
        }
    }
}
