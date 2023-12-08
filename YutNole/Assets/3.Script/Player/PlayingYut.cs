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
}

public class PlayingYut : MonoBehaviour
{
    /*
     * Yut에 따른 플레이어 이동 및 버튼 이동, 화살표 생성 및 화살표 눌렀을 시 플레이어 생성
        Trun인 position에 걸리면 Transform 배열을 바꿔주기
     */
    public Transform[] pos1;
    public Transform[] pos2;
    public Transform[] pos3;
    public Transform[] pos4;

    public Transform[] playerArray; // player가 해당하는 pos array
    public RectTransform[] yutButton; // 도, 개, 걸, 윷, 모, 빽도 버튼

    // UI에서 버튼 선택할 때 달라질 예정
    [SerializeField] private GameObject[] player; // 자신의 말
    // 캐릭터마다 붙어있는 버튼
    public GameObject[] charcterButton; // character
    public GameObject[] returnButton; // return

    public int currentIndex = 0; // Button 위치 시킬 기준 인덱스, player 포지션과 동일해야 함
    public int resultIndex = 0; // 버튼 위치할 인덱스
    public List<int> yutResultIndex = new List<int>(); // yut 결과에 대한 숫자, 이동 버튼 클릭 시 Remove, Nack이면 Add 안함
    private int[] yutArray = { 1, 2, 3, 4, 5, -1 }; // 도 개 걸 윷 모 빽도

    // 윷 결과 가져오기
    public Yut_Gacha yutGacha;
    public string yutResult;

    public GameObject goalButton; // goal button, resultIndex보다 클 때 SetActive(true)

    private void Awake()
    {
        yutGacha = FindObjectOfType<Yut_Gacha>();
        playerArray = pos1;
        
    }

    private void Start()
    {
        //player = new GameObject[4];
        //Debug.Log(GameManager.instance.isPlayer1);
        //if (GameManager.instance.isPlayer1)
        //{
        //    for (int i = 0; i < player.Length; i++)
        //    {
        //        player[i] = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(i).gameObject;
        //    }
        //}
        //else
        //{ // Player2
        //    for (int i = 0; i < player.Length; i++)
        //    {
        //        player[i] = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(i).gameObject;
        //    }
        //}
    }

    public void PlayingYutPlus()
    { // 윷 던지기 버튼 event
        yutResult = yutGacha.ThrowResult; // 윷 던진 결과

        if (!yutResult.Equals("Nack") && !(yutResult.Equals("Backdo") && currentIndex == 0))
        { // 낙이거나 현재 인덱스가 0이면서 빽도일 경우 앞으로 가지 않음
            YutState type = (YutState)Enum.Parse(typeof(YutState), yutResult);
            yutResultIndex.Add(yutArray[(int)type]); // yutResult에 따라 List에 이동할 만큼의 숫자 추가

            for (int i = 0; i < 4; i++)
            {
                if (GameManager.instance.playingPlayer[i])
                {
                    Debug.Log("Button?");
                    charcterButton[i].SetActive(true); // 플레이어 선택 버튼, 골인한 플레이어 오브젝트의 버튼은 활성화 X
                }
            }
        }
        // Nack일 때, 인덱스 0일 때 빽도일 때
    }
    
    public void YutButtonClick(string name)
    { // Canvas - YutObject - 도, 개, 걸, 윷, 모, 빽도 event
        for (int i = 0; i < 4; i++)
        {
            charcterButton[i].SetActive(false);
            returnButton[i].SetActive(false);
        }

        YutState yutName = (YutState)Enum.Parse(typeof(YutState), name); // 버튼에 따라 달라짐
        // GameObject yutObject = yutButton[(int)yutName].gameObject;

        yutResultIndex.Remove(yutArray[(int)yutName]); // 추가된 리스트 삭제
        currentIndex += yutArray[(int)yutName]; // 현재 인덱스 리스트 삭제한 값과 같도록 변경
        TurnPosition(playerArray, currentIndex); // 현재 위치 배열 변경

        PositionOut();
        if (goalButton.activeSelf)
        {
            goalButton.SetActive(false);
        }

        if (yutResultIndex.Count > 0)
        { // 리스트가 남았을 때
          // 골인하지 않은 캐릭터 전부 선택 활성화
            for (int i = 0; i < 4; i++)
            {
                if (!GameManager.instance.playingPlayer[i])
                {
                    charcterButton[i].SetActive(true);
                }
            }
        }
    }
    #region Goal Button
    public void GoalButtonClick()
    { // Goal Button Event ... Error 있음
        // Goal Count++
        resultIndex = playerArray.Length - 1;
        PositionOut();

        for (int i = 0; i < 4; i++)
        {
            charcterButton[i].SetActive(false);
            returnButton[i].SetActive(false);
        }

        /*if (Vector3.Distance(player.transform.position, playerArray[resultIndex - 1].position) <= 0.01f)
        { // move 끝났을 때
            goalButton.SetActive(false);
            player.SetActive(false);
        }*/
    }
    #endregion
    #region ButtonPosition
    private void PositionOut()
    { // Button Position out
        for (int i = 0; i < yutButton.Length; i++)
        {  // 윷 던질 때 마다 모든 버튼 Canvas 밖으로 배치
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
    }

    public void PositionIn()
    { // Button Position in
        // Character Button click 시 불러옴, list 최소 1개 이상
        YutState yutType = YutState.Backdo; // 초기화
        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            resultIndex = currentIndex + yutResultIndex[i]; // 버튼 배치할 위치
            if (yutResultIndex[i] != -1)
            {
                yutType = (YutState)(yutResultIndex[i] - 1);
            }
            if (resultIndex >= playerArray.Length)
            { // Goal
                goalButton.SetActive(true);
                continue;
            }
            else if (playerArray.Length > resultIndex)
            { // not Goal
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[resultIndex].transform.position);
                yutButton[(int)yutType].transform.position = screen; // 나온 윷에 맞는 버튼 포지션 설정
            }
        }
    }
    #endregion
    #region PlayerButton
    public void CharacterButtonClick(int playerNum)
    { // Canvas - CharacterButton event
        PositionIn();
        for (int i = 0; i < 4; i++)
        {
            returnButton[i].SetActive(true);
            charcterButton[i].SetActive(false);
        }
        // 어떤 말을 선택했는지 설정
        GameManager.instance.playerNum = 0;
    }

    public void ReturnButtonClick()
    { // Canvas - ReturnButton event
        goalButton.SetActive(false);
        PositionOut();
        for (int i = 0; i < 4; i++)
        {
            charcterButton[i].SetActive(true);
            returnButton[i].SetActive(false);
        }
    }
    #endregion
    public void TurnPosition(Transform[] pos, int num)
    { // Player 현재 위치한 Array 변경
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
            { // pos3 -> pos4, 8(22 위치)
                playerArray = pos4;
                currentIndex = 8;
            }
        }
        // Catch 당했을 때 playerArray = pos1로 변경
    }

}
