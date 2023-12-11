using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
//using Mirror.Examples.NetworkRoom;

public class Room_Scene : NetworkBehaviour
{
    [SerializeField] private Room_Player P1_Component;
    [SerializeField] private Room_Player P2_Component;
    [SerializeField] private Room_Manager manager;



    //[SyncVar]
    private string P1_name;

   //[SyncVar]
    private string P2_name;


    public bool isReady = false;
    [SyncVar(hook = nameof(ReadyChange))]
    private string Ready = string.Empty;


    //[SyncVar]
    public bool isReady_P1;
    //[SyncVar]
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

    private int previous_count = 0;

    #region Unity Callback
    private void Start()
    {

        manager = FindObjectOfType<Room_Manager>();

        isReady_P1 = false;
        isReady_P2 = false;

        Player1_Ready.gameObject.SetActive(false);
        Player2_Ready.gameObject.SetActive(false);

        if (isServer)
        {
            P1_name = string.Empty;
            P2_name = string.Empty;
        }

    }

    private void Update()
    {
        Find_Player();

        if(isClient)
        {

            GetPlayerName();

        }
        Debug.Log("P1  Name : " + P1_name);
        Debug.Log("P2  Name : " + P2_name);



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



    #region Client
    [Client]
    public void GetPlayerName()
    {
        Debug.Log("Client");
        SQLManager.instance.isLogined = false;
        if (manager.roomSlots.Count == 1)
        {
            if (P1_Component.isLocalPlayer)
            {
                P1_name = SQLManager.instance.info.User_name;
                Player1_Text.text = P1_name;
                CMD_GetPlayerName(P1_name);
            }
        }
        else if (manager.roomSlots.Count == 2)
        {
            if (P1_Component.isLocalPlayer)
            {
                P1_name = SQLManager.instance.info.User_name;
                Player1_Text.text = P1_name;
                CMD_GetPlayerName(P1_name);
            }
            else if (P2_Component.isLocalPlayer)
            {
                P2_name = SQLManager.instance.info.User_name;
                Player2_Text.text = P2_name;
                CMD_GetPlayerName(P2_name);
            }
        }





        //string name = SQLManager.instance.info.User_name;
        //CMD_GetPlayerName(name);



    }
    #endregion


    #region Command

    [Command(requiresAuthority = false)]
    private void CMD_GetPlayerName(string name)
    {
        Debug.Log("Command");
        Rpc_GetPlayerName(name);
    }
    #endregion

    #region ClientRPC
    [ClientRpc]
    private void Rpc_GetPlayerName(string name)
    {
        Debug.Log("ClientRPC");

        StartCoroutine(Set_Name_Co(name));
    }
    #endregion

    private IEnumerator Set_Name_Co(string name)
    {
        yield return null; 
       
        if (manager.roomSlots.Count == 1)
        {
            if (P1_Component.isLocalPlayer)
            {

                Player1_Text.text = name;
            }
        
        }
        else if (manager.roomSlots.Count == 2)
        {
            if (P1_Component.isLocalPlayer)
            {
                Player1_Text.text = name;
            }
            else if (P2_Component.isLocalPlayer)
            {
                Player2_Text.text = name;
            }
        }

    }






    
    //READY 버튼-----------------------------------------------------------------------------
    private void ReadyChange(string _old, string _new)
    {
        Ready = _new;
    }


    [Client]
    public void Ready_IsClicked()
    {

        #region 눌림/안눌림 버튼 색상 변화 -> 동기화 되지 말아야하는 부분
        Debug.Log("Button Client");

        if (manager.roomSlots.Count == 1)
        {
            if (P1_Component.isLocalPlayer)
            {
                if (isReady)
                {
                    isReady = false;
                    Player1_Ready.gameObject.SetActive(false);
                    Ready_Btn.GetComponent<Image>().color = Color.black;
                }
                else
                {
                    isReady = true;
                    Player1_Ready.gameObject.SetActive(true);
                    Ready_Btn.GetComponent<Image>().color = Color.white;
                }
            }
            else
            {
                return;
            }


        }
        else if (manager.roomSlots.Count == 2)   //룸에 들어온 인원이 2명인 경우
        {
            if (P1_Component.isLocalPlayer)
            {
                if (isReady)
                {
                    isReady = false;
                    Player1_Ready.gameObject.SetActive(false);
                    Ready_Btn.GetComponent<Image>().color = Color.black;
                }
                else
                {
                    isReady = true;
                    Player1_Ready.gameObject.SetActive(true);
                    Ready_Btn.GetComponent<Image>().color = Color.white;
                }
            }
            else if (P2_Component.isLocalPlayer)
            {
                if (isReady)
                {
                    isReady = false;
                    Player2_Ready.gameObject.SetActive(false);
                    Ready_Btn.GetComponent<Image>().color = Color.black;
                }
                else
                {
                    isReady = true;
                    Player2_Ready.gameObject.SetActive(true);
                    Ready_Btn.GetComponent<Image>().color = Color.white;
                }
            }

        }
        else
        {
            return;
        }

        #endregion 

        Cmd_OnReadyBtn_Click();

    }


    [Command(requiresAuthority = false)]
    public void Cmd_OnReadyBtn_Click()
    {
        Debug.Log("Button Command");
        if (manager.roomSlots.Count == 1)    //룸에 들어온 인원이 1명인 경우
        {

            if (P1_Component.readyToBegin)
            {
                P1_Component.CmdChangeReadyState(false);
                Player1_Ready.gameObject.SetActive(false);
                isReady_P1 = false;
            }
            else
            {
                P1_Component.CmdChangeReadyState(true);
                Player1_Ready.gameObject.SetActive(true);
                isReady_P1 = true;
            }


        }
        else if (manager.roomSlots.Count == 2)   //룸에 들어온 인원이 2명인 경우
        {

            if (P1_Component.readyToBegin)
            {
                P1_Component.CmdChangeReadyState(false);
                Player1_Ready.gameObject.SetActive(false);
                isReady_P1 = false;
            }
            else
            {
                P1_Component.CmdChangeReadyState(true);
                Player1_Ready.gameObject.SetActive(true);
                isReady_P1 = true;
            }


            if (P2_Component.readyToBegin)
            {
                P2_Component.CmdChangeReadyState(false);
                Player2_Ready.gameObject.SetActive(false);
                isReady_P2 = false;
            }
            else
            {
                P2_Component.CmdChangeReadyState(true);
                Player2_Ready.gameObject.SetActive(true);
                isReady_P2 = true;
            }


            Rpc_ReadyClicked();
        }
    }




    [ClientRpc]     //서버의 오브젝트에서 호출되어서 클라이언트의 오브젝트에서 수행

    public void Rpc_ReadyClicked()
    {
        Debug.Log("Button RPC");
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



    }




    //public void StartCheck()
    //{
    //    if (isReady_P1 && isReady_P2)
    //    {
    //        //게임시작  
    //    }
    //}



}
