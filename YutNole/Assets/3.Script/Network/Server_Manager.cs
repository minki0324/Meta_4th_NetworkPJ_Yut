using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Server_Manager : NetworkBehaviour
{
    public static Server_Manager instance;
    [SerializeField] private Time_Slider slider;
    private PlayingYut playingYut;
    #region Script
    #endregion

    #region SyncVar
    [SyncVar(hook = nameof(OnTurn_Finish))] 
    public int Turn_Index = 2;
    #endregion

    #region Client
    [Client]
    private void GameStart()
    {
        CMD_Turn_Changer();
        CMD_Turn_Changer();
    }

    [Client]
    public void Catch(PlayerState target)
    {
        CmdCatch(target);
    }

   
    #endregion

    #region Command
    // 턴 바꾸는 CMD
    [Command(requiresAuthority = false)]
    public void CMD_Turn_Changer()
    {
        int next_Index = (Turn_Index % 2) + 1;
        OnTurn_Finish(Turn_Index, next_Index);
    }

    [Command(requiresAuthority = false)]
    public void CmdCatch( PlayerState target)
    {

        RPCCatch(target);
    }
    #endregion

    #region ClientRPC
    [ClientRpc]
    public void RPCCatch( PlayerState target)
    {
        if (GM.instance.Player_Num == Player_Num.P1 && target.gameObject.CompareTag("Player1"))
        {
            target.transform.position = target.startPos.position;
            //target.currentIndex = 0;
            //target.currentArray = playingYut.pos1;

        }
        else if (GM.instance.Player_Num == Player_Num.P2 && target.gameObject.CompareTag("Player2"))
        {
            target.transform.position = target.startPos.position;
            //target.currentIndex = 0;
            //target.currentArray = playingYut.pos1;
        }

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

    private void Start()
    {
        GameStart();
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
