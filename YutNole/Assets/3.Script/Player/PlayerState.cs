using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class PlayerState : NetworkBehaviour
{
    public PlayingYut playingYut;

    // Player의 위치
    public int playerNum = 0; // 플레이어의 처음 위치
    public List<PlayerState> carryPlayer = new List<PlayerState>();
    public Animator ani;
    public Transform currentPositon;
    public Transform[] currentArray; // 자신이 현재 위치한 배열
    public bool isWaiting;
    public int currentIndex = 0; // 현재 위치한 인덱스

    public bool isPlaying = false; // 판에 나왔는지 아닌지
    public bool isGoal = false; // 골인 했는지 아닌지
    public Transform startPos;

    // Player NumImage
    public GameObject[] numImage; // numberImage GameObject 참조해주기

    #region Unity Callback
    private void Start()
    {

        SetUp();
        playingYut.goalButton.GetComponent<Button>().onClick.AddListener(GoalInClick); // 골인 버튼을 눌렀을 때
        ani = transform.GetChild(2).GetComponent<Animator>();
    }

    private void Update()
    {
        currentPositon = currentArray[currentIndex];
    }

    public void CarryNumSetting()
    {
        switch (carryPlayer.Count) {
            case 1:
                for (int i = 0; i < numImage.Length; i++)
                {
                    numImage[i].SetActive(false);
                }
                numImage[0].SetActive(true);
                break;
            case 2:
                for (int i = 0; i < numImage.Length; i++)
                {
                    numImage[i].SetActive(false);
                }
                numImage[1].SetActive(true);
                break;
            case 3:
                for (int i = 0; i < numImage.Length; i++)
                {
                    numImage[i].SetActive(false);
                }
                numImage[2].SetActive(true);
                break;
            default:
                for (int i = 0; i < numImage.Length; i++)
                {
                    numImage[i].SetActive(false);
                }
                break;
        }
    }

    private void SetUp()
    { // 플레이어 상태 처음 설정
        playingYut = FindObjectOfType<PlayingYut>();
        currentArray = playingYut.pos1;
    }

    #endregion
    #region SyncVar
    #endregion
    #region Client
    [Client]
    public void GoalInClick()
    {
        GoalIn_Command();
    }
    #endregion
    #region Command
    [Command(requiresAuthority = false)]
    private void GoalIn_Command()
    {
        isGoal = true;
        GoalIn_RPC();
        GoalInPlayerReset(this);
    }
    #endregion
    #region ClientRpc
    [ClientRpc]
    public void GoalIn_RPC()
    {
        for (int i = 0; i < numImage.Length; i++)
        {
            this.numImage[i].SetActive(false);
        }

        this.startPos.GetComponent<SpriteRenderer>().enabled = true;
        if (this.carryPlayer.Count != 0)
        {
            for (int i = 0; i < this.carryPlayer.Count; i++)
            {
                this.carryPlayer[i].startPos.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
    [ClientRpc]
    private void GoalInPlayerReset(PlayerState player)
    {
        if (player.carryPlayer.Count > 0)
        {
            //업힌말들 초기화해주고 Active 켜주기
            for (int i = 0; i < player.carryPlayer.Count; i++)
            {
                //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ초기화
                player.carryPlayer[i].transform.position = player.carryPlayer[i].startPos.position;
                //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
                player.carryPlayer[i].gameObject.SetActive(true); // false 됬던거 true
            }
            //말 초기화해줬으면 업힌말들 담았던 리스트 초기화
            player.carryPlayer.Clear();
            // 업힌말 표기하는 숫자 오브젝트도 초기화
            player.CarryNumSetting();
        }
    }
    #endregion
    #region Hook Method, 다른 클라이언트도 알아야 함
    #endregion
}