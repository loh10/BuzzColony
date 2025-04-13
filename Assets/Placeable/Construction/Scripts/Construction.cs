using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Construction : Recoltable
{
    public ConstructionSO constructionData;
    public bool isBuild;
    public bool isPlace;
    public float rangeBuild;
    private float timer;
    private GameObject colon;
    public Color ColorInBuild;
    private Light2D[] _lights;

    private void Start()
    {
        timer = constructionData.constructionTime;
        ColorSwipe(ColorInBuild);
        if(_lights == null)
            _lights = GetComponentsInChildren<Light2D>();
        SwitchLight(false);
        SwitchAlpha(0.25f);
    }

    private void Update()
    {
        if (!isBuild && ColonNear() && isPlace)
        {
            if (timer <= 0)
            {
                isBuild = true;
                isPlace = false;
                colon.GetComponent<ChoiceState>().isWorking = false;
                ColorSwipe(Color.white);
                SwitchLight(true);
            }
            else
            {
                timer -= Time.deltaTime;
                float alpha = 1 - (timer / constructionData.constructionTime);
                SwitchAlpha(alpha);
            }
        }
    }
    bool ColonNear()
    {
        Transform colonList = GameObject.Find("ColonMine").GetComponent<Transform>();
        foreach (Transform colonIsMine in colonList.GetComponentsInChildren<Transform>())
        {
            if (colonIsMine.GetComponent<Agent>() && Vector3.Distance(colonIsMine.position, transform.position) < rangeBuild)
            {
                colon = colonIsMine.gameObject;
                return true;
            }
        }
        return false;
    }

    private void ColorSwipe(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.color = color;
        }
    }

    private void SwitchLight(bool isOn)
    {
        foreach (Light2D light in _lights)
        {
            light.gameObject.SetActive(isOn);
        }
    }

    private void SwitchAlpha(float alpha)
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = spriteRenderer.color;
            color.a = 0.25f + (alpha * 0.8f);
            spriteRenderer.color = color;
        }
    }
}