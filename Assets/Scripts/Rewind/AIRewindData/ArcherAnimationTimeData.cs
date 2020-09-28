﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationTimeData 
{
    public float currentFrame;
    public int currentClip;

    public ArcherAnimationTimeData(float _currentFrame, int _currentClip) 
    {
        currentFrame = _currentFrame;
        currentClip = _currentClip;
    }
}
