using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Throw_Yut : NetworkBehaviour
{
    /*
        1. �� ������ ��ư�� ������ Ŀ�ǵ忡�� ����� �����
        2. ���� ������� RPC�� ���� ��� Ŭ���̾�Ʈ���� ���� �ִϸ��̼� ���
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
    [SyncVar] // ������ �����
    private string trigger_ = string.Empty;
    #endregion

    #region Client
    [Client] // ��ư ������ �� Ŭ���̾�Ʈ ���忡�� �������� ��ư ���ȴٰ� ȣ�����ִ� �޼ҵ�
    public void Btn_Click()
    {
        Debug.Log("Btn_Click ȣ���");
        CMDYut_Throwing();
        Server_Manager.instance.CMD_Turn_Changer();
        ThrowYutResult(trigger_);
        playingYut.PlayingYutPlus();
    }
    [Client]
    public void ThrowYutResult(string trigger_)
    {
        playingYut.yutResult = trigger_;
    }
    #endregion

    #region Command
    [Command(requiresAuthority = false)] // �������� ������ ������� ������ ����Ʈ�� ���� �� Ŭ���̾�Ʈ�鿡�� �Ѹ��� RPC �޼ҵ� ȣ��
    private void CMDYut_Throwing()
    {
        string[] triggers = { "Do", "Do", "Do", "Backdo", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo", "Nack", "Nack" };
        trigger_ = triggers[Random.Range(0, triggers.Length)];
        Debug.Log($"CMDYut_Throwing ȣ�� : {trigger_}");
        // Result_Yut Ŭ������ Set_Result �޼ҵ� ȣ��
        result.Set_Result(trigger_, true);
        
        RPCYut_Throwing(trigger_);


    }
    #endregion

    #region ClientRPC
    [ClientRpc] // ������ ������� ���� �ִϸ��̼Ǹ� ������ִ� �޼ҵ�
    private void RPCYut_Throwing(string trigger)
    {
        Debug.Log("RpcRPCYut_Throwing ȣ���");
        Yut_ani.animator.SetTrigger(trigger);


    }
    #endregion
    #region ClientRPC
    //[TargetRpc]
    //private void moveturn(NetworkConnection target)
    //{
    //    if(target.connectionId )
    //    target.connectionId
    //}
    #endregion
    
}
