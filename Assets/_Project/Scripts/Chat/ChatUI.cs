using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
	public Text roomNameLabel;
	public InputField messageInput;
	public Button sendButton;
	public RectTransform messageContent;
	public GameObject messageEntryPrefab;

	public string myNickname = "무명의 전사";

	private void Awake()
	{
		messageInput.onEndEdit.AddListener(x => SendChatMessage());
		sendButton.onClick.AddListener(SendChatMessage);
	}

	//메시지를 보낼 때 호출
	public void SendChatMessage()
	{
		string message = messageInput.text;
		if (string.IsNullOrEmpty(message)) return;

		// ChatManager에게 메시지 보내기 호출
		ChatManager.Instance.SendChatMessage(message);

		messageInput.text = "";
		// 엔터 누를 때마다 다시 활성화
		messageInput.ActivateInputField();
	}

	//메시지를 받을 때 호출
	public void ReceiveChatMessage(string nickname, string message)
	{
		var entry = Instantiate(messageEntryPrefab, messageContent);
		entry.transform.Find("Nickname").GetComponent<Text>().text = nickname;
		entry.transform.Find("Message").GetComponent<Text>().text = message;
	}
}