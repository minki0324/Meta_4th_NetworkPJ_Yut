using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Throw_Yut : NetworkBehaviour
{
    /*
        1. 윷 던지기 버튼을 누르면 커맨드에서 결과값 만들기
        2. 나온 결과값을 RPC를 통해 모든 클라이언트에게 같은 애니메이션 출력
    */
    private PlayingYut playingYut;
    [SerializeField] private NetworkAnimator Yut_ani;
    [SerializeField] private Result_Yut result;

    public int throw_removeIndex = -1;

    #region Unity Callback
    private void Start()
    {
        playingYut = FindObjectOfType<PlayingYut>();
        for (int i = 0; i < playingYut.yutButton.Length; i++)
        {
            int index = i;
            playingYut.yutButton[i].gameObject.GetComponent<Button>().onClick.AddListener(() => Yut_Btn_Click(index));
        }
        /*playingYut.goalButton.gameObject.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("(throw_removeIndex) 들어왓어요");
            Yut_Btn_Click(throw_removeIndex);
        });*/

        // playingYut.OnDeleteThisIndex += OnDeleteThisIndex;
    }

    private void Update()
    {
        
    }

   /* public void GoalInButtonPlus()
    {
        // 클릭 이벤트에 등록된 모든 리스너를 가져옵니다.
        UnityEngine.Events.UnityEventBase buttonClickEvent = playingYut.goalButton.GetComponent<Button>().onClick;
        for (int i = 0; i < buttonClickEvent.GetPersistentEventCount(); i++)
        {
            // 리스너 확인
            Debug.Log("Listener Count: " + buttonClickEvent.GetPersistentMethodName(i));
        }
        Debug.Log(buttonClickEvent.GetPersistentEventCount());
        int index = playingYut.removeIndex;
        playingYut.goalButton.GetComponent<Button>().onClick.AddListener(() => Yut_Btn_Click(index));
    }*/
    #endregion

    #region SyncVar
    [SyncVar(hook = nameof(TriggerChange))] // 윷놀이 결과값
    private string trigger_ = string.Empty;
    #endregion

    #region Client
    [Client] // 버튼 눌렀을 때 클라이언트 입장에서 서버에게 버튼 눌렸다고 호출해주는 메소드
    public void Btn_Click()
    {
        GameManager.instance.hasChance = false;
        int playPlayer = GameManager.instance.PlayingCount();
        CMDYut_Throwing(playPlayer);
    }


    [Client]
    public void Yut_Btn_Click(int name)
    {
        CMDYut_Button_Click(name);
    }

    private void Addlist(int index)
    {
        playingYut.yutResultIndex.Add(index);
        playingYut.PlayingYutPlus();
    }


    #endregion

    #region Command
    [Command(requiresAuthority = false)] // 실질적인 윷놀이 결과값을 만들어내고 리스트에 저장 및 클라이언트들에게 뿌리는 RPC 메소드 호출
    private void CMDYut_Throwing(int playPlayer)
    {
        string[] triggers = { "Backdo", "Do", "Do", "Do", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo" };
        trigger_ = triggers[Random.Range(0, triggers.Length)];
        // Result_Yut 클래스의 Set_Result 메소드 호출
        if (!(trigger_.Equals("Backdo") && playPlayer == 0))
        {
            GameManager.instance.hasChance = true;
            result.Set_Result(trigger_, true);
        }
        RPCYut_Throwing(trigger_);
    }

    [Command(requiresAuthority = false)]
    private void CMDYut_Button_Click(int name)
    {
        string yutTrigger = string.Empty;
        switch (name)
        {
            case 0:
                yutTrigger = "Do";
                break;
            case 1:
                yutTrigger = "Gae";
                break;
            case 2:
                yutTrigger = "Geol";
                break;
            case 3:
                yutTrigger = "Yut";
                break;
            case 4:
                yutTrigger = "Mo";
                break;
            case 5:
                yutTrigger = "Backdo";
                break;
        }
        result.Set_Result(yutTrigger, false);
    }

    // [Command(requiresAuthority = false)]
    [Client]
    public void ThrowYutResult(string trigger_)
    {
        int index = 0;
        playingYut.yutResult = trigger_;
        switch (trigger_)
        {
            case "Do":
                index = 0;
                break;
            case "Gae":
                index = 1;
                break;
            case "Geol":
                index = 2;
                break;
            case "Yut":
                index = 3;
                GameManager.instance.hasChance = true;
                break;
            case "Mo":
                index = 4;
                GameManager.instance.hasChance = true;
                break;
            case "Backdo":
                index = 5;
                break;
        }
        // 낙이 아닐 때 || (판에 내말이 없으면서 && 빽도가 나올때)
        // 내턴이 아닐 때
        int playPlayer = GameManager.instance.PlayingCount();
        if ((int)GM.instance.Player_Num == Server_Manager.instance.Turn_Index)
        { // 내턴일 때
            if (trigger_.Equals("Backdo") && playPlayer == 0)
            {
                GameManager.instance.PlayerTurnChange();
            }
            else
            {
                Addlist(index);
            }
        }
    }
    #endregion

    #region ClientRPC
    [ClientRpc] // 윷놀이 결과값에 대한 애니메이션만 출력해주는 메소드
    private void RPCYut_Throwing(string trigger)
    {
        Yut_ani.animator.SetTrigger(trigger);
        StartCoroutine(PlaySfx(trigger));
        ThrowYutResult(trigger);
    }
    #endregion
    #region Hook Method
    private void TriggerChange(string _old, string _new)
    {
        trigger_ = _new;
    }
    #endregion
    private IEnumerator PlaySfx(string trigger)
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX(trigger);
    }
}
