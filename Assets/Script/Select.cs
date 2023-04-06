using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject _buttonOne,_buttonTwo,_buttonThree,selectGameObject;
    [SerializeField] Transform _endPoint,_buttonOneBeginPoint, _buttonTwoBeginPoint, _buttonThreeBeginPoint;
    [SerializeField] bool one, two, three;

    PhotonView pw;

    private void Start()
    {
        pw= GetComponent<PhotonView>();
    }

    public void ClickButtonOne()
    {
        _buttonOne.SetActive(true);
        _buttonTwo.SetActive(false);
        _buttonThree.SetActive(false);
        Move();
        if (PhotonNetwork.IsMasterClient)
        {

            selectGameObject.GetComponent<PhotonView>().RPC("SelectCard", RpcTarget.All, 1);
        }
        else
        {
            selectGameObject.GetComponent<PhotonView>().RPC("SelectCard", RpcTarget.All, 4);

        }
        _buttonOne.GetComponent<Button>().interactable= false;
    }

    public void ClickButtonTwo()
    {
        _buttonOne.SetActive(false);
        _buttonTwo.SetActive(true);
        _buttonThree.SetActive(false);
        Move();
        if (PhotonNetwork.IsMasterClient)
        {

            selectGameObject.GetComponent<PhotonView>().RPC("SelectCard", RpcTarget.All, 2);
        }
        else
        {
            selectGameObject.GetComponent<PhotonView>().RPC("SelectCard", RpcTarget.All, 5);

        }
        _buttonTwo.GetComponent<Button>().interactable = false;

    }

    public void ClickButtonThree()
    {
        _buttonOne.SetActive(false);
        _buttonTwo.SetActive(false);
        _buttonThree.SetActive(true);
        Move();
        if (PhotonNetwork.IsMasterClient)
        {

            selectGameObject.GetComponent<PhotonView>().RPC("SelectCard", RpcTarget.All, 3);
        }
        else
        {
            selectGameObject.GetComponent<PhotonView>().RPC("SelectCard", RpcTarget.All, 6);

        }
        _buttonThree.GetComponent<Button>().interactable = false;

    }


    void Move()
    {
        transform.DOMove(_endPoint.position, 2f);
    }

    public void BackMove()
    {
       pw.GetComponent<PhotonView>().RPC("BackMoved", RpcTarget.All, null);
    }

    [PunRPC]
    void BackMoved()
    {
        _buttonOne.SetActive(true);
        _buttonTwo.SetActive(true);
        _buttonThree.SetActive(true);
        _buttonOne.transform.DOMove(_buttonOneBeginPoint.position, 2f);
       _buttonTwo.transform.DOMove(_buttonTwoBeginPoint.position, 2f);
       _buttonThree.transform.DOMove(_buttonThreeBeginPoint.position, 2f);
        _buttonOne.GetComponent<Button>().interactable = true;
        _buttonTwo.GetComponent<Button>().interactable = true;
        _buttonThree.GetComponent<Button>().interactable = true;

    }



}
