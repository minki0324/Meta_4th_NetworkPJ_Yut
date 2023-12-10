using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginControll : MonoBehaviour
{
    public GameObject JoinUI;
    public InputField id_i;
    public InputField pass_i;
    public InputField nickName;
    [SerializeField] private Text Log;
    public void Login_btn()
    {
        if(id_i.text.Equals(string.Empty) || pass_i.text.Equals(string.Empty))
        {
            Log.text = "���̵� ��й�ȣ�� �Է��ϼ���.";  
            return;
        }

        if(SQLManager.instance.Login(id_i.text, pass_i.text))
        {
            //�α��� ����
            Debug.Log("��������");
            user_info info = SQLManager.instance.info;
            Debug.Log(info.User_ID + " �� " + info.User_Password +" l " + info.User_name);
            ServerChecker.instance.Start_Client();
        }
        else
        {
            //�α��� ����
            Log.text = "���̵� ��й�ȣ�� Ȯ���� �ּ���..";
        }
    }

    public void Join()
    {
        if (id_i.text.Equals(string.Empty) || pass_i.text.Equals(string.Empty) )
        {
            Log.text = "���̵� ��й�ȣ�� �Է��ϼ���.";
            return;
        }
        if (nickName.text.Equals(string.Empty))
        {
            Log.text = "�г����� �Է� ���ּ���";
            return;
        }

        if(SQLManager.instance.Join(id_i.text,pass_i.text , nickName.text))
        {
            Debug.Log($"id : {id_i.text} l pass : {pass_i.text} l nickName : {nickName.text}");
            gameObject.SetActive(false);
        }
        else
        {
            Log.text = "���̵� Ȥ�� �г����� �̹� �����մϴ�";
        }
        
    }
    public void OpenJoinUI()
    {
        JoinUI.SetActive(true);
    }
}
