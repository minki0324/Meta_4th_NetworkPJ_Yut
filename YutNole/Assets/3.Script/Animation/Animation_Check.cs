using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animation_Check : MonoBehaviour
{
    [SerializeField]
    private Yut_Gacha yutGacha;

    [SerializeField]
    ThrowYut_Button throwBtn;

    [SerializeField]
    Unit_Panel unitPanel;

    
    void Start()
    {
        yutGacha = FindObjectOfType<Yut_Gacha>();
        throwBtn = FindObjectOfType<ThrowYut_Button>();
        unitPanel = FindObjectOfType<Unit_Panel>();

    }


    public void Throwing_End()
    {
        if(GameManager.instance.isPlayer1)
        {
            for (int i = 0; i < unitPanel.P1_Units.Count; i++)
            {
                if (GameManager.instance.isThrew)
                {
                    //화살표 이미지
                    unitPanel.P1_Units[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < unitPanel.P2_Units.Count; i++)
            {
                if (GameManager.instance.isThrew)
                {
                    //화살표 이미지
                    unitPanel.P2_Units[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
                }
            }
        }

    }

    public void Throwing_Start()
    {

        if (GameManager.instance.isPlayer1)
        {
            for (int i = 0; i < unitPanel.P1_Units.Count; i++)
            {
                if (GameManager.instance.isThrew)
                {
                    //화살표 이미지
                    unitPanel.P1_Units[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < unitPanel.P2_Units.Count; i++)
            {
                if (GameManager.instance.isThrew)
                {
                    //화살표 이미지
                    unitPanel.P2_Units[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().gameObject.SetActive(false);
                }
            }
        }
  
    }




}
