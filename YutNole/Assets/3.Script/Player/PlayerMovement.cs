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

    public void PlayerMove()
    { // 도 개 걸 윷 모 빽도 버튼 event
        transform.position = playerArray[currentIndex].position; // 플레이어가 위치할 포지션
        targetIndex = playingYut.currentIndex; // 버튼을 눌렀을 때 이동할 플레이어 타겟 인덱스

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

        isBackdo = false;
        currentIndex = targetIndex;
        playerArray = playingYut.playerArray;
    }

    private IEnumerator Move_Co()
    {
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

        if (GameManager.instance.hasChance)
        { // 윷, 모, 캐치
            for (int i = 0; i < 4; i++)
            {
                playingYut.charcterButton[i].SetActive(true);
            }
        }
    }
}
