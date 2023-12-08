using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArrow_Btn : MonoBehaviour
{
    //YutObject에 넣기

    [SerializeField] private List<Button> buttons;
    [SerializeField] private Unit_Panel unitPanel;


    private void Start()
    {
        buttons.Add(GetComponentInChildren<Button>());
        unitPanel = FindObjectOfType<Unit_Panel>();
    }



    //윷 버튼 클릭 시
    public void YutObject_Clicked()
    {
        GameManager.instance.P1_Units_Obj[GameManager.instance.playerNum].gameObject.SetActive(true);
        
        //나중에 두번째 말도 움직일 수 있을 때 테스팅
        if (unitPanel.canBoard)
        {
            unitPanel.canBoard = false;
        }

    }

}
