using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Player_Control player_Control;
    private PlayingYut playingYut;
    private PlayerState playerState;
    private Throw_Yut throw_Yut;
    public Transform[] playerArray; // player가 해당하는 pos array
    public int currentIndex = 0; // player 현재 index, 버튼 클릭 후 이동할 때마다 바뀜
    public int targetIndex = 0; // 버튼 클릭 시 이동할 index
    public Transform targetPos; // 버튼 클릭 시 이동할 위치

    public float speed = 4f;
    private int moveIndex = 0; // 이동 인덱스
    public bool isBackdo = false;

    private void Awake()
    {
        playingYut = FindObjectOfType<PlayingYut>();
        // UI Player 선택했을 때로 나중에 이동
        playerArray = playingYut.pos1;
        playingYut.playerArray = playerArray;
        playerState = GetComponent<PlayerState>();
        throw_Yut = FindObjectOfType<Throw_Yut>();
        player_Control = FindObjectOfType<Player_Control>();
    }

    public void PlayerMove(int index)
    { // 도 개 걸 윷 모 빽도 버튼 event
        GameManager.instance.players[GameManager.instance.playerNum].isPlaying = true; // 판 위에 올라감
        currentIndex = playerState.currentIndex;
        playerArray = playerState.currentArray;
        transform.position = playerArray[currentIndex].position; // 플레이어가 위치할 포지션
        targetIndex = index; // 버튼을 눌렀을 때 이동할 플레이어 타겟 인덱스

        if (targetIndex - currentIndex == -1)
        { // Backdo
            isBackdo = true;
            moveIndex = currentIndex;
        }
        else if (targetIndex >= playerArray.Length)
        { // Goal 
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
        for (int i = currentIndex; i <= moveIndex; i++)
        {
            if (isBackdo)
            {
                targetPos = playerArray[i - 1];
            }else if (playerState.isGoal)
            {
                targetPos = playerArray[0];
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
            yield return new WaitForSeconds(0.1f);
        }
        
        Transform[] pos = playingYut.playerArray;

        Debug.Log("isbackdo : " + isBackdo);
        Debug.Log("2번째 트루 ? : " + (playerArray == playingYut.pos2));
        Debug.Log("currentindex : " + currentIndex);
        if (isBackdo && playerState.currentArray == playingYut.pos1 && playerState.currentIndex == 1)
        { // player가 빽도로 골인 지점에 돌아왔을 때
            currentIndex = playingYut.pos4.Length - 1;
            playerArray = playingYut.pos4;
        }
        else if(isBackdo && playerArray == playingYut.pos3 && currentIndex == 5)
        {
            Debug.Log("들어옴 ?");
            pos = playingYut.pos1;
        }
        else if (isBackdo && playerArray == playingYut.pos2 && currentIndex == 10)
        {
            pos = playingYut.pos1;
        }
        else if (isBackdo && playerArray == playingYut.pos4 && currentIndex == 8)
        {
            pos = playingYut.pos3;
        }

        currentIndex = targetIndex;
        playerArray = pos;

        playingYut.currentIndex = currentIndex;
        playingYut.playerArray = playerArray;

        playingYut.player[GameManager.instance.playerNum].currentIndex = currentIndex;
        playingYut.player[GameManager.instance.playerNum].currentArray = playerArray;
        isBackdo = false;
        if (!playerState.isGoal)
        {
            foreach (PlayerState player in GameManager.instance.tempPlayers)
            {
                if (player.gameObject == gameObject) continue;
                if (!player.gameObject.activeSelf) continue;
                if (Vector2.Distance(player.transform.position, gameObject.transform.position) < 0.01f)
                {
                    if (player.tag == gameObject.tag)
                    {
                        Debug.Log("업");
                        Server_Manager.instance.Carry(playerState, player);
                        Debug.Log(player.carryPlayer.Count);
                    }
                    else
                    {
                        Debug.Log("잡");
                        Server_Manager.instance.Catch(playerState, player);
                        GameManager.instance.hasChance = true;
                    }
                }
            }
        }
        else
        {
            Debug.Log("골 이프문");
            player_Control.Goal_CountUp(); //본인 골인카운트
            gameObject.transform.position = playerState.startPos.transform.position;
            playerState.isPlaying = false; // 골인 시 판에서 빠짐
            if (playingYut.goalButton.activeSelf)
            {
                playingYut.goalButton.SetActive(false);
            }           
            for (int i = 0; i < playerState.carryPlayer.Count; i++) //업은애들 골인 카운트
            { // player Carry한 만큼
                Debug.Log("업은애들카운트" + i);
                player_Control.Goal_CountUp();
            }
            playerState.GoalInClick(playerState);
        }
        if (player_Control == null)
        {
            player_Control = FindObjectOfType<Player_Control>();
        }

        throw_Yut.Yut_Btn_Click(playingYut.removeIndex); // result panel remove
        GameManager.instance.PlayerTurnChange();
    }
}
