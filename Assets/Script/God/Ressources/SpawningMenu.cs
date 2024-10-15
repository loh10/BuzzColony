using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawningMenu : MonoBehaviour
{
    public List<GameObject> buttonRessources;
    private bool wasActive;
    [SerializeField]private TextMeshProUGUI textToChange;
    public GameObject spawnRessources;
    
    void Start()
    {
        EnableAll(false);
    }

    private void EnableAll(bool isActive)
    {
        foreach (GameObject button in buttonRessources)
        {
            button.SetActive(isActive);
        }
    }

    public void ButtonAppear()
    {
        DisableAllRessource();
        wasActive = !wasActive;
        EnableAll(wasActive);
        textToChange.text = wasActive ? "X" : "Ressources";
    }
    
    public void SpawnWood()
    {
        DisableAllRessource();
        spawnRessources.GetComponent<SpawningWood>().enabled = true;
    }
    public void SpawnRock()
    {
        DisableAllRessource();
        spawnRessources.GetComponent<SpawningRock>().enabled = true;
    }
    public void SpawnMeat()
    {
        DisableAllRessource();
        spawnRessources.GetComponent<SpawningMeat>().enabled = true;
    }

    private void DisableAllRessource()
    {
        spawnRessources.GetComponent<SpawningWood>().enabled = false;
        spawnRessources.GetComponent<SpawningRock>().enabled = false;
        spawnRessources.GetComponent<SpawningMeat>().enabled = false;
    }
}
