using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotonTest : MonoBehaviour
{
    public ClientState state = 0;

    private void Update()
    {
        if (PhotonNetwork.NetworkClientState != state)
        {
            state = PhotonNetwork.NetworkClientState;
            LogManager.Log($"state changed: {state}");
        }
    }
}