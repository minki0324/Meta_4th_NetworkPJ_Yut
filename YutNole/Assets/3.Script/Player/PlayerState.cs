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

    #region Unity Callback
    private void Start()
    {
        SetUp();
        if (!isLocalPlayer) return;
        //StartCoroutine(PlayerButtonSetting());
    }

    private void SetUp()
    { // �÷��̾� ���� ó�� ����
        playingYut = FindObjectOfType<PlayingYut>();
        currentArray = playingYut.pos1;
    }

    //private IEnumerator PlayerButtonSetting()
    //{ // Button Position Setting
    //    yield return new WaitForSeconds(1.5f);
    //    int index = int.Parse(startPos.gameObject.name);
    //    Debug.Log(index);
    //    characterButton = playingYut.characterButton[index];
    //    returnButton = playingYut.returnButton[index];

    //    characterButton.GetComponent<ButtonPositionSetter>().target = gameObject.transform;
    //    returnButton.GetComponent<ButtonPositionSetter>().target = gameObject.transform;

    //    characterButton.SetActive(false);
    //    returnButton.SetActive(false);
    //}
    #endregion
    #region SyncVar
    #endregion
    #region Client
    #endregion
    #region Command
    #endregion
    #region ClientRPC
    #endregion
    #region Hook Method, �ٸ� Ŭ���̾�Ʈ�� �˾ƾ� ��
    private void PlayerStateTrans(Transform[] _old, Transform[] _new)
    {
        currentArray = _new;
    }
    #endregion
}