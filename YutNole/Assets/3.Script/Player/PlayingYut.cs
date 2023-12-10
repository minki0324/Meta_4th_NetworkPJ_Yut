using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum YutState
{
    Do = 0,
    Gae,
    Geol,
    Yut,
    Mo,
    Backdo,
}

public class PlayingYut : MonoBehaviour
{
    /*
     * Yut�� ���� �÷��̾� �̵� �� ��ư �̵�, ȭ��ǥ ���� �� ȭ��ǥ ������ �� �÷��̾� ����
        Trun�� position�� �ɸ��� Transform �迭�� �ٲ��ֱ�
     */
    public Transform[] pos1;
    public Transform[] pos2;
    public Transform[] pos3;
    public Transform[] pos4;

    public Transform[] playerArray; // player�� �ش��ϴ� pos array
    public RectTransform[] yutButton; // ��, ��, ��, ��, ��, ���� ��ư

    // UI���� ��ư ������ �� �޶��� ����
    [SerializeField] private GameObject[] player; // �ڽ��� ��
    // ĳ���͸��� �پ��ִ� ��ư
    public GameObject[] characterButton; // character
    public GameObject[] returnButton; // return

    public int currentIndex = 0; // Button ��ġ ��ų ���� �ε���, player �����ǰ� �����ؾ� ��
    public int resultIndex = 0; // ��ư ��ġ�� �ε���
    public List<int> yutResultIndex = new List<int>(); // yut ����� ���� ����, �̵� ��ư Ŭ�� �� Remove, Nack�̸� Add ����
    public List<string> yutResultString = new List<string>(); // yut ����� ���� ����, �̵� ��ư Ŭ�� �� Remove, Nack�̸� Add ����

    private int[] yutArray = { 1, 2, 3, 4, 5, -1 }; // �� �� �� �� �� ����
    private Result_Yut result;
    // �� ��� ��������
    public string yutResult;

    public GameObject goalButton; // goal button, resultIndex���� Ŭ �� SetActive(true)

    private void Awake()
    {
        for (int i = 0; i < result.result_Value.Count; i++)
        {
            yutResultString.Add(result.result_Value[i]);
        }
        playerArray = pos1;

        string temp = result.result_Value[0];
    }

    private void Start()
    {
        //������ ...
        //���� ĳ���͹�ư Ÿ�� ���⼭ ���������...
        //players �ε��� ������� startpos�� �ٽ� �����߾�
        StartCoroutine(SetButtons());
       
    }
    private void Update()
    {
      
    }
    public IEnumerator SetButtons()
    {
        yield return new WaitForSeconds(1.1f);
        for (int i = 0; i < characterButton.Length; i++)
        {
            characterButton[i].GetComponent<ButtonPositionSetter>().target = GameManager.instance.players[i].gameObject.transform;
            returnButton[i].GetComponent<ButtonPositionSetter>().target = GameManager.instance.players[i].gameObject.transform;
        }
    }
    public void PlayingYutPlus()
    { // �� ������ ��ư event

        if (!yutResult.Equals("Nack") && !(yutResult.Equals("Backdo") && currentIndex == 0))
        { // ���̰ų� ���� �ε����� 0�̸鼭 ������ ��� ������ ���� ����
            //YutState type = (YutState)Enum.Parse(typeof(YutState), yutResult);
            Debug.Log(yutResult);
            int intyut = ConvertToInt(yutResult);
            //yutResultIndex.Add(yutArray[intyut]); // yutResult�� ���� List�� �̵��� ��ŭ�� ���� �߰�
            
            Debug.Log(yutArray[intyut]);
            for (int i = 0; i < 4; i++)
            {
                if (GameManager.instance.players[i])
                {
                    Debug.Log("Button?");
                    characterButton[i].SetActive(true); // �÷��̾� ���� ��ư, ������ �÷��̾� ������Ʈ�� ��ư�� Ȱ��ȭ X
                }
            }
        }
        // Nack�� ��, �ε��� 0�� �� ������ ��
    }
    
    public void YutButtonClick(string name)
    { // Canvas - YutObject - ��, ��, ��, ��, ��, ���� event
        for (int i = 0; i < 4; i++)
        {
            characterButton[i].SetActive(false);
            returnButton[i].SetActive(false);
        }

        YutState yutName = (YutState)Enum.Parse(typeof(YutState), name); // ��ư�� ���� �޶���
        // GameObject yutObject = yutButton[(int)yutName].gameObject;

        //yutResultIndex.Remove(yutArray[(int)yutName]); // �߰��� ����Ʈ ����
        currentIndex += yutArray[(int)yutName]; // ���� �ε��� ����Ʈ ������ ���� ������ ����
        TurnPosition(playerArray, currentIndex); // ���� ��ġ �迭 ����

        PositionOut();
        if (goalButton.activeSelf)
        {
            goalButton.SetActive(false);
        }

        if (yutResultIndex.Count > 0)
        { // ����Ʈ�� ������ ��
          // �������� ���� ĳ���� ���� ���� Ȱ��ȭ
            for (int i = 0; i < 4; i++)
            {
                if (!GameManager.instance.playingPlayer[i])
                {
                    characterButton[i].SetActive(true);
                }
            }
        }
    }
    #region Goal Button
    public void GoalButtonClick()
    { // Goal Button Event ... Error ����
        // Goal Count++
        resultIndex = playerArray.Length - 1;
        PositionOut();

        for (int i = 0; i < 4; i++)
        {
            characterButton[i].SetActive(false);
            returnButton[i].SetActive(false);
        }

        /*if (Vector3.Distance(player.transform.position, playerArray[resultIndex - 1].position) <= 0.01f)
        { // move ������ ��
            goalButton.SetActive(false);
            player.SetActive(false);
        }*/
    }
    #endregion
    #region ButtonPosition
    private void PositionOut()
    { // Button Position out
        for (int i = 0; i < yutButton.Length; i++)
        {  // �� ���� �� ���� ��� ��ư Canvas ������ ��ġ
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
    }

    public void PositionIn()
    { // Button Position in
        // Character Button click �� �ҷ���, list �ּ� 1�� �̻�
        YutState yutType = YutState.Backdo; // �ʱ�ȭ
        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            resultIndex = currentIndex + yutResultIndex[i]; // ��ư ��ġ�� ��ġ
            if (yutResultIndex[i] != -1)
            {
                yutType = (YutState)(yutResultIndex[i] - 1);
            }
            if (resultIndex >= playerArray.Length)
            { // Goal
                goalButton.SetActive(true);
                continue;
            }
            else if (playerArray.Length > resultIndex)
            { // not Goal
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[resultIndex].transform.position);
                yutButton[(int)yutType].transform.position = screen; // ���� ���� �´� ��ư ������ ����
            }
        }
    }
    #endregion
    #region PlayerButton
    public void CharacterButtonClick(int playerNum)
    { // Canvas - CharacterButton event
        PositionIn();
        for (int i = 0; i < 4; i++)
        {
            returnButton[i].SetActive(true);
            characterButton[i].SetActive(false);
        }
        // � ���� �����ߴ��� ����
        GameManager.instance.playerNum = playerNum;
    }

    public void ReturnButtonClick()
    { // Canvas - ReturnButton event
        goalButton.SetActive(false);
        PositionOut();
        for (int i = 0; i < 4; i++)
        {
            characterButton[i].SetActive(true);
            returnButton[i].SetActive(false);
        }
    }
    #endregion
    public void TurnPosition(Transform[] pos, int num)
    { // Player ���� ��ġ�� Array ����
        if (pos == pos1)
        {
            if (num == 5)
            { // pos1 -> pos3, 5
                playerArray = pos3;
                currentIndex = 5;
            } else if (num == 10)
            { // pos1 -> pos2, 10
                playerArray = pos2;
                currentIndex = 10;
            }
        } else if (pos == pos3)
        {
            if (num == 8)
            { // pos3 -> pos4, 8(22 ��ġ)
                playerArray = pos4;
                currentIndex = 8;
            }
        }
        // Catch ������ �� playerArray = pos1�� ����
    }
    public void MoveButton()
    {
        PlayerMovement SelectPlayer = GameManager.instance.players[GameManager.instance.playerNum].GetComponent<PlayerMovement>();
        SelectPlayer.PlayerMove();
       
    }
    private int ConvertToInt(string yut)
    {
        int a = 0;
        switch (yut) {
            case "Backdo":
                a = -1;
                break;
            case "Do":
                a = 1;
               
                break;
            case "Gae":
                a = 2;
                break;
            case "Geol":
                a = 3;
                break;
            case "Yut":
                a = 4;
                break;
            case "Mo":
                a = 5;
                break;

        }
        return a;
    }
}
