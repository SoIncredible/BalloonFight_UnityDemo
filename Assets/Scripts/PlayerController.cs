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
    public 
    // Start is called before the first frame update
    void Start()
    {
        coll = foot_coll.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        //mass = rb.mass;
        anim = GetComponent<Animator>();
        
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
            
            if (rb.velocity.y >= 3.5f)
            {
                Ytemp = 3.5f;
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
                    rb.velocity = new Vector2(horizontal * 8, 0);
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

    public void SwitchAnim()
    {

    }

    //做一个反弹的效果

    //
    //玩家踩到敌人或者碰到墙壁都要判定朝着初速度的方向反向反弹

    public void OnCollisionEnter2D(Collision2D collision)
    {

        /*
        if(collision.gameObject.tag == "GameEdge")
        {
            //主要是顶部的边界
            rb.velocity = new Vector2(rb.velocity.x, -boundForce * Time.deltaTime);
            Debug.Log("碰到了边界！");
            isBound = true;
            StartCoroutine(freeBound());
        }
        //如果玩家碰到了敌人，并且敌人是在玩家的下面，那么玩家就能踩到他然后触发一段小跳反弹
        else if(collision.gameObject.tag == "Enemy" && collision.gameObject.transform.position.y < transform.position.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, boundForce * Time.deltaTime);
        }else if(transform.position.x < collision.gameObject.transform.position.x){

            rb.velocity = new Vector2(-boundForce * Time.deltaTime,rb.velocity.y);
            isBound = true;
            StartCoroutine(freeBound());
        }
        else if(transform.position.x > collision.gameObject.transform.position.x){
            rb.velocity = new Vector2(boundForce * Time.deltaTime, rb.velocity.y);
            isBound = true;
            StartCoroutine(freeBound());
        }
        
        */
    //     //我想要实现的是：
    //     //在被反弹回来的过程中
    //     //我不能通过键盘控制角色的移动

    //     //在反弹过程中确实不能控制移动了，那如何确定反弹结束了呢
    //     //统一一下吧，被反弹的0.5秒内不能控制移动
        

    }
    /*
    IEnumerator freeBound(){
        Debug.Log("执行了");
        yield return new WaitForSeconds(0.5f);
        isBound = false;
    }
    */
}
