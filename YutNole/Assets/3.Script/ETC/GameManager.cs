using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

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

    public bool hasChance = false; // 윷, 모, 잡기일 때 찬스 한 번 더

    public int GoalCount = 0;

    public bool isWin = false;
    public bool isLose = false;

    public int playerNum ; // 어떤 player가 선택되었는지 저장하는 변수, CharacterButton
   
    public bool[] playingPlayer = { false, false, false, false }; // player 0, 1, 2, 3 판에 올라갔다면 true, 잡혔을 때, 골인했을 때는 false로 바꿔줌

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        winNlose = FindObjectOfType<WINnLose>();
        isPlayer1 = true;
    }
    private void Start()
    {
        players = new PlayerState[4];
        StartCoroutine(GetPlayer());
    }

    private IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(1f);
        tempPlayers = FindObjectsOfType<PlayerState>();
        int index = 0;
        foreach (PlayerState player in tempPlayers)
        {
            if (GM.instance.Player_Num == Player_Num.P1)
            {
                if (player.gameObject.CompareTag("Player1"))
                {
                    players[3-index] = player;
                    players[3-index].startPos = startPos[3-index];
                    index++;
                }
            }
            else if (GM.instance.Player_Num == Player_Num.P2)
            {
                if (player.gameObject.CompareTag("Player2"))
                {
                    players[3-index] = player;
                    players[3-index].startPos = startPos[3-index];
                    index++;
                }
            }
        }
    }
}
