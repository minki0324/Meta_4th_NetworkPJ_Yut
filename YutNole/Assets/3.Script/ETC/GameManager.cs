using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private WINnLose winNlose;
    [SerializeField] private Unit_Panel unitPanel;

    //말판 위 게임 유닛 
    [SerializeField] public GameObject[] P1_Units_Obj; 
    [SerializeField] public GameObject[] P2_Units_Obj;
    public GameObject[] myObject; //0번 힐라 1번 매그
    public Transform[] targetPos; // 0123 -> P1 돌위치 / 4567 -> P2 돌위치
    public List<int> PlayerIndex;   //말판위에 올라간 유닛
    public PlayerMovement[] players;   //말판위에 올라간 유닛
    public PlayerMovement[] tempplayers;
    public bool isPlayer1 = true;  //턴구분 변수
    
    public bool isMyTurn;
    public bool isThrew = false;

    public bool hasChance = false; // 윷, 모, 잡기일 때 찬스 한 번 더

    public int GoalCount = 0;
    public bool isWin = false;
    public bool isLose = false;

    public int playerNum = 0; // 어떤 player가 선택되었는지 저장하는 변수, CharacterButton
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
        unitPanel = FindObjectOfType<Unit_Panel>();

        isPlayer1 = true;

        //for(int i = 0; i<P1_Units_Obj.Length; i++)
        //{
        //    P1_Units_Obj[i].SetActive(false);
        //}

    }
    private void Start()
    {
        players = new PlayerMovement[4];
        StartCoroutine(GetPlayer());
        
    }
    private IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(1f);
      tempplayers = FindObjectsOfType<PlayerMovement>();
        int index = 0;
        foreach (PlayerMovement player in tempplayers)
        {
           
            if (GM.instance.Player_Num == Player_Num.P1)
            {
                if (player.gameObject.CompareTag("Player1"))
                {
                    
                    players[index] = player;
                    index++;
                }
            }
            else if(GM.instance.Player_Num == Player_Num.P2)
            {
                if (player.gameObject.CompareTag("Player2"))
                {

                    players[index] = player;
                    index++;
                }
            }
        }

        Debug.Log(players[1].gameObject.name);
    }


    //캐릭터가 Goal 지점에 도착할때 호출해줘 :)
    public void Count_GoalUnit(GameObject unit)
    {

        if(isPlayer1)
        {
            for (int i = 0; i < unitPanel.P1_Units.Count; i++)
            {
                if (unit.name == unitPanel.P1_Units[i].name)
                {
                    unitPanel.P1_Units[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }

        }   
        else
        {
            for (int i = 0; i < unitPanel.P2_Units.Count; i++)
            {
                if (unit.name == unitPanel.P2_Units[i].name)
                {
                    unitPanel.P2_Units[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
     

        GoalCount++;

        if(GoalCount >= 4)
        {
            isWin = true;
            winNlose.Play_ImgAnimation();
        }
        
    }

 



}
