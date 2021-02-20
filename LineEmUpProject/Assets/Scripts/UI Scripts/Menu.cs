using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string MenuName { get => menuName; }
    public bool IsOpen { get => isOpen;}

    [SerializeField] private string menuName;
    [SerializeField] private bool isOpen;

    public void Open() 
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    public void Close() 
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
