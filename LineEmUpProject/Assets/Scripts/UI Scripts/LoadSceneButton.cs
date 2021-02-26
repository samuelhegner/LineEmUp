using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneButton : Button
{
    [SerializeField] private string sceneName;
    public override void OnButtonPressed()
    {
        SceneManager.LoadScene(sceneName);
    }
}
