using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Enemys enem;
    public GameObject triger;
    public GameObject jerjinsky;
    public float YcoordJump=0.2f;


    private int waitCount=0;
    private int waitCountDance=-4;
    private Animator anim;
    private int readyHP = 2;
    private bool biger = false;
    
    private float jumpTrigger = 15;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void SpawnTrigger(int speed)
    {
        waitCount++;
        if (waitCount >= 5)
        {
            waitCount = waitCount - 5;
            GameObject[] trigers = GameObject.FindGameObjectsWithTag("triggers");
            if (trigers.Length < 8-readyHP*2)
            {
                GameObject trash = Instantiate(triger, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f), Quaternion.identity);
                trash.GetComponent<Enemys>().speed = speed;
                trash.GetComponent<Enemys>().jumpTrigger = jumpTrigger;
                trash.GetComponent<Enemys>().YcoordJump = YcoordJump;
            }
            GameObject[] en = GameObject.FindGameObjectsWithTag("EnemyDown");
            /*if (en.Length < 6 - readyHP*2)
            {
                GameObject trash = Instantiate(jerjinsky, new Vector3(14, -2, transform.position.z - 0.01f), Quaternion.identity);
                trash.GetComponent<Enemys>().speed = -3;
                trash.GetComponent<Enemys>().patrulPointOne = new Vector2(-9, 0);
                trash.GetComponent<Enemys>().patrulPointTwo = new Vector2(13, 0);
            }*/
        }
            
        
        
    }
    public void Dancing()
    {
        waitCountDance++;
        if (waitCountDance >= 7)
        {
            waitCountDance = waitCount - 7;
            anim.SetTrigger("Dance");
        }
    }
    private void Update()
    {
        if (enem.hp <= readyHP)
        {
            enem.hp = readyHP;
            readyHP = readyHP - 1;
            anim.SetTrigger("Damage");
            enem.speed = 0;

        }
        if (biger && (transform.localScale.y < 1.5))
        {
            transform.localScale = transform.localScale + transform.localScale * 0.005f;
            if (transform.localScale.x >= 1.5)
            {
                biger = false;
                transform.localScale = Vector3.one * 1.5f;
            }
        }
        if ((transform.localScale.y>1.3)&& !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage")&&(enem.speed==0)) enem.speed=2;
    }
    public void NewPhase()
    {
        if (readyHP == 1)
        {
            biger = true;
        }
        if (readyHP == 0)
        {
            anim.SetBool("hat", true);
            jumpTrigger = 20;
            YcoordJump = -2.7f;
            GameObject[] trashs = GameObject.FindGameObjectsWithTag("triggers");
            foreach (GameObject trash in trashs)
            {
                trash.GetComponent<Enemys>().jumpTrigger = jumpTrigger;
                trash.GetComponent<Enemys>().YcoordJump = YcoordJump;
            }
        }
    }
}
