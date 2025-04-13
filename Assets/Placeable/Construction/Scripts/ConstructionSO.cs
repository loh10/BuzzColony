using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Construction", menuName = "ScriptableObjects/Construction", order = 1)]
public class ConstructionSO : ScriptableObject
{
    public GameObject constructionPrefab;
    public int[] constructionCost;//BOIS puis STONE puis METAL
    public float constructionTime;//En seconde
    public Vector2 constructionSize; // x = width, y = height
    public string tag ;
}
