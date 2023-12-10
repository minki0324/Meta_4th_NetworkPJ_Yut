using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Result_Yut : NetworkBehaviour
{
    /*
        1. �����⸦ �ϰ� ���� ��Ʈ������ ����Ʈ�� ���� �� ����
        2. ��Ʈ�� ���� ���� ������� ���
    */

    [SerializeField] private Image[] results; // ���� ĭ
    [SerializeField] private Sprite[] yut_Counts; // ���� ��������Ʈ
    [SerializeField] private Throw_Yut throw_Btn; // ������ ��ư

    #region SyncVar
    [SyncVar(hook = nameof(ResultList_Renew))]
    public SyncList<string> result_Value = new SyncList<string>(); // � ���Դ��� ��� ����Ʈ
    #endregion

    #region Server
    [Server] // ���������� ȣ��, ������ ���� ����� �������ִ� �޼ҵ�
    public void Set_Result(string trigger, bool Add_Del)
    {
        if (Add_Del)
        {
            // �߰� �۾�
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
            // ���� �۾�
            if (result_Value.Count > 0)
            {
                // ����Ʈ���� trigger ���� ��ġ�ϴ� ù ��° �׸��� ã�� ����
                string itemToRemove = result_Value.Find(item => item == trigger);
                if (itemToRemove != null)
                {
                    result_Value.Remove(itemToRemove);
                }
            }
        }
        RPCSet_Result();
    }
    #endregion

    #region Client
    #endregion

    #region Command
    [Command(requiresAuthority =false)] // �׽�Ʈ �Դϴ�.
    private void Testt()
    {
        Set_Result("Gae", false);
        // ��� �� ����
        StartCoroutine(DelayedRPCSetResult());
    }
    #endregion

    #region ClientRPC
    [ClientRpc] // Ŭ���̾�Ʈ �鿡�� ��� ������ ������ִ� �޼ҵ�
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
    private void Update() // �׽�Ʈ��
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Testt();
        }
    }
    #endregion

    #region Hook Method
    // ����Ʈ�� �������� ���� �� �ڵ����� �ٲ��ִ� Hook
    private void ResultList_Renew(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        ModifyList(oldItem, newItem);
    }
    #endregion

    #region Other Utils
    // �������� ����� �޾Ƽ� ����Ʈ �߰�, Ȥ�� ���� ���ִ� �޼ҵ�
    private void ModifyList(string oldItem, string newItem)
    {
        // ����Ʈ�� null�� ���� ���, �ش� null�� ����
        result_Value.RemoveAll(item => item == null);

        // ��� �� �߰� �Ǵ� ����
        if (!string.IsNullOrEmpty(newItem))
        {
            result_Value.Add(newItem);
        }
        else
        {
            result_Value.Remove(oldItem);
        }

        // ��� �� ����
        StartCoroutine(DelayedRPCSetResult());

        Debug.Log($"ModifyList: oldItem = {oldItem}, newItem = {newItem}, result_Value.Count = {result_Value.Count}");
    }

    // ������ ������� ���� ��������Ʈ �ٲ��ִ� �޼ҵ�
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

    // RPC �޼ҵ� ��� �ڷ�ƾ
    private IEnumerator DelayedRPCSetResult()
    {
        yield return null; // 1������ ���
        RPCSet_Result();
    }

    #endregion
}
