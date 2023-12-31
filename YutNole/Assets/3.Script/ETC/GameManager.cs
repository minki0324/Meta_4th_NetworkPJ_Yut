using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private PlayingYut playingYut;

    [SerializeField] private WINnLose winNlose;

    public GameObject[] myObject; //0번 힐라 1번 매그
    public Transform[] startPos; // 0123 -> P1 돌위치 / 4567 -> P2 돌위치

    public List<int> PlayerIndex;   //말판위에 올라간 유닛
    public PlayerState[] players;   //말판위에 올라간 유닛
    public PlayerState[] tempPlayers;

    public bool isPlayer1 = true;  //턴구분 변수
    public bool isMoving;
    public bool isMyTurn;
    public bool isThrew = false;


    public bool hasChance = true; // 윷, 모, 잡기일 때 찬스 한 번 더

    public int GoalCount = 0;
    public bool isWin = false;
    public bool isLose = false;
    public int playerNum; // 어떤 player가 선택되었는지 저장하는 변수, CharacterButton

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        winNlose = FindObjectOfType<WINnLose>();
        playingYut = FindObjectOfType<PlayingYut>();
        isPlayer1 = true;
    }
    private void Start()
    {
        players = new PlayerState[4];
        StartCoroutine(GetPlayer());
        hasChance = true;
    }

    private IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(1f);
        tempPlayers = FindObjectsOfType<PlayerState>();
        tempPlayers = tempPlayers.OrderBy(player => player.gameObject.CompareTag("Player2") ? 0 : 1).ToArray();
        for (int i = 0; i < tempPlayers.Length; i++)
        {
            tempPlayers[i].startPos = startPos[7-i];
        }
        
        int index = 0;
        foreach (PlayerState player in tempPlayers)
        {
            if (GM.instance.Player_Num == Player_Num.P1)
            {
                if (player.gameObject.CompareTag("Player1"))
                {
                    players[3 - index] = player;
                    //players[3 - index].startPos = startPos[3 - index];
                    index++;
                }
            }
            else if (GM.instance.Player_Num == Player_Num.P2)
            {
                if (player.gameObject.CompareTag("Player2"))
                {
                    players[3 - index] = player;
                    //players[3 - index].startPos = startPos[7 - index];
                    index++;
                }
            }
        }
        playingYut.SetButtons();
    }

    public void PlayerTurnChange()
    { // Player Turn Change
        if (playingYut.yutResultIndex.Count == 0 && !hasChance)
        {
            // 카운트가 없고, 찬스가 없을 때 턴 넘겨주고 비워줌
            Server_Manager.instance.CMD_Turn_Changer();
            playingYut.yutResultIndex.Clear();
            hasChance = true;
        }
        else if(playingYut.yutResultIndex.Count > 0)
        { // 리스트가 남아있으면 캐릭터 버튼 다시 활성화
            playingYut.PlayingYutPlus();
        }
    }

    public int PlayingCount()
    { // 판에 있는 말의 개수
        int zeroPlayer = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isPlaying)
            { // 말 판에 나가있는 플레이어 말의 개수
                zeroPlayer++;
            }
        }
        Debug.Log(zeroPlayer);
        return zeroPlayer;
    }
}