using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Menu[] menus; //all menus that can be opened and closed

    public static MenuManager Instance; //Singleton for quick practice implementation

    void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// Open a menu and close the currently open menu
    /// </summary>
    /// <param name="menuName">Take in the name of the menu you want to open</param>
    public void SwitchMenu(string menuName)
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


    /// <summary>
    /// Open a menu and close the currently open menu
    /// </summary>
    /// <param name="menuName">Menu to open</param>
    public void SwitchMenu(Menu menu) 
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
