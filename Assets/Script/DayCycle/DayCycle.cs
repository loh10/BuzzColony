using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class DayCycle : MonoBehaviour
{
    
    public float speed;
    [SerializeField] private int _hourDay;
    [SerializeField] private int _hourNight;
    [SerializeField] private Transform _constructionTransform;
    [SerializeField] private bool _lightOn;
    private float _second, _minute, _day;
    private int _hour = 8;
    private Volume _volume;
    private Light2D[] _light;
    private bool isPass;
    private RessourceAct _ressourceAct;

    private void Start()
    {
        _ressourceAct = RessourceAct.Instance;
        _volume = GetComponent<Volume>();
    }

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
        _lightOn = !_lightOn;
        _light = _constructionTransform.GetComponentsInChildren<Light2D>();
        foreach (Light2D lightComponent in _light)
        {
            lightComponent.enabled = _lightOn;
        }
    }

    void CheckHour()
    {
        //night
        if (_hour >= _hourNight && _hour < _hourNight + 1)
        {
            _volume.weight = _minute / 60;
            if (!_lightOn && _minute > 45)
            {
                CheckLight();
            }
        }
        //_day
        else if (_hour >= _hourDay && _hour < _hourDay + 1)
        {
            _volume.weight = 1 - _minute / 60;
            isPass = false;
            if (_lightOn && _minute > 25)
            {
                CheckLight();
            }
        }

        if (_hour == 0 && _minute == 0 && !isPass)
        {
            isPass = true;
            _ressourceAct.ResetClick();
        }
    }

    private void Update()
    {
        Clock();
        CheckHour();

    }
}