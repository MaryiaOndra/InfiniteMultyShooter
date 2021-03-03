using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPunObservable
{
    [SerializeField] Transform bulletPos;

    CharacterController chController;

    float _speed = 7f;
    float _rotateSpeed = 120f;

    PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 _pos = transform.position;
            Quaternion _rot = transform.rotation;

            stream.Serialize(ref _pos);
            stream.Serialize(ref _rot);
        }
        else
        {
            Vector3 _pos = Vector3.zero;
            Quaternion _rot = Quaternion.identity;

            stream.Serialize(ref _pos);
            stream.Serialize(ref _rot);

            transform.position = _pos;
            transform.rotation = _rot;
        }
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        chController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float _horizontal = Input.GetAxis("Horizontal");
            float _vertical = Input.GetAxis("Vertical");

            transform.Rotate(Vector3.up, _horizontal * _rotateSpeed * Time.deltaTime);
            chController.SimpleMove(transform.forward * _vertical * _speed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                PhotonNetwork.Instantiate("Bullet", bulletPos.position, bulletPos.rotation);
            }
        }
    }

    [PunRPC]
    public void BulletHit() 
    {
        if (photonView.IsMine)
        {
            Debug.Log("BulletHit");
        }
    }
}
