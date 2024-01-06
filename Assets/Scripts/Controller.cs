using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : SingletonMonoBehaviour<Controller>
{
    public List<Digit> digits;
    public int time;
    public Action LightsOn;
    public Action LightsOff;
    [SerializeField] TextMeshProUGUI secondsTxt;
    [SerializeField] Light light1;
    [SerializeField] Light light2;
    [SerializeField, Range(1, 100)] private float timeMultiplier = 1;
    public DateTime debugTime;

    public int CurrentTime {
        set {
            if (value != time) {
                time = value;
                SetupTime(time);
            }
        } 
    }
    void Start()
    {
        for (int i = 0; i < digits.Count; i++) {
            digits[i].myIndex = i;
            digits[i].SetupCars();
        }
        debugTime = new DateTime(2024,1,1,21,37,01);
    }

    void Update()
    {
        CurrentTime = GetTime();
    }

    public void TurnOnTheLights() {
        LightsOn.Invoke();
        light1.enabled = true;
        light2.enabled = true;

    }    
    
    public void TurnOffTheLights() {
        LightsOff.Invoke();
        light1.enabled = false;
        light2.enabled = false;
    }

    void SetupTime(int time) {
        int thousand = time / 1000;
        int hundred = (time / 100) % 10;
        int tens = (time / 10) % 10;
        int ones = (time % 10);

        digits[0].SetupDigit(thousand);
        digits[1].SetupDigit(hundred);
        digits[2].SetupDigit(tens);
        digits[3].SetupDigit(ones);
        if (time/100 >= 19 || time/100 <= 6) {
            TurnOnTheLights();
        } else {
            TurnOffTheLights();
        }
    }

    int GetTime() {
        DateTime currentTime = DateTime.Now;
        //DateTime currentTime = debugTime;
        secondsTxt.text = ":" + currentTime.Second;
        int hours = currentTime.Hour;
        int minutes = currentTime.Minute;
        int timeInt = hours * 100 + minutes;
        return timeInt;
    }
}
