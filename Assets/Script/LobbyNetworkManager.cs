using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Photon.Pun.UtilityScripts;
using UnityEditor;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    [SerializeField] private TMP_InputField _roomInput,_nameInput;
    [SerializeField] private RoomItemUI _roomItemUIPrebfab;
    [SerializeField] private Transform _roomListParent;
    [SerializeField] private RoomItemUI _playerItemUIPrebfab;
    [SerializeField] private Transform _playerListParent;
    [SerializeField] private GameObject _loadingScreen, _roomScreen, _beginScreen;
    [SerializeField] private TextMeshProUGUI _statusField;
    [SerializeField] private Button _leaveRoomButton,_startButton;

    private List<RoomItemUI> _roomList = new List<RoomItemUI>();
    private List<RoomItemUI> _playerList = new List<RoomItemUI>();
    void Start()
    {
        if (PlayerPrefs.GetString("ID")!="")
        {
            Connect();
            _loadingScreen.SetActive(false);
        }

        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }


    }

    #region PhotonCallBacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        _loadingScreen.SetActive(false);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    }

    public override void OnJoinedLobby()
    {
        _statusField.text = "Lobby";
        _loadingScreen.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        _statusField.text = "Joined " + PhotonNetwork.CurrentRoom.Name + " Room";
        UpdatePlayerList();
        _roomScreen.SetActive(true);
        _loadingScreen.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        _statusField.text = "Lobby";
        UpdatePlayerList();
        _roomScreen.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnCreatedRoom()
    {
    }
    #endregion

    public void Connect()
    {
        if (PlayerPrefs.GetString("ID") != "")
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("ID");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
            _beginScreen.SetActive(false);
        }
        else
        {
            if (string.IsNullOrEmpty(_nameInput.text) == false)
            {
                string name = _nameInput.text + "#" + Random.Range(0, 5000);
                PlayerPrefs.SetString("ID", name);
                PhotonNetwork.NickName = name;
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
                _beginScreen.SetActive(false);
            }
        }


    }

    public void GameQuit()
    {
        Application.Quit();
        
    }

    private void OnApplicationQuit()
    {
        UpdatePlayerList();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        //Clear the current list of room
        for (int i = 0; i < _roomList.Count; i++)
        {
            Destroy(_roomList[i].gameObject);
        }
        _roomList.Clear();

        // Generate a new list with the updated info
        for (int i = 0; i < roomList.Count; i++)
        {
            //skip empty rooms
            if (roomList[i].PlayerCount == 0) { continue; }

            RoomItemUI newRoomItem = Instantiate(_roomItemUIPrebfab);
            newRoomItem.LobbyNetworkParent = this;
            newRoomItem.SetName(roomList[i].Name);
            newRoomItem.transform.SetParent(_roomListParent);
            newRoomItem.name = roomList[i].Name;
            _roomList.Add(newRoomItem);
            newRoomItem.SetPlayerCount(roomList[i].PlayerCount);
        }


    }


    public void UpdatePlayerList()
    {
        //Clear the current list of player
        for (int i = 0; i < _playerList.Count; i++)
        {
            Destroy(_playerList[i].gameObject);
        }
        _playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null) { return; }

        // Generate a new list of player
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            RoomItemUI newPlayerItem = Instantiate(_playerItemUIPrebfab);
            newPlayerItem.LobbyNetworkParent = this;
            newPlayerItem.transform.SetParent(_playerListParent);
            newPlayerItem.SetName(player.Value.NickName);
            _playerList.Add(newPlayerItem);
        }

        if (_playerList.Count == 2)
        {
            _startButton.interactable= true;
        }
        else
        {
            _startButton.interactable = false;
        }
    }


    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }


    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        _loadingScreen.SetActive(true);

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomInput.text) == false)
        {
            PhotonNetwork.JoinOrCreateRoom(_roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
            _roomInput.text = "";
            _loadingScreen.SetActive(true);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}
