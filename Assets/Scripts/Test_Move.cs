using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Move : MonoBehaviour
{

	public Rigidbody2D rigidbody2D;

	// 玩家的输入信息
	private float moveHorizontal;
	private float moveVertical;
	private Vector2 movement;

	// 移动速度
	public float speed;

	void Start()
	{
		// ...初始化省略
	}

	void Update()
	{
		// ...获取玩家输入省略
	}

	void FixedUpdate()
	{
		// 获取玩家输入
		moveHorizontal = Input.GetAxis("Horizontal");
		moveVertical = Input.GetAxis("Vertical");
		if (rigidbody2D.velocity.x < 0 && moveHorizontal > 0)
		{
			rigidbody2D.AddForce(movement * speed * 30f);
		}
		if (rigidbody2D.velocity.x > 0 && moveHorizontal < 0)
		{
			rigidbody2D.AddForce(movement * speed * 30f);
		}

		// 加速优化
		if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -6 && moveHorizontal < 0)
		{
			rigidbody2D.AddForce(movement * speed * 20f);
		}
		if (rigidbody2D.velocity.x < 6 && rigidbody2D.velocity.x > 0 && moveHorizontal > 0)
		{
			rigidbody2D.AddForce(movement * speed * 20f);
		}


		movement = new Vector2(moveHorizontal, moveVertical);
		// 对玩家施加力
		rigidbody2D.AddForce(movement * speed * 10.0f);
	}

}
