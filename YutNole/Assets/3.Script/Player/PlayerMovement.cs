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
        Time.timeScale = 3;
        playingYut = FindObjectOfType<PlayingYut>();
        // UI Player 선택했을 때로 나중에 이동
        playerArray = playingYut.pos1;
        playingYut.playerArray = playerArray;
    }

    public void PlayerMove()
    {
        transform.position = playerArray[currentIndex].position; // 플레이어 현재 포지션
        targetIndex = playingYut.currentIndex; // 버튼을 눌렀을 때 이동할 플레이어 타겟 인덱스
        if (targetIndex >= playerArray.Length) // 현재 인덱스가 target Index보다 작으면 Backdo 아니면 기본
        { // Goal
            targetIndex = playerArray.Length - 1;
        }
        targetPos = playerArray[targetIndex];

        playerArray = playingYut.playerArray;
        currentIndex = targetIndex;


        if (targetIndex >= playerArray.Length)
        {
            Debug.Log("일루 들어와죵..!");
            targetPos = playerArray[playerArray.Length - 1];
            playingYut.GoalButtonClick();
            
        }
        StartCoroutine(Move_Co());


    }

    public IEnumerator Move_Co()
    {
        if (targetIndex >= playerArray.Length)
        {
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);

            for (int i = currentIndex; i <= targetIndex; i++)
            {
                targetPos = playerArray[i];
                while (transform.position != targetPos.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * speed);
                    yield return null;
                }
                yield return new WaitForSeconds(0.2f);
            }
            //player.SetActive(false);
            //goalButton.SetActive(false);
        }
        else
        {
            if (currentIndex > targetIndex)
            { // Backdo
                targetIndex = currentIndex - targetIndex;
                Debug.Log("Backdoindex: " + targetIndex);
            }

            for (int i = currentIndex; i <= targetIndex; i++)
            {
                targetPos = playerArray[i];
                while (transform.position != targetPos.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * speed);
                    yield return null;
                }
                yield return new WaitForSeconds(0.2f);
            }

            if (playingYut.yutGacha.isChance)
            {
                playingYut.playerButton[0].SetActive(true);
            }
        }

    }


}
