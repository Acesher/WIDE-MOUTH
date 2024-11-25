using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCommand : MonoBehaviourPun
{
    public GameObject instanceObj;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendCommand();
        }
    }

    public void SendCommand()
    {
        photonView.RPC("ReceiveCommand", RpcTarget.All, photonView.ViewID);
    }

    [PunRPC]
    public void ReceiveCommand(int pos)
    {
        Instantiate(instanceObj, new Vector3(pos - 1000, 0, 1), Quaternion.identity);
    }
}
