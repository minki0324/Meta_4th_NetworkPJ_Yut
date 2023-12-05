using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ThrowYut_Button : MonoBehaviour
{
    [SerializeField]
    private Button ThrowYut_Btn;

    [SerializeField]
    private Sprite[] ThrowYut_sprites;

    [SerializeField] private Yut_Gacha Yut_Ani;
    
    /*
     0. Disable
    1. Idle
    2. Mouse Over
    3. Press
     */


    bool isMouseOn;

    public void ThrowYut_Btn_Clicked()
    {
        Yut_Ani.Throwing();
    }

}
