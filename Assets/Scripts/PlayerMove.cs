using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public float MaxSpeed = 1;
    public float accelerate = 1;
    public float maxJumpStrengh = 1;
    public float maxJumpHold = 1;
    public Camera cam;
    [Header("Коэффициент для прекращения прыжка при отжатии кнопки")]
    public float kFall = 1;
    public int hp = 3;
    public int coins = 0;

    private float speed = 0;
    private float jumpHold = 1;
    private Rigidbody2D physics;
    private bool freeJump = true;
    private bool unmortal = false;
    private Animator anim;

    private void Start()
    {
        physics = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag == "ground")
        {
            
            freeJump = true;
            unmortal = false;
            anim.SetBool("OnGround", true);
            jumpHold = maxJumpHold;
        }
        
    }
   
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "ground")
        {
            
            anim.SetBool("OnGround", false);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.tag == "damageZone")&& !unmortal)
        {
            hp--;
            physics.velocity=new Vector2(-transform.localScale.x/Mathf.Abs(transform.localScale.x)*2, 10);
            unmortal = true;
            if (hp < 0) Deatch();
        }
        if (collision.transform.tag == "deatchZone")
        {
            Deatch();
        }
       
    }

    public void Deatch()
    {
        GameObject.Destroy(gameObject);
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        //speed = Mathf.Clamp(speed + Controll.GetHorizontalMove()* accelerate*Time.deltaTime,-MaxSpeed,MaxSpeed);
        //anim.SetFloat("speedPanda", speed);
        speed = Mathf.Lerp(speed, Controll.GetHorizontalMove() * MaxSpeed, 0.01f);
        anim.SetFloat("speedPanda", speed);
        if (speed < 0) transform.localScale= new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (anim.GetBool("OnGround") && Controll.GetJumpStart())
        {
            physics.AddForce(new Vector2(0f, maxJumpStrengh));
        }
        if ((Controll.GetJumpHold()) && (jumpHold > 0))
        {
            //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, maxJumpStrengh));
            //jumpHold = jumpHold - Time.deltaTime;
        }
        if (Controll.GetJumpStop()&& (physics.velocity.y>0) && (freeJump))
        {
            physics.AddForce(new Vector2(0f,-physics.velocity.y*kFall));
        }
        if (cam.transform.position.x<transform.position.x-1)        cam.transform.position= new Vector3(gameObject.transform.position.x-1, cam.transform.position.y,cam.transform.position.z);
        if (transform.position.y < -200) transform.position = new Vector3(0,0,-0.2f);
    }


    public void MustJump(float strenghtJump)
    {
        //physics.AddForce(new Vector2(0f, strenghtJump));
        if (Controll.GetJumpHold()) strenghtJump = strenghtJump * 1.5f;
        physics.velocity = new Vector2(0f, strenghtJump);
       
        freeJump = false;
    }
}
