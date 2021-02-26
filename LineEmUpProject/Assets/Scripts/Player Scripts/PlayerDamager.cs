using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage the player when an ememy hits it
/// </summary>
public class PlayerDamager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //on the enemy physics layer
        {
            Death();
            enabled = false;
        }
    }

    void Death() 
    {
        ScoreSceneSwitcher.SwitchToEndScreen();
    }
}
