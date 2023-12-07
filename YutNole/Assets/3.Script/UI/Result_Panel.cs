using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Result_Panel : MonoBehaviour
{
    //던진 결과 
    [SerializeField] private Image[] Result_imgs;

    [SerializeField]  private Sprite[] Result_sprites;

    [SerializeField] private Yut_Gacha yutGacha;

    [SerializeField] private List<string> Results;


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

        //꺼진 오브젝트가 있나 확인 및 start 증가
        for (int i = start; i < Result_imgs.Length; i++)
        {
            if (Result_imgs[i].gameObject.activeSelf) start++;

        }
        Debug.Log(start);



        for (int i = start; i < Result_imgs.Length; i++)
        {

            if (!Result_imgs[i].gameObject.activeSelf)
            {
                Result_imgs[i].gameObject.SetActive(true);
                Throw_Result(ref (Result_imgs[i]));
                break;
            }
            else
            {
                //활성화 되어 있으면
                if (i < 4)
                {

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
