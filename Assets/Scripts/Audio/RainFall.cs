﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFall : MonoBehaviour
{

    private BackgroundAudio backgroundAudio;

    void Start()
    {
        backgroundAudio = GameManager.instance.audioManager.backgroundAudio;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            backgroundAudio.PlayRain();
        }
    }
}