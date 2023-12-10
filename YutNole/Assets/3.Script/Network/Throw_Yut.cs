using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log("Btn_Click 호출됨");
        CMDYut_Throwing();

        Server_Manager.instance.CMD_Turn_Changer();
    }

    public void ThrowYutResult(string trigger_)
    {
        Debug.Log("ThrowYutResult");
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
                break;
            case "Mo":
                index = 4;
                break;
            case "Backdo":
                index = 5;
                break;
        }
        playingYut.yutResultIndex.Add(index);
        Debug.Log("Count: " + playingYut.yutResultIndex.Count);
    }
    #endregion

    #region Command
    [Command(requiresAuthority = false)] // 실질적인 윷놀이 결과값을 만들어내고 리스트에 저장 및 클라이언트들에게 뿌리는 RPC 메소드 호출
    private void CMDYut_Throwing()
    {
        string[] triggers = { "Do", "Do", "Do", "Backdo", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo", "Nack", "Nack" };
        trigger_ = triggers[Random.Range(0, triggers.Length)];
        Debug.Log($"CMDYut_Throwing 호출 : {trigger_}");
        // Result_Yut 클래스의 Set_Result 메소드 호출
        result.Set_Result(trigger_, true);
        RPCYut_Throwing(trigger_);
    }
    #endregion

    #region ClientRPC
    [ClientRpc] // 윷놀이 결과값에 대한 애니메이션만 출력해주는 메소드
    private void RPCYut_Throwing(string trigger)
    {
        Debug.Log("RpcRPCYut_Throwing 호출됨");
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
