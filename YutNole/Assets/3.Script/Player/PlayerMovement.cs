using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayingYut playingYut;

    public Transform startPos; // 임시 start

    public Transform[] playerArray; // player가 해당하는 pos array
    public int currentIndex = 0; // player 현재 index
    public int targetIndex = 0; // 버튼 클릭 시 이동할 index
    public Transform targetPos; // 버튼 클릭 시 이동할 위치

    public float speed = 2f;

    private void Awake()
    {
        playingYut = FindObjectOfType<PlayingYut>();
        // UI Player 선택했을 때로 나중에 이동
        playerArray = playingYut.pos1;
        playingYut.playerArray = playerArray;
    }

    public void PlayerMove()
    {
        transform.position = playerArray[currentIndex].position;
        targetIndex = playingYut.currentIndex + 1;
        targetPos = playerArray[targetIndex];
        StartCoroutine(Move_Co());
        currentIndex += targetIndex;

        // 버튼 누른 후
        switch (currentIndex)
        { // 현재 위치 확인 후 배열 바꿔주기
            case 5:
            case 10:
            case 22:
                playingYut.TurnPosition(playerArray, currentIndex);
                playerArray = playingYut.playerArray;
                playingYut.currentIndex = this.currentIndex;
                break;
        }
    }

    private IEnumerator Move_Co()
    {
        for (int i = currentIndex; i < targetIndex; i++)
        {
            targetPos = playerArray[i];
            while (transform.position != targetPos.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * speed);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
