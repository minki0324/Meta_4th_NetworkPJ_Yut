using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonClick : MonoBehaviour
{
    // Player Canvas에 달려있는 버튼 목록
    [SerializeField] private GameObject[] characterButton;
    [SerializeField] private GameObject returnButton;

    public void CharacterButton()
    {
        returnButton.SetActive(true);
        // 생성
        foreach (GameObject g in characterButton)
        {
            g.SetActive(false);
        }
    }
    public void ReturnButton()
    {
        foreach (GameObject g in characterButton)
        {
            
            g.SetActive(true);
        }
        returnButton.SetActive(false);
    }
}
