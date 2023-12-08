using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButtonSetter : MonoBehaviour
{
    [SerializeField] private RectTransform buttonTrans;
    private Vector3 buttonPos = new Vector3(0.25f, -0.45f, 0);

    private void Update()
    {
        SetUp();
    }

    public void SetUp()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + buttonPos);
        buttonTrans.position = screenPosition;
    }
}
