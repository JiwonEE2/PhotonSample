using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
	private Animator anim;

	private Rigidbody rb;

	// 캐릭터가 쳐다볼 곳
	private Transform pointer;

	// 투사체가 발사될 곳
	private Transform shotPoint;

	private float hp = 100;
	private int shotCount = 0;

	// 이동 속도
	public float moveSpeed;

	// 투사체 발사 파워
	public float shotPower;

	// 체력을 표시할 text
	public Text hpText;

	// 발사 횟수를 표시할 text
	public Text shotText;
	public Bomb bombPrefab;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		pointer = transform.Find("PlayerPointer");
		shotPoint = transform.Find("ShotPoint");
		tag = photonView.IsMine ? "Player" : "Enemy";
	}

	private void Update()
	{
		hpText.text = hp.ToString();
		shotText.text = shotCount.ToString();

		if (false == photonView.IsMine) return;

		Move();

		if (Input.GetButtonDown("Fire1"))
		{
			//Fire();
			photonView.RPC("Fire", RpcTarget.All, shotPoint.position, shotPoint.forward);
			shotCount++;
			anim.SetTrigger("Attack");
		}
	}

	private void FixedUpdate()
	{
		if (false == photonView.IsMine) return;
		Rotate();
	}

	private void Move()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		rb.velocity = new Vector3(x, 0, z) * moveSpeed;

		// 움직이고 있을 때
		anim.SetBool("IsMoving", rb.velocity.magnitude > 0.01f);
	}

	private void Rotate()
	{
		Vector3 pos = rb.position;
		pos.y = 0;
		Vector3 forward = pointer.position - pos;
		rb.angularVelocity = Vector3.zero;
		rb.rotation = Quaternion.LookRotation(forward, Vector3.up);
	}

	private void Hit(float damage)
	{
		hp -= damage;
	}

	private void Heal(float amount)
	{
		hp += amount;
	}

	// fire를 통해 생성하는 bomb 객체는 "데드레커닝" (추측항법 알고리즘)을 통해 각 클라이언트들이 직접 생성하고,
	// Fire 함수를 호출받는 시점을 온라인에서 원격으로 호출받음. (Remote Procedure Call)
	[PunRPC]
	private void Fire(Vector3 shotPoint, Vector3 shotDir, PhotonMessageInfo info)
	{
		print($"Fire Procedure called by {info.Sender.NickName}");
		print($"my local time: {PhotonNetwork.Time}");
		print($"server time when procedure called: {info.SentServerTime}");

		// "지연보상": 추측항법을 위해 rpc를 받은 시점은 서버에서 호출된 시간보다 항상 늦기 때문에,
		// 해당 지연시간만틈 위치, 또는 연산량을 보정해 주어야 최대한 원격에서의 플레이가 동기화될 수 있음.

		// 보정해야할 지연 시간
		float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

		Bomb bomb = Instantiate(bombPrefab, shotPoint, Quaternion.identity);
		bomb.rb.AddForce(shotDir * shotPower, ForceMode.Impulse);
		bomb.owner = photonView.Owner;

		// 지연 보상 시작
		bomb.rb.position += bomb.rb.velocity * lag;
	}
}