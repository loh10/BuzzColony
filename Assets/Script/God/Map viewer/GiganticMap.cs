using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticMap : MonoBehaviour
{
    bool isMapViewer;
    [SerializeField]private GameObject mapViewer,squareVisualizer;
    

    private void Start()
    {
        mapViewer.SetActive(isMapViewer);
    }

    public void MapViewer()
    {
        mapViewer.SetActive(isMapViewer);
        isMapViewer = !isMapViewer;
        squareVisualizer.SetActive(isMapViewer);
    }
}
