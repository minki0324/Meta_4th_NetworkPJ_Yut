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
    }

    [Client]
    public void Catch(PlayerState me, PlayerState target)
    {
        CmdCatch(me, target);
    }
    [Client]
    public void Carry(PlayerState me, PlayerState target)
    {
        CmdCarry(me, target);
    }


    #endregion

    #region Command
    // ÅÏ ¹Ù²Ù´Â CMD
    [Command(requiresAuthority = false)]
    public void CMD_Turn_Changer()
    {
        int next_Index = (Turn_Index % 2) + 1;
        OnTurn_Finish(Turn_Index, next_Index);
        RPC_Sfx();
    }

    [Command(requiresAuthority = false)]
    public void CmdCatch(PlayerState me, PlayerState target)
    {
        RPCCatch(me, target);

    }

    [Command(requiresAuthority = false)]
    public void CmdCarry(PlayerState me, PlayerState target)
    {

        RPCCarry(me, target);


    }

    #endregion

    #region ClientRPC
    [ClientRpc]
    public void RPCCatch(PlayerState me, PlayerState target)
    {
        me.GetComponent<NetworkAnimator>().SetTrigger("isCatch");
        target.transform.position = target.startPos.position;
        if (target != null)
        {
            //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ잡힌말 초기화
            target.currentIndex = 0;
            target.currentArray = target.playingYut.pos1;
            target.currentPositon = target.currentArray[0];
            //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
            //잡힌 말이 만약 업고있는 말이 있다면?  count = 업힌말 갯수
            if (target.carryPlayer.Count > 0)
            {
                //업힌말들 초기화해주고 Active 켜주기
                for (int i = 0; i < target.carryPlayer.Count; i++)
                {
                    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ초기화
                    target.carryPlayer[i].currentIndex = 0;
                    target.carryPlayer[i].currentArray = target.carryPlayer[i].playingYut.pos1;
                    target.carryPlayer[i].currentPositon = target.currentArray[0];
                    target.carryPlayer[i].transform.position = target.carryPlayer[i].startPos.position;
                    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
                    target.carryPlayer[i].gameObject.SetActive(true); // false 됬던거 true
                }
                //말 초기화해줬으면 업힌말들 담았던 리스트 초기화
                target.carryPlayer.Clear();
                // 업힌말 표기하는 숫자 오브젝트도 초기화
                target.CarryNumSetting();
            }
        }
    }

    [ClientRpc]
    private void RPC_Sfx()
    {
        if (isLocalPlayer)
        {
            if((int)GM.instance.Player_Num == Turn_Index)
            {
                AudioManager.instance.PlaySFX("YourTurn");
            }
        }
    }

    #endregion
    [ClientRpc]
    public void RPCCarry(PlayerState me, PlayerState target)
    {
        //업힌말이있는 말 = me //안업힌말 = target
        if (me.carryPlayer.Count > 0)
        {
            for (int i = 0; i < me.carryPlayer.Count; i++)
            {
                target.carryPlayer.Add(me.carryPlayer[i]);

            }
            me.carryPlayer.Clear();
        }
        me.gameObject.SetActive(false);
        target.carryPlayer.Add(me);
        target.CarryNumSetting();
    }


    #region Unity Callback
    private void Awake()
    {
        if (instance == null)
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
        if (Input.GetKeyDown(KeyCode.K))
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