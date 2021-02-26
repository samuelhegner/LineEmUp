using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// Destroy an enemy
/// </summary>
public class EnemyDestroyer : MonoBehaviour, IDamageable
{
    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void TakeDamage()
    {
        photonView.RPC("RPC_DestroySelf", RpcTarget.All);
    }

    [PunRPC]
    void RPC_DestroySelf()
    {
        if (!photonView.IsMine)
            return;

        PhotonNetwork.Destroy(gameObject);
    }
}
