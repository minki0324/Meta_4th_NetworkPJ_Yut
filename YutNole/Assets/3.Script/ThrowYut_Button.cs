using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ThrowYut_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Button ThrowYut_Btn;

    [SerializeField]
    private Sprite[] ThrowYut_sprites;
    /*
     0. Disable
    1. Idle
    2. Mouse Over
    3. Press
     */
    [SerializeField] private Yut_Gacha Yut_Ani;


    bool isAbleTo_Throw;

    void Start()
    {
  
        ThrowYut_Btn.GetComponent<Button>();
      
        ThrowYut_Btn.onClick.AddListener(ThrowYut_Btn_Clicked);
        ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[0];
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAbleTo_Throw)
        {
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[2];
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAbleTo_Throw)
        {
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[1];
        }         

    }



    public void ThrowYut_Btn_Clicked()
    {
        ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[3];
        Yut_Ani.Throwing();

        if (!GameManager.instance.playerState.hasChance)
        {
            //윷던지기 버튼 비활성화
            ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[0];
            ThrowYut_Btn.enabled = false;
            isAbleTo_Throw = false;
        }
        else
        {
            ThrowYut_Btn.enabled = true;
            isAbleTo_Throw = true;
     
        }

    }

    

}