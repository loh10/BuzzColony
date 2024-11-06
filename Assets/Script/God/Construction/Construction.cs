using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public ConstructionSO constructionData;
    public bool isBuild;
    public bool isPlace;
    public float rangeBuild;
    private float timer;

    private void Start()
    {
        timer = constructionData.constructionTime;
    }

    private void Update()
    {
        if (!isBuild && ColonNear() && isPlace)
        {
            if (timer <= 0)
            {
                isBuild = true;
                isPlace = false;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    bool ColonNear()
    {
        Transform colon = GameObject.Find("ColonMine").GetComponent<Transform>();
        foreach (Transform colonIsMine in colon.GetComponentsInChildren<Transform>())
        {
            if (colonIsMine.GetComponent<Agent>() && Vector3.Distance(colonIsMine.position, transform.position) < rangeBuild)
            {
                return true;
            }
        }

        return false;
    }
}