using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
	public InputField idInput;
	public InputField pwInput;
	public Button createButton;
	public Button loginButton;

	private void Awake()
	{
		loginButton.onClick.AddListener(OnLoginButtonClick);
	}

	private void OnLoginButtonClick()
	{
		string userNickname = idInput.text;
		PhotonNetwork.NickName = userNickname;
		PhotonNetwork.ConnectUsingSettings();
	}
}