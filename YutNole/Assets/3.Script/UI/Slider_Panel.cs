using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Slider_Panel : MonoBehaviour
{

    [SerializeField]
    private Slider P1_Timer_slider;

    [SerializeField]
    private Slider P2_Timer_slider;

    //기타
    public float TimeLimit = 30; //던지고 뭐시기하는 제한시간
    public float ThrowTime = 0f;
    public float SliderSpeed = 0.1f;

    private void Start()
    {
        Init();


        //껏다 켜기 -> 내턴이 될떄 켜도록 코딩하기
        StartCoroutine(Throw_Timer());
    }

    private void Init()
    {
        P1_Timer_slider.GetComponent<Slider>();
        P2_Timer_slider.GetComponent<Slider>();


        //턴이 1p면 조건달기
        P1_Timer_slider.value = 0;

        //턴이 2p면 조건달기
        P2_Timer_slider.value = 0;

        P1_Timer_slider.maxValue = TimeLimit;
        P2_Timer_slider.maxValue = TimeLimit;

    }


    public IEnumerator Throw_Timer()
    {


        while (true)
        {
            //턴이 1p면 조건달기
            P1_Timer_slider.value += SliderSpeed;

            //턴이 2p면 조건달기
            P2_Timer_slider.value += SliderSpeed;

            if (ThrowTime > TimeLimit)
            {
                break;
            }

            yield return new WaitForSeconds(SliderSpeed);
        }

    }


}
