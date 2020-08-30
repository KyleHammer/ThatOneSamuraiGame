﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ButtonController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public GameObject menu;
    
    public void CloseMenu()
    {
        vcam.m_Priority = 0;
        menu.SetActive(false);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}