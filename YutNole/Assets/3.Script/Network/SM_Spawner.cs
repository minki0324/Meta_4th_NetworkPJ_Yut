using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SM_Spawner : NetworkBehaviour
{
    // 프리팹을 할당할 변수
    public GameObject serverManagerPrefab;

    [Command(requiresAuthority = false)]
    public void InitServer_Manager()
    {
        // 서버에서만 실행되도록
        if (isServer)
        {
            // Spawn 메서드를 사용하여 프리팹을 소환
            GameObject serverManager = Instantiate(serverManagerPrefab);
            NetworkServer.Spawn(serverManager);
        }
    }
}
