using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayingYut playingYut;

    public Transform[] playerArray; // player�� �ش��ϴ� pos array
    public int currentIndex = 0; // player ���� index, ��ư Ŭ�� �� �̵��� ������ �ٲ�
    public int targetIndex = 0; // ��ư Ŭ�� �� �̵��� index
    public Transform targetPos; // ��ư Ŭ�� �� �̵��� ��ġ

    public float speed = 2f;
    private int moveIndex = 0; // �̵� �ε���
    public bool isBackdo = false;
    public bool isGoal = false;

    private void Awake()
    {
        playingYut = FindObjectOfType<PlayingYut>();
        // UI Player �������� ���� ���߿� �̵�
        playerArray = playingYut.pos1;
        playingYut.playerArray = playerArray;
    }

    public void PlayerMove()
    { // �� �� �� �� �� ���� ��ư event
        transform.position = playerArray[currentIndex].position; // �÷��̾ ��ġ�� ������
        targetIndex = playingYut.currentIndex; // ��ư�� ������ �� �̵��� �÷��̾� Ÿ�� �ε���

        if (targetIndex - currentIndex == -1)
        { // Backdo
            isBackdo = true;
            moveIndex = currentIndex;
        }
        else if (targetIndex >= playerArray.Length)
        { // Goal 
            isGoal = true;
            moveIndex = playerArray.Length - 1;
        }
        else
        { // ����, ���� �̿�
            moveIndex = targetIndex;
        }

        StartCoroutine(Move_Co());
        
        //���� ������ ��ҿ� ������ �ִٸ�?
        //hasChance =true �����
        //���ٸ�? else
        //if(Yutindex.count > 0) 
        // playingYut.PlayingYutPlus();
        //else turn ���� 
        //������ ���� : ī��Ʈ 0 , ������ȸ X
    }

    private IEnumerator Move_Co()
    {
        GameManager.instance.isMoving = true;
        for (int i = currentIndex; i <= moveIndex; i++)
        {
            if (isBackdo)
            {
                targetPos = playerArray[i - 1];
            }
            else
            {
                targetPos = playerArray[i];
            }
            while (transform.position != targetPos.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * speed);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
        }
        GameManager.instance.isMoving = false;
        isBackdo = false;
        currentIndex = targetIndex;
        playerArray = playingYut.playerArray;

        foreach (PlayerState player in GameManager.instance.tempPlayers)
        {
            if (player.gameObject == gameObject) continue;
            if (Vector2.Distance(player.transform.position, gameObject.transform.position) < 0.01f)
            {
                if (player.tag == gameObject.tag)
                {
                    //����
                    Debug.Log("��");
                }
                else
                {
                    //���
                    Debug.Log("��");
                }


            }

        }

        if (playingYut.yutResultIndex.Count == 0 && !GameManager.instance.hasChance)
        {
            //������ ī��Ʈ�� ������ �ٽ�����
            Server_Manager.instance.CMD_Turn_Changer();
            playingYut.yutResultIndex.Clear();
            GameManager.instance.hasChance = true;

        }
        else if (playingYut.yutResultIndex.Count > 0)
        {
            playingYut.PlayingYutPlus();
        }

        //if (GameManager.instance.hasChance)
        //{ // ��, ��, ĳġ
        //    for (int i = 0; i < 4; i++)
        //    {
        //        playingYut.characterButton[i].SetActive(true);
        //    }
        //}
    }
}
