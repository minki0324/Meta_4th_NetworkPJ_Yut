using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player_Control : NetworkBehaviour
{
    [SerializeField] private Button Yut_Btn;

    #region SyncVar
    #endregion
    #region Client
    #endregion
    #region Command
    #endregion
    #region ClientRPC
    #endregion
    #region Unity Callback
    private void Update()
    {
        if((int)GM.instance.Player_Num != Server_Manager.instance.Turn_Index)
        {
            Yut_Btn.interactable = false;
        }
        else
        {
            Yut_Btn.interactable = true;
        }
    }
    #endregion
}
