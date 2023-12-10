using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Result_Yut : NetworkBehaviour
{
    /*
        1. 던지기를 하고 들어온 스트링값을 리스트에 저장 및 제거
        2. 스트링 값에 따라 결과딱지 출력
    */

    [SerializeField] private Image[] results; // 딱지 칸
    [SerializeField] private Sprite[] yut_Counts; // 각각 스프라이트
    [SerializeField] private Throw_Yut throw_Btn; // 던지기 버튼

    #region SyncVar
    [SyncVar(hook = nameof(ResultList_Renew))]
    public SyncList<string> result_Value = new SyncList<string>(); // 몇개 나왔는지 담는 리스트
    #endregion

    #region Server
    [Server] // 서버에서만 호출, 윷놀이 던진 결과값 셋팅해주는 메소드
    public void Set_Result(string trigger, bool Add_Del)
    {
        if (Add_Del)
        {
            // 추가 작업
            if (!string.IsNullOrEmpty(trigger))
            {
                if (result_Value.Count > 0)
                {
                    ModifyList(result_Value[result_Value.Count - 1], trigger);
                }
                else
                {
                    ModifyList(null, trigger);
                }
            }
        }
        else
        {
            // 제거 작업
            if (result_Value.Count > 0)
            {
                // 리스트에서 trigger 값과 일치하는 첫 번째 항목을 찾고 제거
                string itemToRemove = result_Value.Find(item => item == trigger);
                if (itemToRemove != null)
                {
                    result_Value.Remove(itemToRemove);
                }
            }
        }
        StartCoroutine(DelayedRPCSetResult());
    }
    #endregion

    #region Client
    #endregion

    #region Command
    [Command(requiresAuthority =false)] // 테스트 입니다.
    private void Testt()
    {
        Set_Result("Gae", false);
        // 결과 값 전파
        StartCoroutine(DelayedRPCSetResult());
    }
    #endregion

    #region ClientRPC
    [ClientRpc] // 클라이언트 들에게 결과 딱지를 출력해주는 메소드
    private void RPCSet_Result()
    {
        for(int i = 0; i < results.Length; i++)
        {
            results[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < result_Value.Count; i++)
        {
            results[i].gameObject.SetActive(true);
            Throw_Result(ref results[i], result_Value[i]);
        }
    }
    #endregion

    #region Unity Callback
    private void Update() // 테스트용
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Testt();
        }
    }
    #endregion

    #region Hook Method
    // 리스트에 변동사항 있을 때 자동으로 바꿔주는 Hook
    private void ResultList_Renew(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        ModifyList(oldItem, newItem);
    }
    #endregion

    #region Other Utils
    // 실질적인 결과값 받아서 리스트 추가, 혹은 제거 해주는 메소드
    private void ModifyList(string oldItem, string newItem)
    {
        // 리스트에 null이 있을 경우, 해당 null을 제거
        result_Value.RemoveAll(item => item == null);

        // 결과 값 추가 또는 제거
        if (!string.IsNullOrEmpty(newItem))
        {
            result_Value.Add(newItem);
        }
        else
        {
            result_Value.Remove(oldItem);
        }

        // 결과 값 전파
        StartCoroutine(DelayedRPCSetResult());

        Debug.Log($"ModifyList: oldItem = {oldItem}, newItem = {newItem}, result_Value.Count = {result_Value.Count}");
    }

    // 윷놀이 결과값에 따라 스프라이트 바꿔주는 메소드
    private void Throw_Result(ref Image img, string result)
    {
        switch (result)
        {
            case "Backdo":
                img.sprite = yut_Counts[0];
                break;

            case "Do":
                img.sprite = yut_Counts[1];
                break;

            case "Gae":
                img.sprite = yut_Counts[2];
                break;

            case "Geol":
                img.sprite = yut_Counts[3];
                break;

            case "Yut":
                img.sprite = yut_Counts[4];
                break;

            case "Mo":
                img.sprite = yut_Counts[5];
                break;
        }
    }

    // RPC 메소드 대기 코루틴
    private IEnumerator DelayedRPCSetResult()
    {
        yield return new WaitForSeconds(0.1f); // 1프레임 대기
        RPCSet_Result();
    }
    #endregion
}
