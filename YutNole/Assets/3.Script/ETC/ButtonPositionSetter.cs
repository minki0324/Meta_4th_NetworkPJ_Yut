using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPositionSetter : MonoBehaviour
{
    // Button Position�� GameObject Position �����ִ� Script, Button�� �ٿ��ֱ�
    public Transform target; // GameObject Position
    private RectTransform buttonTrans; // Button Position

    private string buttonName;
    private Vector3 targetPos;
    private Vector3 returnButtonPos = new Vector3(0.25f, -0.45f, 0); // Return Button
    private bool gameStart = false;

    private void Awake()
    {
        TryGetComponent(out buttonTrans);
        buttonName = gameObject.name;
    }
    private void OnEnable()
    {
        SetUp(buttonName);
    }
   

    private void OnDisable()
    {
        gameStart = true;
    }


    public void SetUp(string buttonName)
    { // Button ���� �� Setup ���ֱ�
        switch (buttonName)
        {
            case "Return 0":
            case "Return 1":
            case "Return 2":
            case "Return 3":
                targetPos = returnButtonPos;
                break;
            default:
                targetPos = Vector3.zero;
                break;
        }
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position + targetPos);
        buttonTrans.position = screenPosition;
    }
}
