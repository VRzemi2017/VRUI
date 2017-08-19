using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UniRx;
using UniRx.Triggers;

public class TCAServer : MonobitEngine.MonoBehaviour {
    [SerializeField]
    private byte maxPlayer = 2;
    [SerializeField]
    private int updateStreamRate = 60;
    [SerializeField]
    private int sendRate = 30;
    [SerializeField]
    private float reconnectTime = 3;
	[SerializeField]
	private bool GUIDisplay = true;
	[SerializeField]
	private Color TestColor = Color.black;
    [SerializeField]
    private Messager message;
    [SerializeField]
    private bool offLine;

    const string SERVER_NAME = "TCA_SERVER";
    const string LOBBY_NAME = "TCA_LOBBY";
    const string ROOM_NAME = "TCA_MT_ROOM";

    private Subject<Unit> enterRoom = new Subject<Unit>();
    public IObservable<Unit> OnEnterRoom { get { return enterRoom; } }

    private static int playerNo = -1;
    public static int PlayerNo { get { return playerNo; } }

    private void Start() {
        if (offLine)
        {
            enterRoom.OnNext(Unit.Default);
        }
        else
        {
            MonobitNetwork.updateStreamRate = updateStreamRate;
            MonobitNetwork.sendRate = sendRate;

            MonobitNetwork.autoJoinLobby = true;

            if (!MonobitNetwork.inRoom)
            {
                ConnectServer();
            }
        }
    }

    private void OnDisconnectedFromServer()
    {
        SetMessage("Disconnected.");
	    Observable.Timer(System.TimeSpan.FromSeconds(reconnectTime)).Subscribe(_ =>
        {
            ConnectServer ();
        });

    }

	private void ConnectServer()
	{
        SetMessage("Connecting Server...");
	    MonobitNetwork.ConnectServer(SERVER_NAME);
	}

    private void OnJoinedLobby()
    {
        SetMessage("Enter Lobby.");

        MonobitEngine.RoomSettings settings = new MonobitEngine.RoomSettings();
        settings.maxPlayers = maxPlayer;
        settings.isVisible = true;
        settings.isOpen = true;
        MonobitEngine.LobbyInfo lobby = new MonobitEngine.LobbyInfo();
        lobby.Kind = LobbyKind.Default;
        lobby.Name = LOBBY_NAME;
        MonobitEngine.MonobitNetwork.JoinOrCreateRoom(LOBBY_NAME, settings, lobby);
    }

    private void OnJoinedRoom()
    {
        SetMessage("Enter Room.");

        playerNo = MonobitEngine.MonobitNetwork.playerCountInRoom - 1;
        Debug.Log("Player: " + (playerNo + 1) + "P");

        enterRoom.OnNext(Unit.Default);
    }

	private void OnGUI()
	{
	    if (GUIDisplay) 
	    {
	        GUI.color = TestColor;
	        GUILayout.Label (MonobitNetwork.isConnect ? "Connected." : "Disconnected.");
	        GUILayout.Label (MonobitNetwork.inRoom ? "In Room." : "Not In Room.");
	        GUILayout.Label (MonobitNetwork.isHost ? "Host." : "Not Host.");	
            GUILayout.Label ("Player: " + (playerNo + 1) + "P");	
	    }
	}
    

    private void SetMessage(string msg)
    {
        if (message)
        {
            message.SetMessage(msg); 
        }
    }
}
