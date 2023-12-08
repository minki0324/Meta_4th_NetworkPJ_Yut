using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Room_Manager : NetworkRoomManager
{
    public override void OnGUI()
    {
        
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();

        StartCoroutine(OnEnter_GameScene_Co());
    }

    private IEnumerator OnEnter_GameScene_Co()
    {
        yield return new WaitForSeconds(1f);

        if (Utils.IsSceneActive(GameplayScene))
        {
            Server_Manager server_Manager = FindObjectOfType<Server_Manager>();
            server_Manager.First_TurnSet();
        }
    }
}
