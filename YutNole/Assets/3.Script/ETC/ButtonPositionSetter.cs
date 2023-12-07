using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPositionSetter : MonoBehaviour
{
    // Button Position과 GameObject Position 맞춰주는 Script, Button에 붙여주기
    public Transform target; // GameObject Position
    private RectTransform buttonTrans; // Button Position

    private string buttonName;
    private Vector3 targetPos;
    private Vector3 returnButtonPos = new Vector3(0.25f, -0.45f, 0); // Return Button

    private void Awake()
    {
        TryGetComponent(out buttonTrans);
        buttonName = gameObject.name;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        SetUp(buttonName);
    }

    public void SetUp(string buttonName)
    { // Button 생길 때 Setup 해주기
        switch (buttonName)
        {
            case "Return":
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
