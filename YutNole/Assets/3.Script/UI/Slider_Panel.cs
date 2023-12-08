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


        //오브젝트 껏다 켜기 -> 내턴이 될떄 켜도록 코딩하기
        StartCoroutine(Throw_Timer());
    }


    private void Init()
    {
        P1_Timer_slider.GetComponent<Slider>();
        P2_Timer_slider.GetComponent<Slider>();


        if (GameManager.instance.isPlayer1)
        {
            P1_Timer_slider.value = 0;
            P1_Timer_slider.maxValue = TimeLimit;

        }
        else
        {
            P2_Timer_slider.value = 0;
            P2_Timer_slider.maxValue = TimeLimit;

        }



    }


    public IEnumerator Throw_Timer()
    {
      

        if (GameManager.instance.isPlayer1)
        {
            P2_Timer_slider.gameObject.SetActive(false);
            while (true)
            {
                P1_Timer_slider.value += SliderSpeed;
                if (ThrowTime > TimeLimit)
                {
                    GameManager.instance.hasChance = false;
                    break;
                }

                yield return new WaitForSeconds(SliderSpeed);
            }

        }
        else
        {
            P1_Timer_slider.gameObject.SetActive(false);
            while (true)
            {
                P2_Timer_slider.value += SliderSpeed;
                if (ThrowTime > TimeLimit)
                {
                    GameManager.instance.hasChance = false;
                    break;
                }

                yield return new WaitForSeconds(SliderSpeed);
            }

        }



        if (GameManager.instance.isPlayer1)
        {
            P1_Timer_slider.gameObject.SetActive(false);
            P1_Timer_slider.value = 0;

        }
        else
        {
            P2_Timer_slider.gameObject.SetActive(false);
            P2_Timer_slider.value = 0;
        }
            


        yield break;

    }


}
