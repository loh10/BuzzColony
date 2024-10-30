using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayCycle : MonoBehaviour
{
    public int hourDay, hourNight;
    float _second, _minute, _day;
    float _hour = 8;
    public float speed;
    public Volume volume;
    [SerializeField] private Transform constructionTransform;
    private Light2D[] _light;
    [SerializeField] private bool lightOn;


    void Clock()
    {
        _second += Time.deltaTime * speed;
        if (_second >= 60)
        {
            _second = 0;
            _minute++;
        }

        if (_minute >= 60)
        {
            _minute = 0;
            _hour++;
        }

        if (_hour >= 24)
        {
            _hour = 0;
            _day++;
        }
    }

    void CheckLight()
    {
        lightOn = !lightOn;
        _light = constructionTransform.GetComponentsInChildren<Light2D>();
        foreach (Light2D lightComponent in _light)
        {
            lightComponent.enabled = lightOn;
        }
    }
    void CheckHour()
    {
        //night
        if (_hour >= hourNight && _hour < hourNight + 1)
        {
            volume.weight = _minute / 60;
            if (!lightOn && _minute > 45)
            {
                CheckLight();
            }
        }
        //_day
        else if (_hour >= hourDay && _hour < hourDay + 1)
        {
            volume.weight = 1 - _minute / 60;
            if (lightOn && _minute > 25)
            {
                CheckLight();
            }
        }
    }

    private void Update()
    {
        Clock();
        CheckHour();
    }
}