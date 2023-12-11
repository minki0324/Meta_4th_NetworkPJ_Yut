using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player_Nickname : NetworkBehaviour
{
    [SerializeField] private Text text1;
    [SerializeField] private Text text2;

    #region Unity Callback
    private void Start()
    {
        Set_Name();
    }
    #endregion

    #region Client
    [Client]
    private void Set_Name()
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
        StartCoroutine(Set_Name_Co(name));
    }
    #endregion

    private IEnumerator Set_Name_Co(string name)
    {
        yield return new WaitForSeconds(0.5f);

        if(GM.instance.Player_Num == Player_Num.P1)
        {
            if(isLocalPlayer)
            {
                text1.text = name;
            }
            else
            {
                text2.text = name;
            }
        }
        else
        {
            if (isLocalPlayer)
            {
                text2.text = name;
            }
            else
            {
                text1.text = name;
            }
        }
    }
}
