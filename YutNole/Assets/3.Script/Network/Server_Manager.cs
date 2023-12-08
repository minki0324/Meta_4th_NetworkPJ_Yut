using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server_Manager : NetworkBehaviour
{
    public static Server_Manager instance;

    #region SyncVar
    [SyncVar(hook = nameof(OnTurn_Finish))] 
    public int Turn_Index = 1;
    
    #endregion

    #region Client
    #endregion

    #region Command
    [Command(requiresAuthority = false)]
    public void CMD_Turn_Changer()
    {
        int next_Index = (Turn_Index % 2) + 1;
        OnTurn_Finish(Turn_Index, next_Index);
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
            return;
        }
        StartCoroutine(DelayedFirstTurnSet());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            CMD_Turn_Changer();
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
