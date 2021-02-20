using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Menu[] menus;

    public static MenuManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }

    public void OpenMenu(string menuName) 
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].MenuName == menuName)
            {
                menus[i].Open();
            }
            else if(menus[i].IsOpen) 
            {
                menus[i].Close();
            }
        }
    }

    public void OpenMenu(Menu menu) 
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].IsOpen)
            {
                menus[i].Close();
            }
        }
        menu.Open();
    }
}
