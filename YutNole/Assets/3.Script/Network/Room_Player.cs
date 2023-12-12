using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class Room_Player : NetworkRoomPlayer
{
    [SerializeField] private Text P1_Name;
    [SerializeField] private Text P2_Name;

    public override void OnGUI()
    {

    }

    #region Unity Callback
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Jaeyun") return;
        Call_Name();
    }
    #endregion

    #region Client
    [Client]
    public void Call_Name()
    {
        string name = SQLManager.instance.info.User_name;
        CMD_Set_Name(name);
    }
    #endregion
    #region Command
    [Command]
    private void CMD_Set_Name(string name)
    {
        RPC_Set_Name(name);
    }
    #endregion
    #region ClientRPC
    [ClientRpc]
    private void RPC_Set_Name(string name)
    {
        P1_Name = GameObject.FindGameObjectWithTag("Player1").GetComponent<Text>();
        P2_Name = GameObject.FindGameObjectWithTag("Player2").GetComponent<Text>();

        if(index == 0)
        {
            if (index == 0)
            {
                P1_Name.text = name;
                P2_Name.text = "";
            }
        }
        else if (index == 1)
        {
            if (index == 0)
            {
                P1_Name.text = name;
            }
            else if (index == 1)
            {
                P2_Name.text = name;
            }
        }
    }
    #endregion
    #region Client
    #endregion

}
