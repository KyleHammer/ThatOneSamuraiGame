﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RewindManager : MonoBehaviour
{

    public delegate void StepBackEvent();
    public event StepBackEvent StepBack;

    public delegate void StepForwardEvent();
    public event StepForwardEvent StepForward;

    public float rewindDirection;
    public bool isTravelling = false;

    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(.05f);

    public List<RewindEntity> rewindObjects;

    public delegate void ResetEvent();
    public event ResetEvent Reset;

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator RewindCoroutine()
    {
        if (isTravelling && rewindDirection < 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepBack != null) StepBack();
            // Debug.Log(Time.timeScale);
        }

        else if (isTravelling && rewindDirection > 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepForward != null) StepForward();

        }

        if (isTravelling && rewindDirection == 0)
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        if (!isTravelling && Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        yield return null;

        StartCoroutine(RewindCoroutine());

    }

    public void StartRewind()
    {
        if (isTravelling)
        {
            foreach (RewindEntity entity in rewindObjects) 
            {
                entity.isTravelling = true;
            }
            StartCoroutine("RewindCoroutine");
        }

    }

    public void EndRewind() 
    {
        if (isTravelling)
        {
            isTravelling = false;
            foreach (RewindEntity entity in rewindObjects)
            {
                StopAllCoroutines();
              //  Debug.LogError("BREAK");
                entity.ApplyData();
                entity.isTravelling = false;
            }
            Reset();
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }

    
}