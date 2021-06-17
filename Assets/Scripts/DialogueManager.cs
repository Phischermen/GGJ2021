﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager singleton;
    public Text textDisplay;
    public Image portraitDisplay;
    public Message[] sentences;
    public Canvas canvas;

    private WaitForSeconds waitForSeconds;
    private WaitUntil waitUntilDismissed; 
    public Coroutine TypingRoutine;
    public enum Messages
    {
        gameStart, //in
        sailTorn, //in
        rudderBroken, //in
        lanternOut, //in
        windPickingUp, 
        wavePickingUp,
        brokenHullReminder,
        steeringAwayFromWheel, //in
        raisingSailAwayFromSail,
        captainPassesOut, //in
        getHitWhileAwayFromTheWheel,
        BrokenSailReminder,
        objectiveReminder,
        directionCallNorth, //in
        directionCallSouth, //in
        directionCallEast, //in
        directionCallWest, //in
        directionCallNorthEast, //in
        directionCallNorthWest, //in
        directionCallSouthEast, //in
        directionCallSouthWest, //in
        wrongDirection, //in
        brokenLanternReminder
    }
    public float typingSpeed;
    private bool dismiss;
    private bool typing;

    [System.Serializable]
    public class Message
    {
        public string sentence;
        public Sprite portrait;
    }
    public void Awake()
    {
        singleton = this;
    }
    public void Start()
    {
        waitForSeconds = new WaitForSeconds(typingSpeed);
        waitUntilDismissed = new WaitUntil(() => { return dismiss == true; });
        canvas.enabled = false;
        textDisplay.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Dismiss();
        }
    }

    public bool DisplayMessage(Messages message)
    {
        if (typing == true) return false;
        var idx = (int)message;
        TypingRoutine = StartCoroutine(Type(idx));
        portraitDisplay.sprite = sentences[idx].portrait;
        return true;
    }


    IEnumerator Type(int index)
    {
        textDisplay.text = "";
        typing = true;
        canvas.enabled = true;
        foreach (char letter in sentences[index].sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return waitForSeconds;
            if (dismiss)
            {
                break;
            }
        }
        dismiss = false;
        textDisplay.text = sentences[index].sentence;
        yield return waitUntilDismissed;
        typing = false;
        dismiss = false;
        canvas.enabled = false;
    }

    public void Dismiss()
    {
        if (typing == false) return;
        dismiss = true;
    }

}
