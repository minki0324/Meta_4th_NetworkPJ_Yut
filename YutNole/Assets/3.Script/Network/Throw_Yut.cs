using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Throw_Yut : NetworkBehaviour
{
    /*
        1. 윷 던지기 버튼을 누르면 커맨드에서 결과값 만들기
        2. 나온 결과값을 RPC를 통해 모든 클라이언트에게 같은 애니메이션 출력
    */
    private PlayingYut playingYut;
    [SerializeField] private NetworkAnimator Yut_ani;
    [SerializeField] private Result_Yut result;

    #region Unity Callback
    private void Start()
    {
        playingYut = FindObjectOfType<PlayingYut>();
        for (int i = 0; i < playingYut.yutButton.Length; i++)
        {
            int index = i;
            playingYut.yutButton[i].gameObject.GetComponent<Button>().onClick.AddListener(() => Yut_Btn_Click(index));
        }
    }
    #endregion

    #region SyncVar
    [SyncVar(hook = nameof(TriggerChange))] // 윷놀이 결과값
    private string trigger_ = string.Empty;
    #endregion

    #region Client
    [Client] // 버튼 눌렀을 때 클라이언트 입장에서 서버에게 버튼 눌렸다고 호출해주는 메소드
    public void Btn_Click()
    {
        GameManager.instance.hasChance = false;
        CMDYut_Throwing();
        //Server_Manager.instance.CMD_Turn_Changer();
    }


    [Client]
    public void Yut_Btn_Click(int name)
    {
        CMDYut_Button_Click(name);
    }

    public void ThrowYutResult(string trigger_)
    {
        int index = 0;
        playingYut.yutResult = trigger_;
        switch (trigger_)
        {
            case "Do":
                index = 0;
                break;
            case "Gae":
                index = 1;
                break;
            case "Geol":
                index = 2;
                break;
            case "Yut":
                index = 3;
                GameManager.instance.hasChance = true;
                break;
            case "Mo":
                index = 4;
                GameManager.instance.hasChance = true;
                break;
            case "Backdo":
                index = 5;
                break;
        }
        //내턴이 아닐때 && 낙이 나왔을때 && 판에 내말이없는경우 빽도가 나올때(추가)
        if ((int)GM.instance.Player_Num == Server_Manager.instance.Turn_Index && !trigger_.Equals("Nack"))
        {
            Addlist(index);
        }
       /* else if(playingYut.yutResultIndex.Count == 0 )
        {
            Server_Manager.instance.CMD_Turn_Changer();
            playingYut.yutResultIndex.Clear();
            GameManager.instance.hasChance = true;
        }*/

       



    }

    private void Addlist(int index)
    {

        playingYut.yutResultIndex.Add(index);
        playingYut.PlayingYutPlus();
    }


    #endregion

    #region Command
    [Command(requiresAuthority = false)] // 실질적인 윷놀이 결과값을 만들어내고 리스트에 저장 및 클라이언트들에게 뿌리는 RPC 메소드 호출
    private void CMDYut_Throwing()
    {
        string[] triggers = { "Backdo", "Backdo", "Backdo", "Do", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo" };
        trigger_ = triggers[Random.Range(0, triggers.Length)];
        // Result_Yut 클래스의 Set_Result 메소드 호출
        result.Set_Result(trigger_, true);
        RPCYut_Throwing(trigger_);
    }

    [Command(requiresAuthority = false)]
    private void CMDYut_Button_Click(int name)
    {
        string yutTrigger = string.Empty;
        switch (name)
        {
            case 0:
                yutTrigger = "Do";
                break;
            case 1:
                yutTrigger = "Gae";
                break;
            case 2:
                yutTrigger = "Geol";
                break;
            case 3:
                yutTrigger = "Yut";
                break;
            case 4:
                yutTrigger = "Mo";
                break;
            case 5:
                yutTrigger = "Backdo";
                break;
        }
        result.Set_Result(yutTrigger, false);
    }
    #endregion

    #region ClientRPC
    [ClientRpc] // 윷놀이 결과값에 대한 애니메이션만 출력해주는 메소드
    private void RPCYut_Throwing(string trigger)
    {
        Yut_ani.animator.SetTrigger(trigger);

        ThrowYutResult(trigger);
    }
    #endregion
    #region Hook Method
    private void TriggerChange(string _old, string _new)
    {
        trigger_ = _new;
    }
    #endregion
}
