using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Awake()
    {
        TryGetComponent(out playerMovement);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            Debug.Log(gameObject.name);
            Debug.Log("Player1");
        }
    }

    private void SamePlayerCatch()
    {

    }

    private void OtherPlayerCatch()
    {

    }
}
