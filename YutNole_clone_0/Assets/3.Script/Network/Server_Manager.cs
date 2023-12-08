using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server_Manager : NetworkBehaviour
{
    #region SyncVar
    [SyncVar(hook = nameof(OnTurn_Finish))] 
    public int Turn_Index = 1;
    
    #endregion

    #region Client
    #endregion

    #region Command
    [Command]
    public void CMD_Turn_Checker()
    {
        // GetConnectionToClient()를 통해 현재 명령을 실행하는 클라이언트의 커넥션을 얻습니다.
        NetworkConnectionToClient connToClient = connectionToClient;

        // 클라이언트의 커넥션이 있을 경우에만 로직을 수행합니다.
        if (connToClient != null)
        {
            int Ran_Num = Random.Range(1, 3);
            OnTurn_Finish(Turn_Index, Ran_Num);
        }
    }

    // 시작시 1, 2중 한개를 골라 랜덤으로 턴을 생성하는 로직 > 룸 매니저에서 게임씬으로 변경됐을 때 호출
    [Command]
    public void First_TurnSet()
    {
        StartCoroutine(DelayedFirstTurnSet());
    }
    #endregion

    #region ClientRPC
    #endregion

    #region Unity Callback
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            CMD_Turn_Checker();
        }
    }
    #endregion

    #region Hook Method
    public void OnTurn_Finish(int _old, int _new)
    {
        Turn_Index = _new;
    }
    #endregion


    private IEnumerator DelayedFirstTurnSet()
    {
        // 1초의 딜레이를 줌 
        yield return new WaitForSeconds(1f);

        int Ran_Num = Random.Range(1, 3);
        OnTurn_Finish(Turn_Index, Ran_Num);
    }
}
