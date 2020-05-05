using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    public float jumpStrenght;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("jump");
        if (collision.gameObject.tag == "foot")
        {
            //collision.GetComponent<PlayerMove>().MustJump(jumpStrenght);
            collision.GetComponentInParent<PlayerMove>().MustJump(jumpStrenght);
        }
    }

   
}
