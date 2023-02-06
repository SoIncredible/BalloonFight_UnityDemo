using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Move : MonoBehaviour
{

	public Rigidbody2D rigidbody2D;

	// ��ҵ�������Ϣ
	private float moveHorizontal;
	private float moveVertical;
	private Vector2 movement;

	// �ƶ��ٶ�
	public float speed;

	void Start()
	{
		// ...��ʼ��ʡ��
	}

	void Update()
	{
		// ...��ȡ�������ʡ��
	}

	void FixedUpdate()
	{
		// ��ȡ�������
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

		// �����Ż�
		if (rigidbody2D.velocity.x < 0 && rigidbody2D.velocity.x > -6 && moveHorizontal < 0)
		{
			rigidbody2D.AddForce(movement * speed * 20f);
		}
		if (rigidbody2D.velocity.x < 6 && rigidbody2D.velocity.x > 0 && moveHorizontal > 0)
		{
			rigidbody2D.AddForce(movement * speed * 20f);
		}


		movement = new Vector2(moveHorizontal, moveVertical);
		// �����ʩ����
		rigidbody2D.AddForce(movement * speed * 10.0f);
	}

}
