using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpFreeBlock : MonoBehaviour
{
    Transform playerY;
    Rigidbody2D playerRB;
    Collider2D colld;
    // Start is called before the first frame update
    void Start()
    {
        playerY = GameObject.Find("Player").transform;
        playerRB = playerY.gameObject.GetComponent<Rigidbody2D>();
        colld = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((playerY.position.y < transform.position.y) && colld.enabled)||Controll.GetDown())
        {
            colld.enabled = false;
        }
        if ((playerY.position.y > transform.position.y) && !colld.enabled && (playerRB.velocity.y < 0)&&!Controll.GetDown())
        {
            colld.enabled = true;
        }
    }
}
