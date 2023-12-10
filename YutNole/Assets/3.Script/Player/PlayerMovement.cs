using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayingYut playingYut;

    public Transform[] playerArray; // player가 해당하는 pos array
    public int currentIndex = 0; // player 현재 index, 버튼 클릭 후 이동할 때마다 바뀜
    public int targetIndex = 0; // 버튼 클릭 시 이동할 index
    public Transform targetPos; // 버튼 클릭 시 이동할 위치

    public float speed = 2f;
    private int moveIndex = 0; // 이동 인덱스
    public bool isBackdo = false;
    public bool isGoal = false;

    private void Awake()
    {
        playingYut = FindObjectOfType<PlayingYut>();
        // UI Player 선택했을 때로 나중에 이동
        playerArray = playingYut.pos1;
        playingYut.playerArray = playerArray;
    }

    public void PlayerMove(int index)
    { // 도 개 걸 윷 모 빽도 버튼 event
        transform.position = playerArray[currentIndex].position; // 플레이어가 위치할 포지션
        targetIndex = index; // 버튼을 눌렀을 때 이동할 플레이어 타겟 인덱스

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
        { // 골인, 빽도 이외
            moveIndex = targetIndex;
        }

        StartCoroutine(Move_Co());
        
        //만약 도착한 장소에 적팀이 있다면?
        //hasChance =true 줘야함
        //없다면? else
        //if(Yutindex.count > 0) 
        // playingYut.PlayingYutPlus();
        //else turn 종료 
        //턴종료 조건 : 카운트 0 , 던질기회 X
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
        playingYut.player[GameManager.instance.playerNum].currentIndex = currentIndex;
        playingYut.player[GameManager.instance.playerNum].currentArray = playerArray;

        foreach (PlayerState player in GameManager.instance.tempPlayers)
        {
            if (player.gameObject == gameObject) continue;
            if (player.transform == gameObject.transform)
            {
                if (player.tag == gameObject.tag)
                {
                    //업기
                    Debug.Log("업");
                }
                else
                {
                    Debug.Log("잡");
                    //잡기
                }


            }

        }

        if (playingYut.yutResultIndex.Count == 0 && !GameManager.instance.hasChance)
        {
            //움직일 카운트가 남으면 다시진행
            Server_Manager.instance.CMD_Turn_Changer();
            playingYut.yutResultIndex.Clear();
            GameManager.instance.hasChance = true;

        }
        else if (playingYut.yutResultIndex.Count > 0)
        {
            playingYut.PlayingYutPlus();
        }

        //if (GameManager.instance.hasChance)
        //{ // 윷, 모, 캐치
        //    for (int i = 0; i < 4; i++)
        //    {
        //        playingYut.characterButton[i].SetActive(true);
        //    }
        //}
    }
}
