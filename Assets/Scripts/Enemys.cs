using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
    public int hp=1;
    public Vector2 patrulPointOne;
    public Vector2 patrulPointTwo;
    public float speed;
    public bool leftOrientationSprite = true;
    public GameObject[] parts;

    private Transform player;

    private Animator anim;

    private void Start()
    {
        if (((speed>0)&&leftOrientationSprite) || ((speed < 0) && !leftOrientationSprite)) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        anim=gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void Update()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) < 15 && hp>0)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            if (((transform.position.x < patrulPointOne.x) && (speed < 0)) || ((transform.position.x > patrulPointTwo.x) && (speed > 0)))
            {
                speed = -speed;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public void Damage(int strenght)
    {
        hp = hp - strenght;
        if (hp <= 0) anim.SetTrigger("die");
        //GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        foreach (GameObject part in parts)   GameObject.Destroy(part);
    }
    public void Deatch()
    {
        
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != ("ground"))
        {
            speed = -speed;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (collision.gameObject.tag == ("ground")&& ((collision.contacts[0].point.y > transform.position.y - 0.4)|| (collision.contacts[collision.contactCount-1].point.y > transform.position.y - 0.4)))
        {
            if (collision.contacts[0].point.x > transform.position.x ) speed = -Mathf.Abs(speed);
            else speed = Mathf.Abs(speed);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
