using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class RPCControll : NetworkBehaviour
{
    [SerializeField] private TMP_Text chatText;
    [SerializeField] private TMP_InputField inputfield;
    [SerializeField] private GameObject canvas;
    private static event Action<string> onMessage;
    [SyncVar]
    int currentPlayerIndex = 0;
    //client 가 server에 connect 되었을 때 콜백 함수 
    public override void OnStartAuthority()
    {

        if (isLocalPlayer)
        {
            canvas.SetActive(true);
        }

        onMessage += newMessage;
    }
    private void newMessage(string mess)
    {
        chatText.text += mess;
    }
    //클라이언트가 Server를 나갔을 때
    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }

    [Client]
    public void Send()
    {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (string.IsNullOrWhiteSpace(inputfield.text)) return;
        string name = SQLManager.instance.info.User_name;

    CmdSendMessage(inputfield.text , name);
        inputfield.text = string.Empty;
    }

    //RPC는 결국 ClientRpc 명령어 < command 명령어 < client 명령어
    [Command(requiresAuthority = false)]
    private void CmdSendMessage(string Message , string name)
    {
        Debug.Log("커멘드실행");
        RPCHandleMessage($"[{name}] : {Message}");
    }

    [ClientRpc]
    private void RPCHandleMessage(string message)
    {
        onMessage?.Invoke($"\n{message}");
    }
}

