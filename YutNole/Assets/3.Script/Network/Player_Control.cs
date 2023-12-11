using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player_Control : NetworkBehaviour
{
    [SerializeField] private Button Yut_Btn;

    [SerializeField] private Image Win_img;
    [SerializeField] private Image Lose_img;

    private string win = "Win";
    private string lose = "Lose";

    #region SyncVar
    [SyncVar(hook = nameof(Goal_CountRenew))]
    public int GoalCount;
    #endregion
    #region Client
    [Client]
    public void Goal_CountUp()
    {
        CMD_Count();
    }
    #endregion
    #region Command
    [Command]
    private void CMD_Count()
    {
        GoalCount++;
        if(GoalCount == 4)
        {
            Server_Manager.instance.Turn_Index = 4;

            Player_Control[] players = FindObjectsOfType<Player_Control>();
            foreach (var player in players)
            {
                if(player.GoalCount == 4)
                {
                    RPC_CountCal(win);
                }
                else
                {
                    RPC_CountCal(lose);
                }
            }

        }
    }
    #endregion
    #region ClientRPC
    [ClientRpc]
    private void RPC_CountCal(string result)
    {
        if(isLocalPlayer)
        {
            AudioManager.instance.PlaySFX("WinBgm");
            AudioManager.instance.PlaySFX("Win");
            Win_img.gameObject.SetActive(true);
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose");
            Lose_img.gameObject.SetActive(true);
        }
    }
    #endregion
    #region Unity Callback
    private void Update()
    {
        //내턴일때 && 찬스가 true일때 버튼활성화
        if ((int)GM.instance.Player_Num == Server_Manager.instance.Turn_Index && GameManager.instance.hasChance)
        {
            Yut_Btn.interactable = true;
        }
        else
        {
            
            Yut_Btn.interactable = false;
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            Goal_CountUp();
            Debug.Log(GoalCount);
        }
    }
    #endregion
    #region Hook Method
    public void Goal_CountRenew(int _old, int _new)
    {
        GoalCount = _new;
    }
    #endregion
}
