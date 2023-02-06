using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerController02 : MonoBehaviour
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
    public int score = 0;
    TMP_Text ScoreText;
    TMP_Text ChanceText;
    TMP_Text LifeText;
    Vector2 point;
    private bool isDead;
    public GameObject random;
    public float speed;
    public Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        coll = foot_coll.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        //mass = rb.mass;
        anim = GetComponent<Animator>();
        perlife = 2;
        chance = 2;
        point = transform.position;
        score = 0;

        //Debug.Log(boundTime * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        isOnGround = IsOnGround();
        if (isDead == true || isHurt == true)
        {
            if (Time.time - startTime >= boundTime * Time.deltaTime)
            {

                isDead = false;
                isHurt = false;
            }
        }
        else
        {
            Movement();
        }
        GameIsOver();
    }


    // 原版游戏中角色在空中时只要有一个初速度，那么水平方向的速度就保持不变了
    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (rb.velocity.x > 0.1f || rb.velocity.x < -0.1f)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
        if (vertical != 0)
        {
            if (rb.velocity.y <= 4)
            {
                movement = new Vector2(0, 1);
                rb.AddForce(movement * speed * 20f);
            }

            
            if (horizontal != 0)
            {
                movement = new Vector2(horizontal, 0);
                if (rb.velocity.x <= 0 && horizontal > 0)
                {
                    rb.AddForce(movement * speed * 30f);
                }
                if (rb.velocity.x >= 0 && horizontal < 0)
                {
                    rb.AddForce(movement * speed * 30f);
                }
                // 加速优化
                if (rb.velocity.x <= 0 && rb.velocity.x >= -8 && horizontal < 0)
                {
                    rb.AddForce(movement * speed * 20f);
                }
                if (rb.velocity.x <= 8 && rb.velocity.x >= 0 && horizontal > 0)
                {
                    rb.AddForce(movement * speed * 20f);
                }

            }


        }
        if (horizontal != 0)
        {
            if (IsOnGround())
            {

                movement = new Vector2(horizontal, 0);
                if (rb.velocity.x <= 0 && horizontal > 0)
                {
                    rb.AddForce(movement * speed * 30f);
                }
                if (rb.velocity.x >= 0 && horizontal < 0)
                {
                    rb.AddForce(movement * speed * 30f);
                }
                // 加速优化
                if (rb.velocity.x <= 0 && rb.velocity.x >= -8 && horizontal < 0)
                {
                    rb.AddForce(movement * speed * 20f);
                }
                if (rb.velocity.x <= 8 && rb.velocity.x >= 0 && horizontal > 0)
                {
                    rb.AddForce(movement * speed * 20f);
                }
                


            }
        }
        // 已解决 BUG：角色在落到平台上的时候按住方向键可以跳跃
        //  已解决BUG：角色卡在平台边上的时候按住空格加方向键会不断抽搐


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
        if (coll.IsTouchingLayers(ground))
        {
            Platform.GetComponent<CompositeCollider2D>().sharedMaterial = smooth;
            anim.SetBool("onground", true);
            return true;
        }
        else
        {
            Platform.GetComponent<CompositeCollider2D>().sharedMaterial = bounce;

            anim.SetBool("onground", false);
            return false;
        }
    }
    //做一个反弹的效果
    //


    public void GameIsOver()
    {
        
        bool isover = true;
        var obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        if (random.GetComponent<RandomGenerate>().count <= 1)
        {
            foreach (GameObject child in obj)
            {
                if (child.gameObject.CompareTag("Balloon"))
                {
                    //
                    isover = false;
                    break;
                }


                //暂停游戏

            }
            if (isover)
            {
                gameOver.transform.GetChild(5).gameObject.SetActive(true);
                gameOver.transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "Your Score:" + score.ToString();
                gameOver.transform.GetChild(3).gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Balloon")
        {
            Destroy(collision.gameObject);
            ScoreText = gameOver.transform.GetChild(0).GetComponent<TMP_Text>();
            score += 500;
            ScoreText.text = "Score:" + score.ToString();
        }
    }

}
