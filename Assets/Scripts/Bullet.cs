using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPunObservable
{
    Rigidbody bulletRb;
    PhotonView photonView;
    float shotForce = 25;

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
        bulletRb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        bulletRb.isKinematic = !photonView.IsMine;
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            bulletRb.AddForce(transform.forward * shotForce, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            var _view = collision.gameObject.GetComponent<PhotonView>();
            
            if (_view)
            {
                _view.RPC("BulletHit", RpcTarget.All);
                PhotonNetwork.Destroy(photonView);
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

    public PhotonView PhotonView => photonView;
}
