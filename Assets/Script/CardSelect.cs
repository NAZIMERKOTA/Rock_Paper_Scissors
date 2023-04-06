using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardSelect : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    [SerializeField] GameObject _playerOneText, _playerOnePointText, _playerTwoText, _playerTwoPointText, _drawText, _restartButton, leaveButton;
    int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, _playerOnePoint = 0, _playerTwoPoint = 0;

    PhotonView pw;
    private void Start()
    {
        pw = GetComponent<PhotonView>();
        _playerOnePointText.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName + "=" + _playerOnePoint.ToString();
        _playerTwoPointText.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName + "=" + _playerTwoPoint.ToString();

    }

    [PunRPC]
    public void SelectCard(int index)
    {
        if (index == 1)
        {
            //taþ
            a++;
        }
        else if (index == 2)
        {
            //kaðýt
            b++;
        }
        else if (index == 3)
        {
            //makas
            c++;
        }
        else if (index == 4)
        {
            //taþ
            d++;

        }
        else if (index == 5)
        {
            //kaðýt
            e++;
        }
        else
        {
            //makas
            f++;
        }


        if (a == 1 && d == 1)
        {
            Draw();
        }
        else if (a == 1 && e == 1)
        {
            PlayerTwoWin();
        }
        else if (a == 1 && f == 1)
        {
            PlayerOneWin();
        }
        else if (b == 1 && d == 1)
        {
            PlayerOneWin();
        }
        else if (b == 1 && e == 1)
        {
            Draw();
        }
        else if (b == 1 && f == 1)
        {
            PlayerTwoWin();
        }
        else if (c == 1 && d == 1)
        {
            PlayerTwoWin();
        }
        else if (c == 1 && e == 1)
        {
            PlayerOneWin();
        }
        else if (c == 1 && f == 1)
        {
            Draw();
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
    }

    void PlayerOneWin()
    {
        _playerOneText.SetActive(true);
        _playerOneText.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName + "Win";
        _restartButton.SetActive(true);
        leaveButton.SetActive(true);
        _playerOnePoint++;
        _playerOnePointText.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName + "=" + _playerOnePoint.ToString();
    }

    void PlayerTwoWin()
    {
        _playerTwoText.SetActive(true);
        _playerTwoText.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName + "Win";
        _restartButton.SetActive(true);
        leaveButton.SetActive(true);
        _playerTwoPoint++;
        _playerTwoPointText.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName + "=" + _playerTwoPoint.ToString();

    }

    void Draw()
    {
        _drawText.SetActive(true);
        _drawText.GetComponent<TextMeshProUGUI>().text = "Draw";
        _restartButton.SetActive(true);
        leaveButton.SetActive(true);
    }

    public void ResetGame()
    {
        pw.GetComponent<PhotonView>().RPC("RestartGame", RpcTarget.All, null);
    }

    public void leavedRoom()
    {
        PhotonNetwork.LoadLevel(0);
       // pw.GetComponent<PhotonView>().RPC("LeavingRoom", RpcTarget.All, null);
    }


    [PunRPC]
    void RestartGame()
    {
        a = 0;
        b = 0;
        c = 0;
        d = 0;
        e = 0;
        f = 0;
        _playerTwoText.SetActive(false);
        _playerOneText.SetActive(false);
        _drawText.SetActive(false);
        _restartButton.SetActive(false);
        leaveButton.SetActive(false);
    }


    [PunRPC]
    void LeavingRoom()
    {
        PhotonNetwork.LeaveRoom();

    }



}
