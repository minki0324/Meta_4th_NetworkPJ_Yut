using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Panel : MonoBehaviour
{
    //플레이어 말
    [SerializeField]
    private GameObject[] P1_Unit;

    [SerializeField]
    private GameObject[] P2_Unit;

    [SerializeField]
    private Sprite Goal_sprite;

    private void Start()
    {
        foreach (GameObject obj in P1_Unit)
        {
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);      //return 버튼 비활성화
            obj.transform.GetChild(1).gameObject.SetActive(false);      //골인 비활성화

            GameObject btn = obj.transform.GetChild(0).gameObject;
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Character_Clicked(ref btn); });
         
        }

        foreach (GameObject obj in P2_Unit)
        {
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);      //return 버튼 비활성화
            obj.transform.GetChild(1).gameObject.SetActive(false);      //골인 비활성화
        }

        //for(int i = 0; i<P1_Unit.Length; i++)
        //{
        //    P1_Unit[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { Character_Clicked(ref P1_Unit[i]);});
        //}
    }

    private void Update()
    {
        
    }


    public void asdf()
    {
        Debug.Log("zzzzz");
    }
    public void Character_Clicked(ref GameObject btn)
    {
        Debug.Log("ㅋㅎㅎ");

        btn.SetActive(false);

    }

    public void Check_Goal()
    {
        //골인한경우

    }
    

}
