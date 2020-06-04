using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator anim;
    public bool inBlock = false;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Collect();
        }
    }

    public void Destroy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInParent<PlayerMove>().coins++;
        GameObject.Destroy(gameObject);
    }

    public void Collect()
    {
        anim.SetTrigger("collect");
    }
    private void Update()
    {
        if (inBlock) Collect();
    }
}
