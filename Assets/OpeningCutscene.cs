﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class OpeningCutscene : MonoBehaviour
{
    PlayableDirector _cutsceneDirector;
    UnityEvent endCutscene;
    public SignalReceiver signalReceiver;

    // Start is called before the first frame update
    void Start()
    {
        _cutsceneDirector = GetComponent<PlayableDirector>();
        if (endCutscene == null) endCutscene = new UnityEvent();
        if (signalReceiver == null) signalReceiver = GetComponent<SignalReceiver>();
        AssignTargets();
    }

   

    void AssignTargets()
    {
        BindToTrack("Cinemachine Track", GameManager.instance.mainCamera.GetComponent<CinemachineBrain>());
        BindToTrack("Animation Track", GameManager.instance.playerController.gameObject.GetComponent<Animator>());
        //endCutscene.AddListener(GameManager.instance.playerController.gameObject.GetComponent<PlayerInputScript>().EnableInput);
        //signalReceiver.ChangeReactionAtIndex(1, endCutscene);
       // signalReceiver.AddEmptyReaction(endCutscene);
        
    }

    void BindToTrack(string trackName, Object val)
    {
        foreach (var playableAssetOutput in _cutsceneDirector.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName == trackName)
            {
                _cutsceneDirector.SetGenericBinding(playableAssetOutput.sourceObject, val);
                break;
            }
        }
    }

    public void ChangeCamPriority()
    {
        GameManager.instance.thirdPersonViewCamera.GetComponent<FreeLookAddOn>().SetPriority(15);
    } 

}