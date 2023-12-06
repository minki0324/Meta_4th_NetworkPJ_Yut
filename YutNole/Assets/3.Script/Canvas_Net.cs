using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Canvas_Net : MonoBehaviour
{

    [SerializeField]
    private Slider P1_Timer_slider;

    [SerializeField]
    private Slider P2_Timer_slider;





    //플레이어 말
    [SerializeField]
    private GameObject[] P1_Unit;

    [SerializeField]
    private GameObject[] P2_Unit;

    //기타
    public float ThrowTime = 0f;
    public float SliderSpeed = 1f;

    private void Start()
    {
        Init();

        StartCoroutine(Throw_Timer());
    }

    private void Init()
    {
        P1_Timer_slider.GetComponent<Slider>();
        P2_Timer_slider.GetComponent<Slider>();
    }


    public IEnumerator Throw_Timer()
    {

        //턴이 1p면 조건달기
        P1_Timer_slider.value = 0;

        //턴이 2p면 조건달기
        P2_Timer_slider.value = 0;

        while (true)
        {
            //턴이 1p면 조건달기
            P1_Timer_slider.value += SliderSpeed;

            //턴이 2p면 조건달기
            P2_Timer_slider.value += SliderSpeed;

            if (ThrowTime > 10)
            {
                break;
            }

            yield return new WaitForSeconds(SliderSpeed);
        }
 
    }

}
    