using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MaxSpeed = 1;
    public float accelerate = 1;
    public float maxJumpStrengh = 1;
    public float maxJumpHold = 1;
    public Camera cam;
    [Header("Коэффициент для прекращения прыжка при отжатии кнопки")]
    public float kFall = 1;

    private float speed = 0;
    private bool grounded = false;
    private float jumpHold = 1;
    private Rigidbody2D physics;
    private bool freeJump = true;

    private void Start()
    {
        physics = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag == "ground")
        {
            grounded = true;
            freeJump = true;
            jumpHold = maxJumpHold;
        }
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Clamp(speed + Controll.GetHorizontalMove()* accelerate*Time.deltaTime,-MaxSpeed,MaxSpeed);
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (grounded && Controll.GetJumpStart())
        {
            physics.AddForce(new Vector2(0f, maxJumpStrengh));
            grounded = false;
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
        cam.transform.position= new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,cam.transform.position.z);
        if (transform.position.y < -200) transform.position = new Vector3(0,0,-0.2f);
    }


    public void MustJump(float strenghtJump)
    {
        //physics.AddForce(new Vector2(0f, strenghtJump));
        if (Controll.GetJumpHold()) strenghtJump = strenghtJump * 1.5f;
        physics.velocity = new Vector2(0f, strenghtJump);
        grounded = false;
        freeJump = false;
    }
}
