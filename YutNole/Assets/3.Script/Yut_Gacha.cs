using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yut_Gacha : MonoBehaviour
{
    /*
        1. 확률에 따라서 윷 애니메이션 출력
    */

    private Animator Yut_ani;

    public string ThrowResult;

 

    [SerializeField] Result_Panel resultPanel;
    [SerializeField] Unit_Panel unitPanel;

    private void Awake()
    {
        Yut_ani = GetComponent<Animator>();
        unitPanel = FindObjectOfType<Unit_Panel>();
        GameManager.instance.playerState.hasChance = true;
    }

    public void Throwing()
    {

        //내턴인 경우 체크하기 

        if (GameManager.instance.playerState.hasChance)
        {
            GameManager.instance.playerState.hasChance = false;
            GameManager.instance.isThrew = true;
            //캐릭터 움직이고 isThrew false로 변경

            for (int i = 0; i < 4; i++)
            {
                unitPanel. P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = true;
            }
            //string[] triggers = { "Do", "Do", "Do", "Backdo", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo", "Nack", "Nack" };
            string[] triggers = { "Do", "Do", "Do", "Backdo", "Gae", "Gae", "Gae", "Gae", "Gae", "Gae", "Geol", "Geol", "Geol", "Geol", "Yut", "Mo", "Nack", "Nack" ,"Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo", "Mo"};

            ThrowResult = triggers[Random.Range(0, triggers.Length)];

            Yut_ani.SetTrigger(ThrowResult);

    


            if (ThrowResult.Equals("Yut"))
            {
                GameManager.instance.playerState.hasChance = true;
                GameManager.instance.isThrew = false;
            }
            if (ThrowResult.Equals("Mo"))
            {
                GameManager.instance.playerState.hasChance = true;
                GameManager.instance.isThrew = false;
            }

            resultPanel.Set_Result();



        }

    }
}
