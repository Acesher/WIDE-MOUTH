using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson.PunDemos;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public string gameversion = "1";
    public GameObject player;
    private void Awake()
    {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients 
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //Start is called before the first frame update
    void Start()
    {
        //Authentication 
        Connect();
    }

    void Update()
    {

    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected) //Dang o lobby
        {
            PhotonNetwork.JoinRoom(PhotonNetwork.CountOfRooms.ToString());
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(); //state OnConnected to master (duoc goi 1 cach tu dong)
            PhotonNetwork.GameVersion = gameversion;
        }
    }
    //C# khong phan biet thu tu goi, ma thu tu goi do trang thai call back xu ly
    public override void OnConnectedToMaster() //callback: ham va phuong thuc duoc goi 1 cach tu dong dua vao trang thai
                                               //Dang o giai doan server
    {
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        PhotonNetwork.JoinRoom(PhotonNetwork.CountOfRooms.ToString());
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed: " + message);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom((PhotonNetwork.CountOfRooms + 1).ToString(), roomOptions);
    }

    /*
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //Thong bao + chan ket noi
        //PhotonNetwork.PlayerList.Length < 20 => Connect();
    }
    */

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom);
        PhotonNetwork.JoinRoom(PhotonNetwork.CountOfRooms.ToString());
    }

    //Instantiate - Tao nhan vat dong bo hoa
    public override void OnJoinedRoom() //Tao moi truong choi
    {
        Debug.Log("Client now join room: " + PhotonNetwork.CurrentRoom);
        GameObject currentPlayer = PhotonNetwork.Instantiate(player.name, new Vector3(PhotonNetwork.PlayerList.Length, 0, 0), Quaternion.identity);
        //Cac component dieu khien chi duoc bat khi dang la current player
        //O prefab thi tat het component nay
        currentPlayer.GetComponent<ThirdPersonUserControl>().enabled = true;
        currentPlayer.GetComponent<ThirdPersonUserControl>().enabled = true;
        //currentPlayer.GetComponent<PlayerCommand>().enabled = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause);
        //Connect();
    }
}
