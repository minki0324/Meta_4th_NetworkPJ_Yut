using UnityEngine;
using UnityEngine.Networking;
using Mirror;


public class SetGame : NetworkBehaviour
{
    private GameObject[] myObject; //0번 힐라 1번 매그
    private Transform[] targetPos; // 0123 -> P1 돌위치 / 4567 -> P2 돌위치
    //플레이어 1 ,2 에따라 어떤말 , 어떤위치에 생성할지 정해줘야함
    private void Start()
    {
        myObject = GameManager.instance.myObject;
        targetPos = GameManager.instance.targetPos;

        // isLocalPlayer 체크를 통해 로컬 플레이어인 경우에만 SpawnOB 메서드 호출
        if (isLocalPlayer)
        {
            SpawnOB(GM.instance.Player_Num);
        }
    }

    [Command]

    private void SpawnOB(Player_Num p)
    {
        // isLocalPlayer를 체크하는 대신 p가 자신의 플레이어 번호와 일치하는지 확인

        if (p == Player_Num.P1)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject myOb = Instantiate(myObject[0], targetPos[i].position, Quaternion.identity);

                // isServer 체크 제거
                NetworkServer.Spawn(myOb, connectionToClient);

                Debug.Log($"플레이어1 생성 {i}");
            }
        }
        else
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject myOb = Instantiate(myObject[1], targetPos[4 + j].position, Quaternion.identity);

                // isServer 체크 제거
                NetworkServer.Spawn(myOb, connectionToClient);

                Debug.Log($"플레이어2 생성 {j}");
            }
        }

    }
}