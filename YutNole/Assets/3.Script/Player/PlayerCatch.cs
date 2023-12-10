using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCatch : NetworkBehaviour
{
    private PlayerMovement playerMovement;
    private PlayingYut playingYut;
    [SerializeField] private PlayerState[] player1; // GameManager���� �������
    [SerializeField] private PlayerState[] player2; // GameManager���� �������
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
    // �÷��̾� ��ġ
    #endregion
    #region Client
    [Client]
    private void Catch()
    { // ���������𻪵� ������, �÷��̾��� �������� ���� ������ �� ����
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
                { // �÷��̾��� ��ġ�� �迭�� ���� ��
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
    { // �ڽ��� ���� ����� �� : ���ļ� ����
        Debug.Log("SameCatch");
        // if (player == player1)
    }

    [Command(requiresAuthority = false)]
    private void OtherPlayerCatch()
    { // ������ ���� ����� �� : ������ ���� ����ġ�� �����ֱ�
        Debug.Log("OtherCatch");

        RCP_OtherPlayerCatch();
    }
    #endregion
    #region ClientRPC
    [ClientRpc]
    private void RCP_SamePlayerCatch(PlayerState players)
    { // �ڽ��� NumberImage SetActive(true) �� ���� ���ֱ�
        for (int i = 0; i < 3; i++)
        {

        }
    }

    [ClientRpc]
    private void RCP_OtherPlayerCatch()
    { // ������ ���� ����ġ��

    }
    #endregion
}