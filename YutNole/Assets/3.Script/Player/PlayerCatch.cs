using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCatch : NetworkBehaviour
{
    private PlayerMovement playerMovement;
    private PlayingYut playingYut;
    [SerializeField] private PlayerState[] player1; // GameManager에서 갖고오기
    [SerializeField] private PlayerState[] player2; // GameManager에서 갖고오기
    private int catchPlayer = 0;
    private bool isMyPlayer = false;

    #region Unity Callback
    private void Start()
    {
        TryGetComponent(out playerMovement);
        playingYut = FindObjectOfType<PlayingYut>().transform.GetComponent<PlayingYut>();
    }
    #endregion
    #region SyncVar
    // 플레이어 위치
    #endregion
    #region Client
    [Client]
    private void Catch()
    { // 도개걸윷모빽도 누르고, 플레이어의 움직임이 전부 끝났을 때 실행
        isMyPlayer = CatchChecker();
        if (isMyPlayer)
        {
            SamePlayerCatch();
        }
        else
        {
            OtherPlayerCatch();
        }
    }

    private bool CatchChecker()
    {

        PlayerState[] players = player1;
        PlayerState playerSet;
        catchPlayer = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = i; j < 4; j++)
            {
                if (players[i].currentArray.Equals(players[j].currentArray)
                    && players[i].currentIndex.Equals(players[j].currentIndex))
                { // 플레이어의 위치와 배열이 같을 때
                    catchPlayer++;
                }
            }
        }

        return true;
    }
    #endregion
    #region Command
    [Command(requiresAuthority = false)]
    private void SamePlayerCatch()
    { // 자신의 말을 잡았을 때 : 겹쳐서 가기
        Debug.Log("SameCatch");
        // if (player == player1)
    }

    [Command(requiresAuthority = false)]
    private void OtherPlayerCatch()
    { // 상대방의 말을 잡았을 때 : 상대방의 말을 원위치로 보내주기
        Debug.Log("OtherCatch");

        RCP_OtherPlayerCatch();
    }
    #endregion
    #region ClientRPC
    [ClientRpc]
    private void RCP_SamePlayerCatch(PlayerState players)
    { // 자신의 NumberImage SetActive(true) 및 변경 해주기
        for (int i = 0; i < 3; i++)
        {

        }
    }

    [ClientRpc]
    private void RCP_OtherPlayerCatch()
    { // 상대방의 말을 원위치로

    }
    #endregion
}