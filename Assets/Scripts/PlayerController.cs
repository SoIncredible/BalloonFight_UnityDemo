using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask ground;
    public float moveSpeed;
    public float flySpeed;
    Rigidbody2D rb;
    public Collider2D coll;
    float mass;
    public bool isOnGround;
    public float boundForce; //这是游戏角色被反弹的时候施加的力
    public float boundTime; //反弹时间；
    public float jumpDuration = 0.5f;
    public float velocityX;
    private bool isBound; //判断角色是否被弹回


    


    public int chance;
    public int perlife;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //mass = rb.mass;
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if(!isBound){
             Movement();
         }
         velocityX = rb.velocity.y;
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        // if (Input.GetButton("Jump")){
        //     rb.velocity = new Vector2(rb.velocity.x, flySpeed * Time.deltaTime);
        // }
        if (IsOnGround())
        {
            if (horizontal != 0)
            {
                rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
            }
        }
        if (horizontal != 0 && Input.GetButton("Jump"))
        {
            float temp = rb.velocity.y + Time.deltaTime * (flySpeed / jumpDuration);
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, temp);
        }else if(Input.GetButton("Jump")){
            Debug.Log("我这里没执行？");
            float temp = rb.velocity.y + Time.deltaTime * (flySpeed / jumpDuration);
            if(rb.velocity.y > flySpeed){
                temp = flySpeed;
            }
            rb.velocity = new Vector2(rb.velocity.x, temp);
        }
        if(faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, transform.localScale.y, transform.localScale.z);
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

        /*
        if (screenPoint.y > Screen.height)
        {
            screenPoint.y = 0;
        }
        else if (screenPoint.y < 0)
        {
            screenPoint.y = Screen.height;
        }
        */
        this.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        //this.transform.Translate(horizontal, 0, vertical);

    }


    public bool IsOnGround()
    {
        if(coll.IsTouchingLayers(ground)){
            Debug.Log("我碰到了地面！");
            return true;
        }
        return false;
    }
    //做一个反弹的效果
    //玩家踩到敌人或者碰到墙壁都要判定朝着初速度的方向反向反弹

    public void OnCollisionEnter2D(Collision2D collision)
    {
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
        
    //     //我想要实现的是：
    //     //在被反弹回来的过程中
    //     //我不能通过键盘控制角色的移动

    //     //在反弹过程中确实不能控制移动了，那如何确定反弹结束了呢
    //     //统一一下吧，被反弹的0.5秒内不能控制移动
        

    }

    IEnumerator freeBound(){
        Debug.Log("执行了");
        yield return new WaitForSeconds(0.5f);
        isBound = false;
    }
}
