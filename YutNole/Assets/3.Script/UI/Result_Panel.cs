using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerStates
{
    //임의로 구조체 파겟습니다 나중에 수정하기
    //나중에 게임매니저에 넣기
    public bool isP1;
    public bool isP2;

    public bool hasChance;


    public bool isUnit1_Alive;
    public bool isUnit2_Alive;
    public bool isUnit3_Alive;
    public bool isUnit4_Alive;

    public int GoalCount;

}


public class Result_Panel : MonoBehaviour
{
    //던진 결과 
    [SerializeField] private Image[] Result_imgs;

    [SerializeField]  private Sprite[] Result_sprites;

    [SerializeField] private Yut_Gacha yutGacha;



    private void Start()
    {
        foreach(Image e in Result_imgs)
        {
           
            e.gameObject.SetActive(false);
        }

    }


    private void Update()
    {


       
    }


    public void Set_Result()
    {
        //0번이 활성화 중이 아니면 0번 활성화
        //만약 0번이 활성화 중이면 0번을 1번으로 새로들어온건 0번으로
        int start = 0;
        for (int i = start; i < Result_imgs.Length; i++)
        {
            if (Result_imgs[i].isActiveAndEnabled) start++;
          
        }

        for (int i = start; i < Result_imgs.Length; i++) 
        {
        
            if (!Result_imgs[i].isActiveAndEnabled)
            {
                Debug.Log("1번 활성화");
                //i번이 활성화 되지 않았으면
                Result_imgs[i].gameObject.SetActive(true);
                Throw_Result(ref (Result_imgs[i]));
                break;
            }
            else
            {
                //활성화 되어 있으면


                if (i<4)
                {
                    //두개까지만됨..2중 for문 안쓰고 하는 방법을 생각해복자..
                    Result_imgs[i + 1].gameObject.SetActive(true);
                    Result_imgs[i + 1].sprite = Result_imgs[i].sprite;
                    Throw_Result(ref (Result_imgs[i]));
                    break;
                           
                }

            }

        }

    
    }


    public void Throw_Result(ref Image img)
    {

        switch (yutGacha.ThrowResult)
        {

            case "Backdo":
                img.sprite = Result_sprites[0];
                break;

            case "Do":
                img.sprite = Result_sprites[1];
                break;

            case "Gae":
                img.sprite = Result_sprites[2];
                break;

            case "Geol":
                img.sprite = Result_sprites[3];
                break;

            case "Yut":
                img.sprite = Result_sprites[4];
                break;

            case "Mo":
                img.sprite = Result_sprites[5];
                break;

        }


    }
}
