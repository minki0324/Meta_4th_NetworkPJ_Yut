using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Test_Btn : NetworkBehaviour
{
    [SerializeField] private NetworkAnimator Yut_ani;
    [SerializeField] private Result_Panel result;

    #region SyncVar
    [SyncVar]
    private string trigger_ = string.Empty;
    #endregion

    #region Client
    [Client]
    public void Btn_Click()
    {
        Debug.Log("Btn_Click »£√‚µ ");
        CMDYut_Throwing();
        Server_Manager.instance.CMD_Turn_Changer();
    }

    [Command(requiresAuthority = false)]
    private void CMDYut_Throwing()
    {
        string[] triggers = { "Do", "Do", "Do", "Backdo", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo", "Nack", "Nack" };
        trigger_ = triggers[Random.Range(0, triggers.Length)];
        Debug.Log($"CMDYut_Throwing »£√‚ : {trigger_}");
        result.Set_Result(trigger_);
        RPCYut_Throwing(trigger_);
    }
    #endregion

    #region Server

    #endregion

    [ClientRpc]
    private void RPCYut_Throwing(string trigger)
    {
        Debug.Log("RpcRPCYut_Throwing »£√‚µ ");
        Yut_ani.animator.SetTrigger(trigger);
    }
}
