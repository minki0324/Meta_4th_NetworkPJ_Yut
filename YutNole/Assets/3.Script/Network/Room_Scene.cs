using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Room_Scene : NetworkBehaviour
{
    [SerializeField] private Room_Player P1_Component;
    [SerializeField] private Room_Player P2_Component;
    [SerializeField] private Room_Manager manager;

    [SyncVar]
    public bool isPlayer1 = false;

    public bool isReady = false;
    public bool isReady_P1;
    public bool isReady_P2;

    [SerializeField]
    private Text Player1_Text;

    [SerializeField]
    private Text Player2_Text;

    [SerializeField]
    private Image Player1_img;

    [SerializeField]
    private Image Player2_img;

    [SerializeField]
    private Text Player1_Ready;

    [SerializeField]
    private Text Player2_Ready;

    [SerializeField]
    private Button Ready_Btn;

    #region Unity Callback
    private void Start()
    {
        manager = FindObjectOfType<Room_Manager>();

        isReady_P1 = false;
        isReady_P2 = false;
    }

    private void Update()
    {
        Find_Player();
        GetPlayerName();
    }
    #endregion

    private void Find_Player()
    {
        if (manager.roomSlots.Count == 0)
        {
            return;
        }
        else if (manager.roomSlots.Count == 2)
        {
            P1_Component = manager.roomSlots[0].GetComponent<Room_Player>();
            P2_Component = manager.roomSlots[1].GetComponent<Room_Player>();
        }
        else if (manager.roomSlots.Count == 1)
        {
            P1_Component = manager.roomSlots[0].GetComponent<Room_Player>();
        }
       
    }

    private void GetPlayerName()
    {
        if (manager.roomSlots.Count == 1)
        {
            if (P1_Component.isLocalPlayer)
            {
                Player1_Text.text = SQLManager.instance.info.User_name;
            }
            else
            {
                return;
            }
        }
        else if (manager.roomSlots.Count == 2)
        {
            if (P1_Component.isLocalPlayer)
            {
                Player1_Text.text = SQLManager.instance.info.User_name;
            }
            else if (P2_Component.isLocalPlayer)
            {
                Player2_Text.text = SQLManager.instance.info.User_name;
            }
        }
        else return;
    }




    [Command]       //클라이언트가 서버로 RPC를 요청하는 함수, 호출하는 부분은 클라이언트지만 함수가 실행되는 부분은 서버
    public void Cmd_OnReadyBtn_Click()
    {
        if (manager.roomSlots.Count == 1)    //룸에 들어온 인원이 1명인 경우
        {
            if (P1_Component.isLocalPlayer)  //p1컴포넌트가 로컬인경우 버튼이 눌리면 on 아니면 off
            {
                if (P1_Component.readyToBegin)
                {
                    P1_Component.CmdChangeReadyState(false);
                    isReady_P1 = false;
                }
                else
                {
                    P1_Component.CmdChangeReadyState(true);
                    isReady_P1 = true;
                }
            }
            else
            {
                return;
            }
        }
        else if (manager.roomSlots.Count == 2)   //룸에 들어온 인원이 2명인 경우
        {
            if (P1_Component.isLocalPlayer)     //p1컴포넌트가 로컬인경우
            {
                if (P1_Component.readyToBegin)
                {
                    P1_Component.CmdChangeReadyState(false);
                    isReady_P1 = false;
                }
                else
                {
                    P1_Component.CmdChangeReadyState(true);
                    isReady_P1 = true;
                }
            }
            else if (P2_Component.isLocalPlayer)    //p2컴포넌트가 로컬인경우
            {
                if (P2_Component.readyToBegin)
                {
                    P2_Component.CmdChangeReadyState(false);
                    isReady_P2 = false;
                }
                else
                {
                    P2_Component.CmdChangeReadyState(true);
                    isReady_P2 = true;
                }
            }
            else
            {
                return;
            }

            Rpc_ReadyClicked();
        }
    }



    [ClientRpc]     //서버의 오브젝트에서 호출되어서 클라이언트의 오브젝트에서 수행

    public void Rpc_ReadyClicked()
    {
        if (isReady_P1)
        {
            Player1_Ready.text = "READY";
        }
        else
        {
            Player1_Ready.text = string.Empty;
        }

        if (isReady_P2)
        {
            Player2_Ready.text = "READY";
        }
        else
        {
            Player2_Ready.text = string.Empty;
        }

        Rpc_StartCheck();

    }

    [ClientRpc]
    public void Rpc_StartCheck()
    {
        if (isReady_P1 && isReady_P2)
        {
            //게임시작  
        }
    }


    [Client]
    public void Ready_IsClicked()
    {
        Debug.Log("zggg");
        if (isReady)
        {
            isReady = false;
            Ready_Btn.GetComponent<Image>().color = Color.black;
        }
        else
        {
            isReady = true;
            Ready_Btn.GetComponent<Image>().color = Color.white;
        }
    }

}
