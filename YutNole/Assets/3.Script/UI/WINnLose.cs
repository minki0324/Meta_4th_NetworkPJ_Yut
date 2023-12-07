using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WINnLose : MonoBehaviour
{

    [SerializeField] private Image Win_img;
    [SerializeField] private Image Lose_img;


    private void Start()
    {

        Win_img = transform.GetChild(0).GetComponent<Image>();
        Lose_img = transform.GetChild(1).GetComponent<Image>();

        Win_img.gameObject.SetActive(false);
        Lose_img.gameObject.SetActive(false);
    }

    public void Play_ImgAnimation()
    {
        if(GameManager.instance.isWin)
        {
            Win_img.gameObject.SetActive(true);
            Lose_img.gameObject.SetActive(true);
           
        }
    }



}
