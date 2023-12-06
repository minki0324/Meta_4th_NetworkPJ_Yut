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
     * Yut에 따른 플레이어 이동 및 버튼 생성, 화살표 생성 및 화살표 눌렀을 시 플레이어 생성
        Trun인 position에 걸리면 Transform 배열을 바꿔주기
     */
    [SerializeField] private Canvas canvas; // 버튼 상속해줄 Canvas, 윷 던지는에 Canvas 상속 예정
    [SerializeField] private Transform[] pos1;
    [SerializeField] private Transform[] pos2;
    [SerializeField] private Transform[] pos3;
    [SerializeField] private Transform[] pos4;

    [SerializeField] private GameObject[] yutPrefabs; // 도, 개, 걸, 윷, 모 버튼
    [SerializeField] private GameObject player; // 자신의 말
    [SerializeField] private GameObject[] playerButton; // character, return
    
    private bool isTurn = false; // 5, 10, 22 위치에 갔을 때 isTurn = true
    private bool isGoal = false; // 0에 도착했을 때 isGoal = true

    private int currentIndex = 0; // 현재 위치한 인덱스
    public List<int> yutResultIndex = new List<int>(); // yut 결과에 대한 숫자, 이동 버튼 클릭 시 Remove
    private int[] yutList = { 1, 2, 3, 4, 4, -1, 0 };

    // 윷 결과 가져오기
    private Yut_Gacha yutGacha;
    private string yutResult;

    private void Awake()
    {
        yutGacha = FindObjectOfType<Yut_Gacha>();
    }

    public void PlayingYutPlus()
    {
        yutResult = yutGacha.ThrowResult;
        YutState type = (YutState)Enum.Parse(typeof(YutState), yutResult);
        switch (yutResult)
        {
            case "Backdo":
            case "Do":
            case "Gae":
            case "Geol":
                yutResultIndex.Add(yutList[(int)type]);
                break;
            case "Yut":
            case "Mo":
                yutResultIndex.Add(yutList[(int)type]);
                ResultYutAndMo();
                break;
            case "Nack":
                yutResultIndex.Add(yutList[(int)type]);
                break;
        }
        
        switch (currentIndex)
        {
            case 0:
                break;
            case 10:
            case 15:
            case 22:
                TurnPosition(currentIndex);
                break;
        }

        playerButton[0].SetActive(true);
    }

    private void TurnPosition(int index)
    {
        isTurn = true;
        yutResultIndex.Add(currentIndex);
        // pos1 -> pos2, 10

        // pos1 -> pos3, 5

        // pos3 -> pos4, 22

    }

    public void ResultButtonClick()
    {
        // 도개걸윷모 버튼 생성된 거 이벤트, 클릭한 플레이어 위치에 따라 달라짐
        List<GameObject> buttonList = new List<GameObject>();        
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
