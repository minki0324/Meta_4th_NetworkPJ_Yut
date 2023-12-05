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

    bool isMouseOn;

    void Start()
    {
        ThrowYut_Btn.GetComponent<Button>();
      


        ThrowYut_Btn.onClick.AddListener(ThrowYut_Btn_Clicked);
        ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[0];
    }


    private void Update()
    {

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[2];

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[1];

    }



    public void ThrowYut_Btn_Clicked()
    {
        ThrowYut_Btn.GetComponent<Image>().sprite = ThrowYut_sprites[3];
        Yut_Ani.Throwing();

    }

}