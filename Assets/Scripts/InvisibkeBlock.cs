using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibkeBlock : MonoBehaviour
{
    Transform playerY;
    Rigidbody2D playerRB;
    Sprite picture;
    Collider2D colld;
    // Start is called before the first frame update
    void Start()
    {
        playerY = GameObject.Find("Player").transform;
        playerRB = playerY.gameObject.GetComponent<Rigidbody2D>();
        colld = GetComponent<Collider2D>();
        picture = GetComponent<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerY.position.y < transform.position.y)&&!colld.enabled && (playerRB.velocity.y > 0))
        {
            colld.enabled = true;
        }
        if ((playerY.position.y > transform.position.y) && colld.enabled&&(playerRB.velocity.y<0)&&(GetComponent<SpriteRenderer>().sprite == null))
        {
            colld.enabled = false;
        }
    }
}
