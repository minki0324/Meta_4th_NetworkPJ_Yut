using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Scene : MonoBehaviour
{
    [SerializeField] private Room_Player P1_Component;
    [SerializeField] private Room_Player P2_Component;
    [SerializeField] private Room_Manager manager;

    #region Unity Callback
    private void Start()
    {
        manager = FindObjectOfType<Room_Manager>();
    }

    private void Update()
    {
        Find_Player();
    }
    #endregion

    private void Find_Player()
    {
        if(manager.roomSlots.Count == 0)
        {
            return;
        }
        else if (manager.roomSlots.Count == 2)
        {
            P1_Component = manager.roomSlots[0].GetComponent<Room_Player>();
            P2_Component = manager.roomSlots[1].GetComponent<Room_Player>();
        }
        else if(manager.roomSlots.Count == 1)
        {
            P1_Component = manager.roomSlots[0].GetComponent<Room_Player>();
        }
        
    }

    public void OnReadyBtn_Click()
    {
        if(manager.roomSlots.Count == 1)
        {
            if(P1_Component.isLocalPlayer)
            {
                if(P1_Component.readyToBegin)
                {
                    P1_Component.CmdChangeReadyState(false);
                }
                else
                {
                    P1_Component.CmdChangeReadyState(true);
                }
            }
            else
            {
                return;
            }
        }
        else if(manager.roomSlots.Count == 2)
        {
            if (P1_Component.isLocalPlayer)
            {
                if (P1_Component.readyToBegin)
                {
                    P1_Component.CmdChangeReadyState(false);
                }
                else
                {
                    P1_Component.CmdChangeReadyState(true);
                }
            }
            else if (P2_Component.isLocalPlayer)
            {
                if (P2_Component.readyToBegin)
                {
                    P2_Component.CmdChangeReadyState(false);
                }
                else
                {
                    P2_Component.CmdChangeReadyState(true);
                }
            }
            else
            {
                return;
            }
        }
    }

    
}
