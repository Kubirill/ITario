using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    public float jumpStrenght;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "foot")
        {
            //collision.GetComponent<PlayerMove>().MustJump(jumpStrenght);
            PlayerMove player= collision.GetComponentInParent<PlayerMove>();
            player.MustJump(jumpStrenght);
            player.ChangeEnemy();
            if (gameObject.GetComponentInParent<Enemys>() != null) gameObject.GetComponentInParent<Enemys>().Damage(1);

        }
    }

   
}
