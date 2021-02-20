using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSceneSwitcher : MonoBehaviour
{
    private static string endScreenName = "End Scene";
    public static void SwitchToEndScreen() 
    {
        SceneManager.LoadScene(endScreenName);
    }
}
