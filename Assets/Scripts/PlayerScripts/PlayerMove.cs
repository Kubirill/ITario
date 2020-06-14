using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public Collider2D small;
    public Collider2D big;
    public GameObject smallFoot;
    public GameObject bigFoot;
    public float MaxSpeed = 1;
    public float accelerate = 1;
    public float maxJumpStrengh = 1;
    public float maxJumpHold = 1;
    public Camera cam;
    public bool freezeCam=false;
    [Header("Коэффициент для прекращения прыжка при отжатии кнопки")]
    public float kFall = 1;
    public int hp = 3;
    public int coins = 0;
    public bool bigState = false;
    public bool active=false;

    private float speed = 0;
    private float jumpHold = 1;
    private Rigidbody2D physics;
    private bool freeJump = true;
    private bool unmortal = false;
    private Animator anim;
    private float maxLeftSpeed;
    private float maxRightSpeed;
    private Text textMoney;
    private string tube;
    private float startTube;
    private GameObject tp;

    private void Start()
    {
        physics = gameObject.GetComponent<Rigidbody2D>();
        textMoney = GameObject.Find("CoinCount").GetComponent<Text>();
        anim = GetComponent<Animator>();
        SetStopSpeed(true, true);
        SetStopSpeed(false, true);
        if (PlayerPrefs.HasKey("Hp"))
        {
            hp = PlayerPrefs.GetInt("Hp");
            coins= PlayerPrefs.GetInt("Coins");
            textMoney.text = "x " + coins.ToString();
            GUI g = GameObject.Find("BlackFon").GetComponent<GUI>();
            g.SetMark(hp);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if ((collision.transform.tag == "ground")&&!(anim.GetBool("OnGround")))
        {

            freeJump = true;
            unmortal = false;
            anim.SetBool("OnGround", true);
            jumpHold = maxJumpHold;

        }
        if (collision.transform.tag == "Money")
        {
            Time.timeScale = 0.1f;
            bigState = true;
            GameObject.Destroy(collision.gameObject);
            anim.SetBool("BigState", true);
            smallFoot.SetActive(false);
            bigFoot.SetActive(true);
            small.enabled = false;
            big.enabled = true;
            transform.position = transform.position + new Vector3(0,0.5f, 0);
        }
        }
        


        

    public void SetStopSpeed (bool isLeft,bool isMax)
    {
        if (transform.localScale.x < 0) isLeft = !isLeft;
        if (isLeft)
        {
            if (isMax) maxLeftSpeed = -Mathf.Abs(MaxSpeed);
            else maxLeftSpeed = 0;
        }
        else
        {
            if (isMax) maxRightSpeed = Mathf.Abs(MaxSpeed);
            else maxRightSpeed = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if ((collision.transform.tag == "Teleport") && anim.GetBool("OnGround") && Controll.GetDown()&&active)
        {
            active = false;
            tp = collision.gameObject;
            transform.position = new Vector3(tp.transform.position.x, transform.position.y, transform.position.z);
            physics.gravityScale = 0;

            big.enabled = false;
            small.enabled = false;
            foreach (Collider2D child in GetComponentsInChildren<Collider2D>())
            {
                child.enabled = false;
            }
            Debug.Log(tp.GetComponent<Teleport>().tube);
            Debug.Log(SetDirect(tp.GetComponent<Teleport>().tube));
            startTube = SetDirect(tp.GetComponent<Teleport>().tube);
            tube = tp.GetComponent<Teleport>().tube;
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
        if ((collision.transform.tag == "damageZone")&& !unmortal && Time.timeScale>0.5f)
        {
            
            unmortal = true;
            if (!bigState) Deatch();
            else
            {
                Time.timeScale = 0.1f;
                bigState = false;
                anim.SetBool("BigState", false);
                smallFoot.SetActive(true);
                bigFoot.SetActive(false);
                small.enabled = true;
                big.enabled = false;
            }
        }
        if (collision.transform.tag == "deatchZone")
        {
            Deatch();
            
        }
       
    }

    public void Deatch()
    {
        if (!anim.GetBool("Death"))
        {
            if (hp > 1)
            {
                PlayerPrefs.SetInt("Hp", hp - 1);
                PlayerPrefs.SetInt("Coins", coins);
            }
            else
            {
                PlayerPrefs.DeleteKey("Hp");
                PlayerPrefs.DeleteKey("Coins");

            }
            Time.timeScale = 0.01f;
            anim.SetBool("Death", true);
            physics.gravityScale = 0;
            small.enabled = false;
            foreach (Collider2D child in GetComponentsInChildren<Collider2D>())
            {
                child.enabled = false;
            }

        }
    }

    public void DownDeatchAnimation()
    {
        Time.timeScale = 1f;
        physics.velocity= (new Vector2(0, 9));
        //physics.AddForce(new Vector2(0, 300));
        physics.gravityScale = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //speed = Mathf.Clamp(speed + Controll.GetHorizontalMove()* accelerate*Time.deltaTime,-MaxSpeed,MaxSpeed);
        //anim.SetFloat("speedPanda", speed);
        if (!anim.GetBool("Death")&&active)
        {
            speed = Mathf.Clamp(Mathf.Lerp(speed, Controll.GetHorizontalMove() * MaxSpeed, 0.01f),maxLeftSpeed,maxRightSpeed);
            anim.SetFloat("speedPanda", speed);
            if (speed < 0) transform.localScale= new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(Mathf.Max(cam.transform.position.x-8, transform.position.x + speed * Time.deltaTime), transform.position.y, transform.position.z);
        
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
            if ((cam.transform.position.x<transform.position.x-1)&&!freezeCam)        cam.transform.position= new Vector3(gameObject.transform.position.x-1, cam.transform.position.y,cam.transform.position.z);
        }
        
        if (transform.position.y < -25) SceneManager.LoadScene(0);
        if (tube != "")
        {
            if (tube == "right")
            {
                transform.position = transform.position + new Vector3(Time.deltaTime, 0, 0);
                if (transform.position.x > startTube + 1.5)
                {
                    if (tp != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
            if (tube == "left")
            {
                transform.position = transform.position - new Vector3(Time.deltaTime, 0, 0);
                if (transform.position.x < startTube - 1.5)
                {
                    if (tp != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
            if (tube == "up")
            {
                transform.position = transform.position + new Vector3(0, Time.deltaTime, 0);
                if (transform.position.y > startTube + 1.5)
                {
                    if (tp != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
            if (tube == "down")
            {
                transform.position = transform.position - new Vector3(0,Time.deltaTime, 0);
                if (transform.position.y < startTube - 1.5)
                {
                    if (tp != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
        }
    }

    private float SetDirect(string direction)
    {
        if ((direction == "right") || (direction == "left"))
        {
            return transform.position.x;
        }
        else
        {
            return transform.position.y;
        }
    }

    private void ContinueAdvanture()
    {
        active = true;
        physics.gravityScale = 2;
        
        foreach (Collider2D child in GetComponentsInChildren<Collider2D>())
        {
            child.enabled = true;
        }
        big.enabled = false;
        small.enabled = false;
        if (bigState) big.enabled = true;
        else small.enabled = true;
        tube = "";
        SetStopSpeed(true, true);
        SetStopSpeed(false, true);
    }

    private void Teleportation()
    {
        Teleport trash = tp.GetComponent<Teleport>();
        freezeCam = trash.setFreezeCam;
        transform.position =new Vector3( trash.teleportTarget.x,trash.teleportTarget.y,transform.position.z);
        cam.transform.position=new Vector3(trash.targetCam.x, trash.targetCam.y, cam.transform.position.z);
        Debug.Log(trash.tubeExit);
        Debug.Log(SetDirect(trash.tubeExit));
        startTube = SetDirect(trash.tubeExit);
        tube = trash.tubeExit;
        
        
        tp = null;
    }

    public void Setmortal()
    {
        unmortal = false;
        Time.timeScale = 1f;
    }

    public void MustJump(float strenghtJump)
    {
        //physics.AddForce(new Vector2(0f, strenghtJump));
        if (Controll.GetJumpHold()) strenghtJump = strenghtJump * 1.5f;
        physics.velocity = new Vector2(0f, strenghtJump);
       
        freeJump = false;
    }

    public void ChangeCoin(int coinsCount)
    {
        coins = coins + coinsCount;
        textMoney.text ="x "+coins.ToString();
    }
}
