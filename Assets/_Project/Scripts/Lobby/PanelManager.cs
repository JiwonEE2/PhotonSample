using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PanelManager : MonoBehaviourPunCallbacks
{
	public static PanelManager Instance;

	public LoginPanel login;
	public MenuPanel menu;
	public LobbyPanel lobby;
	public RoomPanel room;

	private Dictionary<string, GameObject> panelDic;

	private void Awake()
	{
		Instance = this;
		panelDic = new Dictionary<string, GameObject>
		{
			{ "Login", login.gameObject },
			{ "Menu", menu.gameObject },
			{ "Lobby", lobby.gameObject },
			{ "Room", room.gameObject }
		};

		PanelOpen("Login");
	}

	public override void OnEnable()
	{
		// MonoBehaviourPunCallbacks를 상속한 클래스는 OnEnable을 재정의할 때 꼭 부모의 OnEnable을 호출해야 함
		base.OnEnable();
		print("하이");
	}

	public void PanelOpen(string panelName)
	{
		foreach (var row in panelDic) row.Value.SetActive(row.Key == panelName);
	}

	// photon server에 접속되었을 때 호출
	public override void OnConnected()
	{
		PanelOpen("Menu");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		LogManager.Log($"로그아웃됨: {cause}");
		PanelOpen("Login");
	}

	// 방을 생성하였을 때 호출
	public override void OnCreatedRoom()
	{
		PanelOpen("Room");
	}

	// 방에 참여
	public override void OnJoinedRoom()
	{
		PanelOpen("Room");
	}

	// 방에서 떠났을 때 호출
	public override void OnLeftRoom()
	{
		PanelOpen("Menu");
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		room.JoinPlayer(newPlayer);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		room.LeavePlayer(otherPlayer);
	}

	public override void OnJoinedLobby()
	{
		PanelOpen("Lobby");
	}

	public override void OnLeftLobby()
	{
		PanelOpen(("Menu"));
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		lobby.UpdateRoomList(roomList);
	}
}