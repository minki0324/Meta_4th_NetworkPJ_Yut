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
            obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);      //return 버튼 비활성화
            obj.transform.GetChild(1).gameObject.SetActive(false);      //골인 비활성화
        }

        foreach (GameObject obj in P2_Unit)
        {
            obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);      //return 버튼 비활성화
            obj.transform.GetChild(1).gameObject.SetActive(false);      //골인 비활성화
        }
    }
}
