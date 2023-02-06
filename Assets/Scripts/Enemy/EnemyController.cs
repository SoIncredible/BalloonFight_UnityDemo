using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    //实现敌人的随机飞行
    //敌人也要具有撞到平面被反弹的效果

    private Rigidbody2D rb;
    private Collider2D coll;
    public float Speed;
    public float TopY, BottomY;
    private bool isUP;
    public Transform top, bottom;
    public Animator anim;
    public bool isGUard;
    private void Start()
    {

        //TopY = GetComponentInChildren<>
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        BottomY = bottom.position.y;
        TopY = top.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        if (!isGUard)
        {
            if (isUP)
            {
                transform.position += new Vector3(0, 0.05f, 0);
                if (transform.position.y >= TopY)
                {
                    isUP = false;
                }

            }
            else
            {
                transform.position += new Vector3(0, -0.05f, 0);
                if (transform.position.y <= BottomY)
                {
                    isUP = true;
                }
            }
        }
        
    }
    void Death()
    {
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        anim.SetTrigger("death");
        
    }
    void SetColliderFalse()
    {
        coll.enabled = false;
    }

}
