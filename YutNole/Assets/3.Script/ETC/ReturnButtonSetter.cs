using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButtonSetter : MonoBehaviour
{
    // Button Position과 GameObject Position 맞춰주는 Script 
    [SerializeField] private RectTransform buttonTrans;
    public Vector3 resultButtonPos; // index pos 갖고오기
    public Vector3 characterButtonPos; // 윷을 던졌을 때 캐릭터에 뜨는 버튼 pos
    public Vector3 returnButtonPos = new Vector3(0.25f, -0.45f, 0); // Return Button

    private void Update()
    {
        SetUp();
    }

    public void SetUp()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + returnButtonPos);
        buttonTrans.position = screenPosition;
    }
}
