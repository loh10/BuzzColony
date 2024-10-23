using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public class ColonManager : MonoBehaviour
{
    public List<GameObject> colonExist = new List<GameObject>();
    public GameObject colonPrefab;
    public Transform colonParent,constructionParent;
    public bool canSpawn;
    public float mintTimer,maxTimer;
    private GameObject _firstConstruction;
    private void Start()
    {
        StartCoroutine(SpawnColon());
    }
    
    IEnumerator SpawnColon()
    {
        if (constructionParent.childCount > 0 && colonExist.Count < constructionParent.childCount && colonParent.childCount < 4)
        {
            _firstConstruction = constructionParent.GetChild(0).gameObject;
            Vector2 spawnPosition = new Vector2(0,0);
            GameObject newColon = Instantiate(colonPrefab, spawnPosition, Quaternion.identity);
            colonExist.Add(newColon);
            newColon.GetComponent<Colon>()._constructionTarget = _firstConstruction;
            newColon.transform.SetParent(colonParent);
        }
        yield return new WaitForSeconds(Random.Range(mintTimer, maxTimer));
        StartCoroutine(SpawnColon());
    }
        
}
