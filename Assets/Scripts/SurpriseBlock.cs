using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBlock : MonoBehaviour
{
    public int itemCount = 1;
    public GameObject item;
    public Sprite emptyImage;

    GameObject var;
    private Animator anim;

    private void Start()
    {
        anim=GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.tag == "Player") && (collision.contacts[0].point.y < transform.position.y-0.4)/*&&!anim.GetBool("use")&&!anim.GetBool("empty")*/)
        {
            anim.SetBool("use", true);

        }
    }

    public void UseDone()
    {
        itemCount = itemCount - 1;
        anim.SetBool("use", false);
        if (item != null)
        {
            if (itemCount <= 0)
            {
                anim.SetBool("empty", true);
                gameObject.GetComponent<SpriteRenderer>().sprite = emptyImage;
            }
        }
            
    }
    public void SpawnItem()
    {
        if (item == null)
        {
            bool playerBig = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().bigState;
            if (playerBig != false)
            {
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            if (item.name == "CoinsPosition")
            {
                var = Instantiate(item, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(Vector3.zero));
                var.GetComponentInChildren<Coin>().Collect();
            }
            if (item.name == "Money")
            {
                var = Instantiate(item, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(Vector3.zero));
                var.GetComponentInChildren<Coin>().Collect();
            }
        }
         
    }
    
}
