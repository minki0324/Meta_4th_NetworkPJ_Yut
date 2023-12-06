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

public class PlayingYut : MonoBehaviour, IStateYutResult
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
    public RectTransform[] yutButton; // 도, 개, 걸, 윷, 모 버튼

    [SerializeField] private GameObject player; // 자신의 말
    [SerializeField] private GameObject[] playerButton; // character, return
    
    public int currentIndex = 0; // Button 인덱스
    public int resultIndex = 0; // 버튼 위치할 인덱스
    public List<int> yutResultIndex = new List<int>(); // yut 결과에 대한 숫자, 이동 버튼 클릭 시 Remove
    private int[] yutArray = { 1, 2, 3, 4, 4, -1, 0 }; // 도 개 걸 윷 모 빽도 낙

    // 윷 결과 가져오기
    private Yut_Gacha yutGacha;
    public string yutResult;
    public YutState type;

    private void Awake()
    {
        yutGacha = FindObjectOfType<Yut_Gacha>();
        playerArray = pos1;
    }
    
    public void PlayingYutPlus()
    { // 윷 던지기 버튼 클릭 시
        yutResult = yutGacha.ThrowResult; // 윷 던진 결과
        type = (YutState)Enum.Parse(typeof(YutState), yutResult);
        yutResultIndex.Add(yutArray[(int)type]); // yutResult에 따라 List에 이동할 만큼의 숫자 추가

        Vector3 screen = Camera.main.WorldToScreenPoint(player.transform.position);
        playerButton[0].transform.position = screen; // 나온 윷에 맞는 버튼 포지션 설정
    }

    private void YutButtonPosition()
    {
        // 버튼 활성화 및 위치 설정
        resultIndex = currentIndex + yutArray[(int)type];
        if (yutResult != "Nack")
        {
            // 낙이 아닐 때
            if (yutResult == "BackDo" && currentIndex == 0)
            {
                // 빽도가 불가능
            }
            else
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[resultIndex].transform.position);
                yutButton[(int)type].transform.position = screen; // 나온 윷에 맞는 버튼 포지션 설정
            }
        }
    }

    public void CharacterButtonClick()
    { // Canvas - CharacterButton
        playerButton[0].SetActive(false);
        playerButton[1].SetActive(true);

        for (int i = 0; i < yutButton.Length; i++)
        {
            yutButton[(int)type].gameObject.SetActive(true); // 버튼 활성화
        }

        YutButtonPosition();
    }

    public void ReturnButtonClick()
    { // Canvas - ReturnButton
        playerButton[0].SetActive(true);
        playerButton[1].SetActive(false);
    }

    public void YutButtonClick(string name)
    { // Canvas - YutObject - 도개걸윷모빽도
        YutState yutName = (YutState)Enum.Parse(typeof(YutState), name);
        GameObject btn = yutButton[(int)yutName].gameObject;
        Vector3 screen = Camera.main.WorldToScreenPoint(btn.transform.parent.position); // Canvas 밖으로
        btn.transform.position = screen; // 나온 윷에 맞는 버튼 포지션 설정

        yutResultIndex.Remove(yutArray[(int)type]); // 리스트 삭제
        currentIndex += yutArray[(int)type]; // 현재 인덱스 변경
    }

    public void TurnPosition(Transform[] pos, int num)
    {
        if (pos == pos1)
        {
            if (num == 5)
            {
                // pos1 -> pos3, 5
                playerArray = pos3;
            } else if (num == 10)
            {
                // pos1 -> pos2, 10
                playerArray = pos2;
            }
        } else if (pos == pos3)
        {
            if (num == 8)
            {
                // pos3 -> pos4, 8(22 위치)
                playerArray = pos4;
            }
        }
        // Catch 당했을 때 playerArray = pos1로
    }

    #region State YutResult
    public void ResultYutAndMo()
    {
        // Result: Yut, Mo ... Chance++

    }

    public void ResultNack()
    {
        // Result: Nack ... Chance X, 0
        // 바로 다음 턴으로 넘어감
    }
    #endregion
}
