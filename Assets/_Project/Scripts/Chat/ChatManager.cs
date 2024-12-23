using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
// ChatClient
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ChatAuthValues = Photon.Chat.AuthenticationValues;

// Photon Chat 사용
// 1. IChatClientListner 인터페이스 구현
public class ChatManager : MonoBehaviour, IChatClientListener
{
	public static ChatManager Instance { get; private set; }

	public JoinUI joinUI;
	public ChatUI chatUI;

	private ChatClient client;

	public ChatState state = 0;

	public string currentChannel;

	private void Awake()
	{
		Instance = this;
	}

	// 2. ChatClient 생성
	private void Start()
	{
		client = new ChatClient(this);
	}

	// 3. Update에서 Service를 호출
	private void Update()
	{
		client.Service();
	}

	public void SetNickname(string nickname)
	{
		// PhotonNetwork.NickName = nickname;
		client.AuthValues = new ChatAuthValues(nickname);
	}

	// PhotonServerSettings를 사용하여 접속할 경우
	public void ConnectUsingSettings()
	{
		AppSettings appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
		// ChatAppSettings chatSettings = new ChatAppSettings
		// {
		// 	AppIdChat = AppSettings.AppIdChat
		//	...
		// };
		// 위처럼 안해도 이렇게 할 수 있는 확장 메서드가 있다.
		ChatAppSettings chatSettings = appSettings.GetChatSettings();
		client.ConnectUsingSettings(chatSettings);
	}

	// 기본적으로 AppId를 통해 접속할 경우
	public void ConnectUsingAppId()
	{
		string chatId = "2d87970a-3d37-47c0-8b8b-0c5d335ed577";
		client.Connect(chatId, "1.0", client.AuthValues);
	}

	// 특정 채팅방(채팅 채널)에서 채팅 시작
	public void ChatStart(string roomName)
	{
		client.Subscribe(new string[] { roomName });
	}

	// 채팅 메시지 전송
	public void SendChatMessage(string message)
	{
		client.PublishMessage(currentChannel, message);
	}

	public void OnChatStateChange(ChatState state)
	{
		if (this.state != state)
		{
			print($"Chat state changed: {state}");
			this.state = state;
		}
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		currentChannel = channels[0];
		joinUI.gameObject.SetActive(false);
		chatUI.gameObject.SetActive(true);
		chatUI.roomNameLabel.text = channels[0];
		print($"채팅방 접속: {channels[0]}");
	}

	public void OnConnected()
	{
		joinUI.OnJoinedServer();
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		if (channelName != currentChannel)
		{
			print($"다른 채널의 메시지 수신함: {channelName}");
			return;
		}

		for (int i = 0; i < senders.Length; i++)
		{
			chatUI.ReceiveChatMessage(senders[i], messages[i].ToString());
		}
	}

	public void DebugReturn(DebugLevel level, string message)
	{
	}

	public void OnDisconnected()
	{
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
	}

	public void OnUnsubscribed(string[] channels)
	{
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
	}

	public void OnUserSubscribed(string channel, string user)
	{
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
	}
}