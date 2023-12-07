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
            Log.text = "아이디 비밀번호를 입력하세요.";  
            return;
        }

        if(SQLManager.instance.Login(id_i.text, pass_i.text))
        {
            //로그인 성공
            user_info info = SQLManager.instance.info;
            Debug.Log(info.User_ID + " ㅣ " + info.User_Password +" l " + info.User_name);
            ServerChecker.instance.Start_Client();
        }
        else
        {
            //로그인 실패
            Log.text = "아이디 비밀번호를 확인해 주세요..";
        }
    }

    public void Join()
    {
        if (id_i.text.Equals(string.Empty) || pass_i.text.Equals(string.Empty) )
        {
            Log.text = "아이디 비밀번호를 입력하세요.";
            return;
        }
        if (nickName.text.Equals(string.Empty))
        {
            Log.text = "닉네임을 입력 해주세요";
            return;
        }

        if(SQLManager.instance.Join(id_i.text,pass_i.text , nickName.text))
        {
            Debug.Log($"id : {id_i.text} l pass : {pass_i.text} l nickName : {nickName.text}");
            gameObject.SetActive(false);
        }
        else
        {
            Log.text = "아이디 혹은 닉네임이 이미 존재합니다";
        }
        
    }
    public void OpenJoinUI()
    {
        JoinUI.SetActive(true);
    }
}
