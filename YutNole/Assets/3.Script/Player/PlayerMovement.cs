using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerPos;
    [SerializeField] private Transform currentPos;
    [SerializeField] private Transform targetPos;
    private Animator playerAni;

    public float speed = 0;

    private void Awake()
    {
        TryGetComponent(out playerAni);
        TryGetComponent(out playerPos);
        PlayerStart();
    }

    private void Update()
    {
        PlayerMove();
    }

    private void PlayerStart()
    {
        // Player가 판에 없을 때 포지션 지정, 말 생성은 UI에서 해야할 듯
        playerPos.position = currentPos.position;
    }

    private void PlayerMove()
    {
        // Player가 판에 있을 때 포지션 지정
        playerPos.position = Vector3.MoveTowards(playerPos.position, targetPos.position, Time.deltaTime * speed);
    }

    private void PlayerCatch()
    {
        // Player가 다른 Player 위치에 도착했을 때
    }

    private void PlayerFinish()
    {
        // Player가 골인 시
        gameObject.SetActive(false);
    }
}
