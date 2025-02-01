using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Tooltip("移动速度")]
	public float moveSpeed = 5;
	[Tooltip("跳跃初速度")]
	public float jumpSpeed = 8;
	[Tooltip("转身速度，数字越大转身越快")]
	public float rotateSpeed = 360;
	[Tooltip("摄像机物体，必填。移动方向是根据摄像机位置计算的")]
	public Transform transCam;

	[Tooltip("子弹预制体，子弹必须具有Rigidbody刚体组件")]
	public Rigidbody prefabShell;

	[Tooltip("发射子弹的初始速度")]
	public float fireForce = 15;

	Animator animator;
	Rigidbody rigid;
	
	// 发射子弹的起始位置
	public Transform firePos;

	// 是否接触地面
	bool isGrounded = false;

	// 转身量，前进量。根据输入和摄像机角度计算得出
	float turnAmount, forwardAmount;

	bool jump = false;
	float nextFireTime = 0;

	void Start()
    {
		animator = GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody>();
	}

    void Update()
    {
		InputMove();
		Action();

		animator.SetBool("IsGround", isGrounded);
		animator.SetFloat("Speed", forwardAmount);
	}

	void InputMove()
	{
		float h = 0;
		float v = 0;

		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");

		// 根据摄像机位置，对move变换
		Vector3 forward = (transform.position - transCam.position).normalized;
		Vector3 right = Quaternion.Euler(0, 90, 0) * forward;

		Vector3 move = v * Vector3.ProjectOnPlane(forward, Vector3.up) + h * Vector3.ProjectOnPlane(right, Vector3.up);

		if (move.magnitude > 1f)
		{
			move.Normalize();
		}

		move = transform.InverseTransformDirection(move);
		move = Vector3.ProjectOnPlane(move, Vector3.up);
		turnAmount = Mathf.Atan2(move.x, move.z);
		forwardAmount = move.z;

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			jump = true;
		}
	}

	private void FixedUpdate()
	{
		isGrounded = CheckGround();

		float verticalSpeed = rigid.velocity.y;
		if (jump)
		{
			verticalSpeed = jumpSpeed;
			jump = false;
		}

		transform.Rotate(0, turnAmount * rotateSpeed * Time.deltaTime, 0);
		
		rigid.velocity = transform.forward * forwardAmount * moveSpeed + transform.up * verticalSpeed;
	}


	bool CheckGround()
	{
		Vector3 p = transform.position - new Vector3(0, 0.03f, 0);
		bool ground = Physics.CheckBox(p, new Vector3(0.08f, 0.08f, 0.08f), Quaternion.identity, LayerMask.GetMask("Ground"));
		//Debug.DrawLine(p, p + new Vector3(0, -0.05f, 0), Color.red);
		//Debug.Log("ground: " + ground);
		return ground;
	}


	void Action()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (Time.time > nextFireTime)
			{
				Fire();
				nextFireTime = Time.time + 0.8f;
			}
		}
	}

	void Fire()
	{
		Rigidbody shellInstance = Instantiate(prefabShell, firePos.position, firePos.rotation);

		shellInstance.velocity = fireForce * firePos.forward;
		shellInstance.transform.Rotate(0, 180, 0);
		shellInstance.angularVelocity = new Vector3(0, 0, -20); 
		//float Ang = 10; 
		//Ang += Time.deltaTime * 100; 
		//shellInstance.transform.Rotate(new Vector3(0, Ang, 0), Space.Self);

		animator.SetTrigger("Attack");
	}

	public void Die()
	{
		gameObject.SetActive(false);
	}

	public void Revive()
	{
		gameObject.SetActive(true);
        SendMessage("ReviveHP");
    }
}
