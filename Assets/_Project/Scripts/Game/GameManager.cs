using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public Transform playerPositions;
	public static bool isGameReady;

	private void Awake()
	{
		Instance = this;
	}

	// photon에서 컨트롤 동기화하는 방법
	// 1. 프리팹에 PhotonView 컴포넌트를 붙이고, PhotonNetwork.Instantiate를 통해 원격 클라이언트들에게도 동기화된 오브젝트를 생성하도록 함
	// 2. PhotonView가 Observing할 수 있도록 View 컴포넌트를 부착
	// 3. 내 View가 부착되지 않은 오브젝트는 내가 제어하지 않도록 예외처리를 반드시 할 것
	private IEnumerator Start()
	{
		// GameObject playerPrefab = Resources.Load<GameObject>("Player");
		// Instantiate(playerPrefab).name = PhotonNetwork.NickName;
		// Vector3 spawnPos = playerPositions.GetChild(Random.Range(0, playerPositions.childCount)).position;
		// PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity).name = PhotonNetwork.NickName;
		yield return new WaitUntil(() => isGameReady);
		yield return new WaitForSeconds(1f);

		// GetPlayerNumber 확장메서드 : 포톤 네트워크에 연결된 다른 플레이어들 사이에서 동기화된 플레이어 번호.
		// Actor Number와 다름. (Scene마다 선착순으로 0~플레이어 수만큼 부여됨)
		// GetPlayerNumber 확장메서드가 동작하기 위해서는 Scene에 PlayerNumbering 컴포넌트 필요
		int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
		Vector3 playerPos = playerPositions.GetChild(playerNumber).position;
		GameObject playerObj = PhotonNetwork.Instantiate("Player", playerPos, Quaternion.identity);
		playerObj.name = $"Player {playerNumber}";
	}
}