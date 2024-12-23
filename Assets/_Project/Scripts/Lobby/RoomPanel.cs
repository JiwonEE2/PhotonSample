using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
// Player
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public enum Difficulty
{
	Easy = 0,
	Normal,
	Hard,
}

public class RoomPanel : MonoBehaviour
{
	public Text roomTitleText;

	public Difficulty roomDifficulty;

	public Dropdown difficultyDropdown;

	public Text difficultyText;

	public RectTransform playerList;
	public GameObject playerTextPrefab;

	public Dictionary<int, PlayerEntry> playerListDic = new Dictionary<int, PlayerEntry>();

	public Button startButton;
	public Button cancelButton;

	private Dictionary<int, bool> playersReady;

	private void Awake()
	{
		startButton.onClick.AddListener(StartButtonClick);
		cancelButton.onClick.AddListener(CancelButtonClick);
		difficultyDropdown.ClearOptions();
		foreach (object difficulty in Enum.GetValues(typeof(Difficulty)))
		{
			Dropdown.OptionData option = new Dropdown.OptionData(difficulty.ToString());
			difficultyDropdown.options.Add(option);
		}

		difficultyDropdown.onValueChanged.AddListener(DifficultyValueChange);
	}

	private void OnEnable()
	{
		// 유효성 검사
		if (false == PhotonNetwork.InRoom) return;

		roomTitleText.text = PhotonNetwork.CurrentRoom.Name;

		foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
		{
			// player 정보 객체 생성
			JoinPlayer(player);
		}

		// 해당 플레이어의 방장 여부를 확인하여 활성/비활성화
		// 방장일 때만 난이도, 게임 시작 패널 활성화.
		difficultyDropdown.gameObject.SetActive(PhotonNetwork.IsMasterClient);
		startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
	}

	private void OnDisable()
	{
		foreach (Transform child in playerList)
		{
			// 플레이어 리스트에 다른 객체가 있으면 일단 모두 삭제
			Destroy(child.gameObject);
		}
	}

	public void JoinPlayer(Player newPlayer)
	{
		PlayerEntry playerEntry = Instantiate(playerTextPrefab, playerList, false).GetComponent<PlayerEntry>();

		playerEntry.playerNameText.text = newPlayer.NickName;
		playerEntry.player = newPlayer;
		if (PhotonNetwork.LocalPlayer.ActorNumber != newPlayer.ActorNumber)
		{
			playerEntry.readyToggle.gameObject.SetActive(false);
		}
	}

	public void LeavePlayer(Player gonePlayer)
	{
		foreach (Transform child in playerList)
		{
			Player player = child.GetComponent<PlayerEntry>().player;
			if (player.ActorNumber == gonePlayer.ActorNumber)
			{
				Destroy(child.gameObject);
			}
		}
	}

	private void CancelButtonClick()
	{
		PhotonNetwork.LeaveRoom();
	}

	private void StartButtonClick()
	{
	}

	private void DifficultyValueChange(int arg0)
	{
	}
}