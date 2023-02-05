using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    //最后把一些能用代码赋值的变量都用代码赋值
    public LayerMask ground;
    //之前叫moveSpeed，但是叫加速度更合适一点
    public float moveAcceleration;
    public float flyAcceleration;
    Animator anim;
    Rigidbody2D rb;
    public GameObject foot_coll;
    Collider2D coll;
    public Collider2D body_coll;
    public bool isOnGround;
    public float startTime;
    public int EnemyNum = 3;
    public PhysicsMaterial2D bounce;
    public PhysicsMaterial2D smooth;
    // 跳跃时间，在一段时间内加速到指定速度
    public float jumpDuration = 1.5f;
    public GameObject Platform;
    //用来查看一些变化
    public float temp;
    public bool isBound;
    public int chance;
    public int perlife;
    public float boundTime = 1.5f;
    public bool isHurt;
    public Canvas gameOver;
    // Start is called before the first frame update
    void Start()
    {
        coll = foot_coll.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        //mass = rb.mass;
        anim = GetComponent<Animator>();
        perlife = 2;
        chance = 2;
        
        //Debug.Log(boundTime * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (!IsOnGround())
        {
            anim.SetBool("onground", false);
        }
        if (isBound == true)
        {
            if (Time.time - startTime >= boundTime * Time.deltaTime)
            {
                isBound = false;
            }
        }
        else
        {
            Movement();
        }
        temp = rb.velocity.y;
    }


    // 原版游戏中角色在空中时只要有一个初速度，那么水平方向的速度就保持不变了
    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        

        // 已解决 BUG：角色在落到平台上的时候按住方向键可以跳跃
        //  已解决BUG：角色卡在平台边上的时候按住空格加方向键会不断抽搐
        if (IsOnGround())
        {

            //更换材质
            if(Platform.GetComponent<CompositeCollider2D>().sharedMaterial != smooth)
            {
                Platform.GetComponent<CompositeCollider2D>().sharedMaterial = smooth;
            }
            
            if (horizontal != 0)
            {
                if(Mathf.Abs(rb.velocity.x) <= 8)
                {
                    
                    rb.velocity = new Vector2(rb.velocity.x + horizontal * moveAcceleration * Time.deltaTime, 0);
                    
                    
                }
                else
                {
                    rb.velocity = new Vector2(horizontal *8, 0);
                }

                
            }
            else
            {
                if (rb.velocity.x > 0.1f || rb.velocity.x < -0.1f)
                {
                    if (rb.velocity.x > 0)
                    {

                        rb.velocity = new Vector2(rb.velocity.x - moveAcceleration * Time.deltaTime, 0);
                   

                    }

                    else if (rb.velocity.x < 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x + moveAcceleration * Time.deltaTime, 0);
                        
                    }
                }
                //人物静止在地面上的时候会平移，暂时不清楚原因，所以我直接暴力把它的velocity全都设置为0
                else
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }
            if(rb.velocity.x != 0)
            {
                anim.SetBool("running", true);
            }
            else
            {
                anim.SetBool("running", false);
            }
            
            anim.SetBool("onground", true);
        }
        else
        {
            //更换材质
            if(Platform.GetComponent<CompositeCollider2D>().sharedMaterial != bounce)
            {
                Platform.GetComponent<CompositeCollider2D>().sharedMaterial = bounce;
            }
        }
        if (Input.GetButton("Jump"))
        {
            anim.SetBool("onground", false);
            float Ytemp;
            // 一个横向的力
            
            if (rb.velocity.y >= 4f)
            {
                Ytemp = 4f;
            }
            else
            {
               Ytemp = rb.velocity.y + Time.deltaTime * flyAcceleration;
            }
            if (horizontal != 0)
            {
                //在这里写水平加速度

                //人物静止在地面上的时候会平移，暂时不清楚原因，所以我直接暴力把它的velocity全都设置为0

                if(Mathf.Abs(rb.velocity.x) <= 8)
                {
                    rb.velocity = new Vector2(rb.velocity.x + horizontal * moveAcceleration * Time.deltaTime, Ytemp);
                }
                else
                {
                    rb.velocity = new Vector2(horizontal * 8, Ytemp);
                }
                


                //rb.velocity = new Vector2(horizontal * Time.deltaTime * moveAcceleration, Ytemp);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, Ytemp);
            }
        }
        if (body_coll.IsTouchingLayers(ground) && !coll.IsTouchingLayers(ground))
        {
            isBound = true;
            startTime = Time.time;
        }
        if (horizontal != 0)
        {
            transform.localScale = new Vector3(horizontal, transform.localScale.y, transform.localScale.z);
        }
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPoint.x > Screen.width)
        {
            screenPoint.x = 0;
        }
        else if (screenPoint.x < 0)
        {
            screenPoint.x = Screen.width;
        }
        this.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        

    }


    public bool IsOnGround()
    {
        if(coll.IsTouchingLayers(ground)){
            return true;
        }
        else
        {
            return false;
        }
    }

    

    //做一个反弹的效果

    //
    //玩家踩到敌人或者碰到墙壁都要判定朝着初速度的方向反向反弹

    // 我如果也给这些敌人加上和平台一样的物理材质是不是就可以了？
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Enemy")
        {
           if(collision.gameObject.transform.position.y < transform.position.y)
            {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

                EnemyNum--;
                if(EnemyNum <= 0)
                {
                    //游戏结束
                    GameObject.Find("Panel").SetActive(true);
                }
                
                enemy.JumpOn();
                //collision.gameObject.SetActive(false);
                //消灭敌人
                rb.velocity = new Vector2(0,8);
            }
            else
            {
                //
                isHurt = true;
                if(perlife > 0)
                {
                    perlife--;
                }
                else
                {
                    if(chance > 0)
                    {
                        chance--;
                        perlife = 2;
                    }
                    else
                    {
                        //弹出一个窗口，返回主菜单
                        //游戏结束
                    }
                }
                
            }
        }      
    }

}
