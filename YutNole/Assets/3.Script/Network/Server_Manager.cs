using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server_Manager : NetworkBehaviour
{
    public static Server_Manager instance;

    #region Script
    [SerializeField] private Yut_Gacha Yut_Ani;
    #endregion

    #region SyncVar
    [SyncVar(hook = nameof(OnTurn_Finish))] 
    public int Turn_Index = 1;

    [SyncVar]
    public string ThrowResult = string.Empty;
    #endregion

    #region Client
    [Client]
    public void Yut_Btn_Click()
    {

    }

    #endregion

    #region Command
    // 윷 던지는 애니메이션 CMD
    [Command(requiresAuthority = false)]
    private void CMDYut_Throwing()
    {
        string[] triggers = { "Do", "Do", "Do", "Backdo", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo", "Nack", "Nack" };
        ThrowResult = triggers[Random.Range(0, triggers.Length)];

        RPCYut_Throwing(ThrowResult);
    }

    // 턴 바꾸는 CMD
    [Command(requiresAuthority = false)]
    public void CMD_Turn_Changer()
    {
        int next_Index = (Turn_Index % 2) + 1;
        OnTurn_Finish(Turn_Index, next_Index);
    }

    
    #endregion

    #region ClientRPC
    // 애니메이션 출력 RPC
    [ClientRpc]
    private void RPCYut_Throwing(string trigger)
    {
        Debug.Log("RpcRPCYut_Throwing 호출됨");
        Yut_Ani.Throwing(trigger);
    }
    #endregion

    #region Unity Callback
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            CMD_Turn_Changer();
        }
    }
    #endregion

    #region Hook Method
    public void OnTurn_Finish(int _old, int _new)
    {
        Turn_Index = _new;
    }
    #endregion


   
}
