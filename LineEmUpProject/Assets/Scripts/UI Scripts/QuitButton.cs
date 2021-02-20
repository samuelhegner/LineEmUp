using System.Collections;
using UnityEngine;


public class QuitButton : Button
{
    public override void OnButtonPressed()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}