using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
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
        if(SaveAndLoad.Instance)
        {
            if(SaveAndLoad.Instance._colon != null)
            {
                foreach (int index in SaveAndLoad.Instance._colon.Keys)
                {
                     GameObject loadedColon = Instantiate(colonPrefab,StringToVector2(SaveAndLoad.Instance._colon[index]),Quaternion.identity, colonParent.parent.GetChild(1));
                     colonExist.Add(loadedColon );
                     loadedColon.GetComponent<Colon>().isMine = true;
                     print("here");
                }
            }
        }
    }
    
    private Vector2 StringToVector2(string value)
    {
        value = value.Replace("(", "").Replace(")", "");
        string[] vector = value.Split(',');
        Vector2 vector2 = new Vector2(float.Parse(vector[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(vector[1], CultureInfo.InvariantCulture.NumberFormat));
        return vector2;
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
