using Photon.Pun;
using System.Collections;
using UnityEngine;


public class EnemyDestroyer : MonoBehaviour, IDamageable
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public void TakeDamage()
    {
        PV.RPC("RPC_DestroySelf", RpcTarget.All);
    }

    [PunRPC]
    void RPC_DestroySelf()
    {
        if (!PV.IsMine)
            return;

        PhotonNetwork.Destroy(gameObject);
    }
}
