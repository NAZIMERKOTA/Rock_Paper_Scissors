using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour
{
    public LobbyNetworkManager LobbyNetworkParent;
    [SerializeField] private TextMeshProUGUI _roomName,_playerCount;
    [SerializeField] private Button _joinButton;
    
    
    public void SetName(string roomName)
    {
        _roomName.text = roomName;
    }

    public void SetPlayerCount(int playerCount)
    {

        _playerCount.text = playerCount.ToString();

        if(2 == playerCount)
        {
            _joinButton.interactable= false;
        }
        else
        {
            _joinButton.interactable= true;
        }

    }

    public void OnJoinPressed()
    {
        LobbyNetworkParent.JoinRoom(_roomName.text);
        LobbyNetworkParent.UpdatePlayerList();

    }
}
