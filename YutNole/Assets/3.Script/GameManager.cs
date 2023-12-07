using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct PlayerStates
{
//으음..삭제쿠다사이
    public bool hasChance;

    public bool isUnit1_Alive;
    public bool isUnit2_Alive;
    public bool isUnit3_Alive;
    public bool isUnit4_Alive;

    public int GoalCount;

}



public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

   

    public bool isMyTurn;
    public bool isThrew = false;

    public bool hasChance;

    public bool isUnit1_Alive;
    public bool isUnit2_Alive;
    public bool isUnit3_Alive;
    public bool isUnit4_Alive;

    public int GoalCount;


    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }



}
