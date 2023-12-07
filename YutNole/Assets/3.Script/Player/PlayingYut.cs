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
    Nack
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

    [SerializeField] private GameObject player; // �ڽ��� ��
    public GameObject[] playerButton; // character, return
    
    public int currentIndex = 0; // Button �ε���
    public int resultIndex = 0; // ��ư ��ġ�� �ε���
    public List<int> yutResultIndex = new List<int>(); // yut ����� ���� ����, �̵� ��ư Ŭ�� �� Remove, Nack�̸� Add ����
    private int[] yutArray = { 1, 2, 3, 4, 5, -1, 0 }; // �� �� �� �� �� ���� ��

    // �� ��� ��������
    public Yut_Gacha_Test yutGacha; // ���߿� Yut_Gacha�� �ٲ��ֱ�
    public string yutResult;
    public YutState type;

    public GameObject goalButton; // goal button, resultIndex���� Ŭ ��

    [SerializeField] private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();    //���߿� �ٸ�������� �����ϱ�
        yutGacha = FindObjectOfType<Yut_Gacha_Test>(); // ���߿� Yut_Gacha�� �ٲ��ֱ�
        playerArray = pos1;
    }
    
    public void PlayingYutPlus()
    { // �� ������ ��ư Ŭ�� ��
        yutResult = yutGacha.ThrowResult; // �� ���� ���
        type = (YutState)Enum.Parse(typeof(YutState), yutResult);

        if (!yutResult.Equals("Nack") && !(yutResult.Equals("Backdo") && currentIndex == 0))
        {
            yutResultIndex.Add(yutArray[(int)type]); // yutResult�� ���� List�� �̵��� ��ŭ�� ���� �߰�
            playerButton[0].SetActive(true);
        }

        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            Debug.Log($"YutResultIndex{i} : {yutResultIndex[i]}");
        }
    }

    private void YutButtonPosition()
    {
        // ��ư Ȱ��ȭ �� ��ġ ����
        playerButton[0].gameObject.SetActive(true); // ĳ���� ���� ��ư Ȱ��ȭ
        for (int i = 0; i < yutResultIndex.Count; i++)
        {  // �� ���� �� ���� ��� ��ư Canvas ������
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
        if (yutResult != "Nack")
        {
            // ���� �ƴ� ��
            if (yutResult == "BackDo" && currentIndex == 0)
            {
                // �̶��� ������ �Ұ���
                return;
            }
            else
            {
                PositionIn();
            }
        }
        else
        { // Nack
            PositionIn();
        }
    }

    public void CharacterButtonClick()
    { // Canvas - CharacterButton
        YutButtonPosition();
        playerButton[1].SetActive(true);
        playerButton[0].SetActive(false);
    }

    public void ReturnButtonClick()
    { // Canvas - ReturnButton
        playerButton[0].SetActive(true);
        playerButton[1].SetActive(false);
        PositionOut();
    }

    public void YutButtonClick(string name)
    { // Canvas - YutObject - ���������𻪵�
        playerButton[0].SetActive(false);
        playerButton[1].SetActive(false);

        YutState yutName = (YutState)Enum.Parse(typeof(YutState), name);
        GameObject btn = yutButton[(int)yutName].gameObject;
        Vector3 screen = Camera.main.WorldToScreenPoint(btn.transform.parent.position); // Canvas ������
        btn.transform.position = screen; // ���� ���� �´� ��ư ������ ����

        yutResultIndex.Remove(yutArray[(int)yutName]); // ����Ʈ ����

        Debug.Log($"YutResultIndex Remove : {yutArray[(int)yutName]}");

        currentIndex += yutArray[(int)yutName]; // ���� �ε��� ����
        TurnPosition(playerArray, currentIndex); // ���� ��ġ �迭 ����
        PositionOut();
        goalButton.SetActive(false);

        if (yutResultIndex.Count > 0)
        {
            PositionIn();
        }
    }

    public void GoalButtonClick()
    {
        Debug.Log("������������������������������");
        // Goal Count++
        PositionOut();
        playerButton[0].SetActive(false);
       playerButton[1].SetActive(false);
        StartCoroutine(playerMovement.Move_Co());
        goalButton.SetActive(false);
        // StartCoroutine(Goal_Co());
    }

    private IEnumerator Goal_Co()
    {
        yield return new WaitForSeconds(0.2f);
        player.SetActive(false);
        goalButton.SetActive(false);
    }

    private void PositionOut()
    { // Button Position out
        for (int i = 0; i < yutButton.Length; i++)
        {  // �� ���� �� ���� ��� ��ư Canvas ������
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
        goalButton.SetActive(false);
    }

    public void PositionIn()
    { // Button Position in
        YutState yutType = YutState.Backdo;
        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            resultIndex = currentIndex + yutResultIndex[i];
            
            if (!yutResult.Equals("Nack"))
            {
                if (yutResultIndex[i] == -1)
                {
                    yutType = YutState.Backdo; // 5
                }
                else
                {
                    yutType = (YutState)(yutResultIndex[i] - 1);
                }
            }
            Debug.Log("YutType: " + yutType);
            if (playerArray.Length > resultIndex)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[resultIndex].transform.position);
                yutButton[(int)yutType].transform.position = screen; // ���� ���� �´� ��ư ������ ����, Backdo = 5
            } 
            else if (playerArray.Length == resultIndex)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[0].transform.position);
                yutButton[(int)yutType].transform.position = screen;
                goalButton.SetActive(true);
            }
            else
            {
                goalButton.SetActive(true);
            }
        }
    }

    public void TurnPosition(Transform[] pos, int num)
    {
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
}
