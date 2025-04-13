using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticMap : MonoBehaviour
{
    private bool isMapViewer = true;
    [SerializeField]private GameObject mapViewer,squareVisualizer;
    

    private void Start()
    {
        mapViewer.SetActive(false);
    }

    public void MapViewer()
    {
        mapViewer.SetActive(isMapViewer);
        isMapViewer = !isMapViewer;
        squareVisualizer.SetActive(isMapViewer);
    }
}
