using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ThrowYut_Button : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    [SerializeField]
    private Button ThrowYut_Btn;

    [SerializeField]
    public Sprite[] ThrowYut_sprites;
    /*
     0. Disable
    1. Idle
    2. Mouse Over
    3. Press
     */
    [SerializeField] private Yut_Gacha Yut_Ani;

    [SerializeField] private Unit_Panel unitPanel;

    [SerializeField] private PlayerMovement playerMovement;


    //변수 삭제하기
    bool isAbleTo_Throw;

    void Start()
    {

        unitPanel = FindObjectOfType<Unit_Panel>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        ThrowYut_Btn.GetComponent<Button>();
      
        ThrowYut_Btn.onClick.AddListener(ThrowYut_Btn_Clicked);


        //임시로 윷던지기용 코드

        GameManager.instance.hasChance = true;
        ThrowYut_Button yut_Button = FindObjectOfType<ThrowYut_Button>();
        yut_Button.GetComponent<Image>().sprite = yut_Button.ThrowYut_sprites[1];


    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.instance.hasChance)
        {
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[2];
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.instance.hasChance)
        {
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[1];
        }

    }



    public void ThrowYut_Btn_Clicked()
    {
        playerMovement.PlayerMove();

        if (!GameManager.instance.hasChance)
        {
            //윷던지기 버튼 비활성화
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[0];
            ThrowYut_Btn.enabled = false;
            isAbleTo_Throw = false;
        }
        else
        {
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[3];
            Yut_Ani.Throwing();

            //for (int i = 0; i < unitPanel.P1_Units.Count; i++)
            //{
            //    if(GameManager.instance.isThrew)
            //    {
            //        //화살표 이미지
            //        unitPanel.P1_Units[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
            //    }
                
               
            //}


            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[0];
            ThrowYut_Btn.enabled = true;
            // isAbleTo_Throw = true;

      
        }

    }


}