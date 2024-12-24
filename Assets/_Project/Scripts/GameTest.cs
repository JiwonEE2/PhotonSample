using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameTest : MonoBehaviour
{
	private void Start()
	{
		LogManager.Log(PhotonNetwork.NickName);
		LogManager.Log(PhotonNetwork.IsConnected.ToString());
	}
}