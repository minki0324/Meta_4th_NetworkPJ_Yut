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
    Nack
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

    [SerializeField] private GameObject player; // 자신의 말
    public GameObject[] playerButton; // character, return
    
    public int currentIndex = 0; // Button 인덱스
    public int resultIndex = 0; // 버튼 위치할 인덱스
    public List<int> yutResultIndex = new List<int>(); // yut 결과에 대한 숫자, 이동 버튼 클릭 시 Remove, Nack이면 Add 안함
    private int[] yutArray = { 1, 2, 3, 4, 5, -1, 0 }; // 도 개 걸 윷 모 빽도 낙

    // 윷 결과 가져오기
    public Yut_Gacha_Test yutGacha; // 나중에 Yut_Gacha로 바꿔주기
    public string yutResult;
    public YutState type;

    public GameObject goalButton; // goal button, resultIndex보다 클 때

    private void Awake()
    {
        yutGacha = FindObjectOfType<Yut_Gacha_Test>(); // 나중에 Yut_Gacha로 바꿔주기
        playerArray = pos1;
    }
    
    public void PlayingYutPlus()
    { // 윷 던지기 버튼 클릭 시
        yutResult = yutGacha.ThrowResult; // 윷 던진 결과
        type = (YutState)Enum.Parse(typeof(YutState), yutResult);

        if (!yutResult.Equals("Nack") && !(yutResult.Equals("Backdo") && currentIndex == 0))
        {
            yutResultIndex.Add(yutArray[(int)type]); // yutResult에 따라 List에 이동할 만큼의 숫자 추가
            playerButton[0].SetActive(true);
        }

        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            Debug.Log($"YutResultIndex{i} : {yutResultIndex[i]}");
        }
    }

    private void YutButtonPosition()
    {
        // 버튼 활성화 및 위치 설정
        playerButton[0].gameObject.SetActive(true); // 캐릭터 선택 버튼 활성화
        for (int i = 0; i < yutResultIndex.Count; i++)
        {  // 윷 던질 때 마다 모든 버튼 Canvas 밖으로
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
        if (yutResult != "Nack")
        {
            // 낙이 아닐 때
            if (yutResult == "BackDo" && currentIndex == 0)
            {
                // 이때는 빽도가 불가능
                return;
            }
            else
            {
                PositionIn();
            }
        }
        else
        { // Nack
            PositionIn();
        }
    }

    public void CharacterButtonClick()
    { // Canvas - CharacterButton
        YutButtonPosition();
        playerButton[1].SetActive(true);
        playerButton[0].SetActive(false);
    }

    public void ReturnButtonClick()
    { // Canvas - ReturnButton
        playerButton[0].SetActive(true);
        playerButton[1].SetActive(false);
        PositionOut();
    }

    public void YutButtonClick(string name)
    { // Canvas - YutObject - 도개걸윷모빽도
        playerButton[0].SetActive(false);
        playerButton[1].SetActive(false);

        YutState yutName = (YutState)Enum.Parse(typeof(YutState), name);
        GameObject btn = yutButton[(int)yutName].gameObject;
        Vector3 screen = Camera.main.WorldToScreenPoint(btn.transform.parent.position); // Canvas 밖으로
        btn.transform.position = screen; // 나온 윷에 맞는 버튼 포지션 설정

        yutResultIndex.Remove(yutArray[(int)yutName]); // 리스트 삭제

        Debug.Log($"YutResultIndex Remove : {yutArray[(int)yutName]}");

        currentIndex += yutArray[(int)yutName]; // 현재 인덱스 변경
        TurnPosition(playerArray, currentIndex); // 현재 위치 배열 변경
        PositionOut();
        goalButton.SetActive(false);

        if (yutResultIndex.Count > 0)
        {
            PositionIn();
        }
    }

    public void GoalButtonClick()
    {
        // Goal Count++
        PositionOut();
        playerButton[0].SetActive(false);
        playerButton[1].SetActive(false);
        StartCoroutine(Goal_Co());
    }

    private IEnumerator Goal_Co()
    {
        yield return new WaitForSeconds(0.2f);
        player.SetActive(false);
        goalButton.SetActive(false);
    }

    private void PositionOut()
    { // Button Position out
        for (int i = 0; i < yutButton.Length; i++)
        {  // 윷 던질 때 마다 모든 버튼 Canvas 밖으로
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
        goalButton.SetActive(false);
    }

    public void PositionIn()
    { // Button Position in
        YutState yutType = YutState.Backdo;
        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            resultIndex = currentIndex + yutResultIndex[i];
            
            if (!yutResult.Equals("Nack"))
            {
                if (yutResultIndex[i] == -1)
                {
                    yutType = YutState.Backdo; // 5
                }
                else
                {
                    yutType = (YutState)(yutResultIndex[i] - 1);
                }
            }
            Debug.Log("YutType: " + yutType);
            if (playerArray.Length > resultIndex)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[resultIndex].transform.position);
                yutButton[(int)yutType].transform.position = screen; // 나온 윷에 맞는 버튼 포지션 설정, Backdo = 5
            } else if (playerArray.Length == resultIndex)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[0].transform.position);
                yutButton[(int)yutType].transform.position = screen;
                goalButton.SetActive(true);
            }
            else
            {
                goalButton.SetActive(true);
            }
        }
    }

    public void TurnPosition(Transform[] pos, int num)
    {
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
