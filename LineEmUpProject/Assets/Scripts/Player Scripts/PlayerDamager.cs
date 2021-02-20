using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
