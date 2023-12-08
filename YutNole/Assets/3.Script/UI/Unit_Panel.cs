using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Panel : MonoBehaviour
{
    [SerializeField] private PlayingYut playingYut;



    //플레이어 말
    [SerializeField]
    private GameObject P1_Unit;

    [SerializeField]
    private GameObject P2_Unit;

    public bool canBoard = false;   //말판에 나갈수 있는가 판단하는 변수


    public List<GameObject> P1_Units;
    // private GameObject[] P1_Unit;

    [SerializeField]
    public List<GameObject> P2_Units;
    // private GameObject[] P2_Unit;

    [SerializeField]
    private Sprite Goal_sprite;


 

    private void Start()
    {
        playingYut = FindObjectOfType<PlayingYut>();

 
        for (int i= 0; i<4; i++)
        {
            P1_Units.Add(P1_Unit.transform.GetChild(i).gameObject);
            P2_Units.Add(P2_Unit.transform.GetChild(i).gameObject);

            P1_Units[i].name = "Unit" + i;
            P2_Units[i].name = "Unit" + i;

        }

        foreach (GameObject obj in P1_Units)
        {
            //return 버튼
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);


            //선택 화살표
            obj.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            obj.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().raycastTarget = false;

            //골인 img
            obj.transform.GetChild(1).gameObject.SetActive(false);

            //캐릭터 이미지
            GameObject btn = obj.transform.GetChild(0).gameObject;
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Character_Clicked(ref btn); });
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener( playingYut.CharacterButtonClick);

            

            //return 버튼
            obj.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Return_Clicked(ref btn); });


        }


        foreach (GameObject obj in P2_Units)
        {
            //return 버튼
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);


            //선택 화살표
            obj.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            obj.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().raycastTarget = false;

            //골인 img
            obj.transform.GetChild(1).gameObject.SetActive(false);

            //캐릭터 이미지
            GameObject btn = obj.transform.GetChild(0).gameObject;
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Character_Clicked(ref btn); });

            //return 버튼
            obj.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Return_Clicked(ref btn); });

        }



        #region 나중에 자기턴 구분하는 메소드에 넣기
        if (GameManager.instance.isPlayer1)
        {
            foreach (GameObject obj in P1_Units)
            {
                obj.transform.GetChild(0).GetComponent<Button>().enabled = true;
            }

            foreach (GameObject obj in P2_Units)
            {
                obj.transform.GetChild(0).GetComponent<Button>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject obj in P2_Units)
            {
                obj.transform.GetChild(0).GetComponent<Button>().enabled = true;
            }

            foreach (GameObject obj in P1_Units)
            {
                obj.transform.GetChild(0).GetComponent<Button>().enabled = false;
            }
        }

        #endregion
    }


    //Return_Btn 클릭시 호출되는 메소드
    public void Return_Clicked(ref GameObject parentObj)
    {
        //매개변수로 들어온 GameObject는 캐릭터 이미지이므로 상속 0번이 return버튼

        parentObj.transform.GetChild(0).gameObject.SetActive(false);

        if(GameManager.instance.isPlayer1)
        {
            for (int i = 0; i < 4; i++)
            {
                P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = true;
                //선택 화살표 다시 띄우기
                P1_Units[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                P2_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = true;
                P2_Units[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
   
    }





    //캐릭터 클릭시 호출되는 메소드
    public void Character_Clicked(ref GameObject btn)
    {



        if (GameManager.instance.isThrew)
        {
          
            //모든말이 제자리에 있으면 == 다 켜져있으면 클릭한 놈 끄기
            //하나라도 자리에 없으면 return버튼 켜기


            int unitCount = 0;

            if(GameManager.instance.isPlayer1)
            {
                for (int i = 0; i < P1_Units.Count; i++)
                {
                    if (P1_Units[i].transform.GetChild(0).gameObject.activeSelf)
                    {                       
                        unitCount++;
                        P1_Units[i].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);                
                    }
                }

            }
            else
            {
                for (int i = 0; i < P2_Units.Count; i++)
                {
                    if (P2_Units[i].transform.GetChild(0).gameObject.activeSelf)
                    {
                
                        unitCount++;
                        P2_Units[i].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                        // player SetActive(true)
                    }
                }

            }




            if (GameManager.instance.isPlayer1)
            {
                if (unitCount >= 4)
                {
                    //다 켜져있는 경우
                    btn.SetActive(false);
                    for (int i = 0; i < 4; i++)
                    {
                        P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = false;

                        // 말판 위 플레이어 켜기
                        if (btn.gameObject.Equals(P1_Units[i].transform.GetChild(0).gameObject))
                        {
                            GameManager.instance.P1_Units_Obj[i].SetActive(true);

                            //재윤아 말판으로 간 친구 몇번짼지 보냈어^_^..얜 첫번째로 선택된 애야
                            GameManager.instance.PlayerIndex.Add(i);        
                            GameManager.instance.playerNum = i;
                            GameManager.instance.playingPlayer[i] = true;
                        }
                    }
                }
                else
                {
                    //하나라도 말판위에 있는 경우
                    for (int i = 0; i < 4; i++)
                    {

                        if (P1_Units[i].transform.GetChild(0).gameObject.Equals(btn))
                        {
                            //매개변수 btn(캐릭터)의 게임오브젝트가 i번째 unit이면 return 버튼 켜기
                            btn.transform.GetChild(0).gameObject.SetActive(true);
                            if (btn.gameObject.Equals(P1_Units[i].transform.GetChild(0).gameObject))
                            {
                                canBoard = true;

                                //재윤아 말판으로 간 친구 몇번짼지 보냈어^_^..얜 첫번째 말고 나머지들 중 선택된거~
                                GameManager.instance.PlayerIndex.Add(i);       
                                GameManager.instance.playerNum = i;
                                GameManager.instance.playingPlayer[i] = true;
                            }
                              
                        }
                        else
                        {
                            //다른 오브젝트면 
                            P1_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (unitCount >= 4)
                {
                    //다 켜져있는 경우
                    btn.SetActive(false);
                    for (int i = 0; i < 4; i++)
                    {
                        P2_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {

                        if (P2_Units[i].transform.GetChild(0).gameObject.Equals(btn))
                        {
                            btn.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        {
                            P2_Units[i].transform.GetChild(0).GetComponent<Button>().enabled = false;


                        }
                    }
                }
            }
                //재윤아 보드에 목적지 화살표 뜨게 부탁할게..^^ ... 완료 ^^ - 재윤
                //재윤아 보드에 목적지 누르면 GameManager에 isThrew 변수도 false로 바꿔줘 부탁해..^^


            }
        }

     





    public void Check_Goal()    
    {
        //골인한경우

    }
    

}
