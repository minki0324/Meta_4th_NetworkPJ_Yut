using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomIndex : MonoBehaviour
{
    public int Random_Index;
    public int Button_Index;
    private bool developerMode = false;

    public GameObject canvas;
    public GameObject st_img;
    public GameObject end_img;
    public Button[] btns;
    [SerializeField] private List<Button> ActiveBtn;
    public bool right = false;

    #region Unity Callback
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            right = true;
            Onclick();
        }
    }
    #endregion

    public void Onclick()
    {
        int index = -1;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (btns[i] == transform)
            {
                index = i;
                break;
            }
        }

        ActiveBtn.Remove(btns[index]);
        btns[index].interactable = false;

        bool fin = false;


        if (Random_Index == Button_Index)
        {
            fin = true;
        }

        if(fin && right)
        {

        }
        right = false;
    }
}
