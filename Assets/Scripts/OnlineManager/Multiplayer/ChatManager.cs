using UnityEngine;
using System;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;

public class ChatManager : MonoBehaviour, IChatClientListener
{

    public GameObject CurrentPlayer;
    private string currentPlayerUsername;

    private ChatClient chatClient;

    public TMP_Text channelName;

    public TMP_InputField msgInput;
    public TextMeshProUGUI msgArea;

    private void Awake()
    {
        CurrentPlayer = GameObject.FindGameObjectWithTag("CurrentPlayer");
        currentPlayerUsername = CurrentPlayer.GetComponent<CurrentPlayer>().username;


        MultiplayerController.RoomJoined += HandleRoomJoined;
        MultiplayerController.RoomLeft += HandleRoomLeft;
    }

    private void Start()
    {
        chatClient = new ChatClient(this);

    }

    private void Update()
    {
        chatClient.Service();
    }

    private void HandleRoomJoined(string roomName)
    {
        channelName.text = roomName;
        connectToPhotonChat();

    }

    private void HandleRoomLeft()
    {
        chatDisconnected();
    }

    private void connectToPhotonChat()
    {
        Debug.Log("Connecting to Photon chat");
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(currentPlayerUsername));
    }

    public void sendDirectMessage()
    {
        if (msgInput.text.Length > 0)
        {
            chatClient.PublishMessage(channelName.text, msgInput.text);
            Debug.Log("What room am i in: " + channelName.text);
            msgInput.text = "";
        }
    }

    public void chatDisconnected()
    {
        Debug.Log($"Leaving this room chat{channelName.text}");
        msgInput.text = "";
        msgArea.text = "";
        chatClient.Unsubscribe(new string[] { channelName.text });
        chatClient.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"Photon Chat DebugReturn: {message}");
    }

    public void OnDisconnected()
    {
        Debug.Log("You have disconnected from the Photon Chat");
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { channelName.text });
        Debug.Log("Chat connected");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"Photon Chat OnChatStateChange: {state.ToString()}");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            if (senders[i] == currentPlayerUsername)
            {
                msgArea.text += "<color=red>" + senders[i] + ": " + "</color>" + messages[i] + "\n";
            }
            else
            {
                msgArea.text += "<color=blue>" + senders[i] + ": " + "</color>" + messages[i] + "\n";
            }

        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Photon Chat OnSubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"What Channel did I just sub: {channels[i]}");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        msgArea.text = "";
        msgInput.text = "";

        Debug.Log($"Photon Chat OnUnsubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"Unsub from: {channels[i]}");
        }

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log($"Photon Chat OnStatusUpdate: {user} changed to {status}: {message}");
        Debug.Log($"Status Update for {user} and its now {status}.");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserSubscribed: {channel} {user}");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"Photon Chat OnUserUnsubscribed: {channel} {user}");
    }
}
