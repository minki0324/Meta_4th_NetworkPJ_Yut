using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerState : NetworkBehaviour
{
    private PlayingYut playingYut;

    // Player�� ��ġ
    public int playerNum = 0; // �÷��̾��� ó�� ��ġ
    public bool isPlaying = false; // ��� ���°� �ƴ� �ǿ� �����ִ���
    public bool isGoal = false; // ���� �ߴ��� �ƴ���
    public Transform[] currentArray; // �ڽ��� ���� ��ġ�� �迭

    public int currentIndex = 0; // ���� ��ġ�� �ε���
    public Transform startPos;
    // public Player_Num myNum;

    // Player Button
    public GameObject characterButton = null;
    public GameObject returnButton = null;

    // Player NumImage
    public GameObject[] numImage; // numberImage GameObject �������ֱ�

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("LocalPlayer");
    }

    #region Unity Callback
    private void Start()
    {
        SetUp();
        OnStart();
    }

    private void SetUp()
    { // �÷��̾� ���� ó�� ����
        playingYut = FindObjectOfType<PlayingYut>();
        currentArray = playingYut.pos1;
    }

    #endregion
    #region SyncVar
    #endregion
    #region Client
    [Client]
    private IEnumerator OnStart()
    {
        yield return new WaitForSeconds(1.5f);
        ButtonSetting();
    }
    #endregion
    #region Command
    [Command]
    private void ButtonSetting()
    {
        PlayerButtonSetting();
    }
    #endregion
    #region ClientRPC
    [ClientRpc] // �־ȵǴ�... 12. 10 AM 03:20
    private void PlayerButtonSetting()
    { // Button Position Setting
        int index = int.Parse(startPos.gameObject.name);
        characterButton = playingYut.characterButton[index];
        returnButton = playingYut.returnButton[index];

        characterButton.GetComponent<ButtonPositionSetter>().target = gameObject.transform;
        returnButton.GetComponent<ButtonPositionSetter>().target = gameObject.transform;

        characterButton.SetActive(false);
        returnButton.SetActive(false);
    }
    #endregion
    #region Hook Method, �ٸ� Ŭ���̾�Ʈ�� �˾ƾ� ��
    private void PlayerStateTrans(Transform[] _old, Transform[] _new)
    {
        currentArray = _new;
    }
    #endregion
}