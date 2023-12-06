using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Panel : MonoBehaviour
{
    //플레이어 말
    [SerializeField]
    private GameObject P1_Unit;

    [SerializeField]
    private GameObject P2_Unit;



    public List<GameObject> P1_Units;
    // private GameObject[] P1_Unit;

    [SerializeField]
    public List<GameObject> P2_Units;
    // private GameObject[] P2_Unit;

    [SerializeField]
    private Sprite Goal_sprite;



    private void Start()
    {
        
        for(int i= 0; i<4; i++)
        {
            P1_Units.Add(P1_Unit.transform.GetChild(i).gameObject);
            P2_Units.Add(P2_Unit.transform.GetChild(i).gameObject);
        }



        foreach (GameObject obj in P1_Units)
        {
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);      //return 버튼 비활성화
            obj.transform.GetChild(1).gameObject.SetActive(false);      //골인 img 비활성화

            GameObject btn = obj.transform.GetChild(0).gameObject;  //캐릭터img 
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Character_Clicked(ref btn); });

            obj.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Return_Clicked(ref btn); });



        }

        foreach (GameObject obj in P2_Units)
        {
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);      //return 버튼 비활성화
            obj.transform.GetChild(1).gameObject.SetActive(false);      //골인 비활성화
        }

    }

    private void Update()
    {
        
    }

    public void Return_Clicked(ref GameObject parentObj)
    {
        //매개변수로 들어온 GameObject는 캐릭터 이미지이므로 상속 0번이 return버튼

        parentObj.transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = true;
        }
    }

    public void Character_Clicked(ref GameObject btn)
    {

        if(GameManager.instance.isThrew)
        {


            //모든말이 제자리에 있으면 == 다 켜져있으면 클릭한 놈 끄기
            //하나라도 자리에 없으면 return버튼 켜기

            int unitCount = 0;

            //만약 플레이어 1이면
            for (int i = 0; i < P1_Units.Count; i++)
            {
                if (P1_Units[i].transform.GetChild(0).gameObject.activeSelf)
                {
                    //값이 일루만 들어옴..
                    unitCount++;
                    Debug.Log(unitCount);
                }
            }


            if (unitCount >= 4)
            {
                //다 켜져있는 경우
                btn.SetActive(false);
                for (int i = 0; i < 4; i++)
                {
                    P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    //p2인경우도 만들기
                    if (P1_Units[i].transform.GetChild(0).gameObject.Equals(btn))
                    {
                        btn.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = false;


                    }
                }



                //재윤아 보드에 목적지 화살표 뜨게 부탁할게..^^
                //재윤아 보드에 목적지 누르면 GameManager에 isThrew 변수도 false로 바꿔줘 부탁해..^^


            }
        }

       


    }





    public void Check_Goal()
    {
        //골인한경우

    }
    

}
