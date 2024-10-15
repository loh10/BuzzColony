using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawningMenu : MonoBehaviour
{
    public List<GameObject> buttonRessources;
    private bool isActive;
    [SerializeField] private TextMeshProUGUI textToChange;
    [SerializeField] private ConstructionMenu _constructionMenu;
    public GameObject spawnRessources;


    void Start()
    {
        EnableAll(false);
    }

    private void EnableAll(bool wasActive)
    {
        foreach (GameObject button in buttonRessources)
        {
            button.SetActive(wasActive);
        }
    }

    public void ButtonAppear()
    {
        UnactiveScripts();
        CloseOtherMenu();
        isActive = !isActive;
        textToChange.text = isActive ? "X" : "Ressources";
        EnableAll(isActive);
    }

    public void SpawnWood()
    {
        UnactiveScripts();
        spawnRessources.GetComponent<SpawningWood>().enabled = true;
    }

    public void SpawnRock()
    {
        UnactiveScripts();
        spawnRessources.GetComponent<SpawningRock>().enabled = true;
    }

    public void SpawnMeat()
    {
        UnactiveScripts();
        spawnRessources.GetComponent<SpawningMeat>().enabled = true;
    }

    private void UnactiveScripts()
    {
        spawnRessources.GetComponent<SpawningWood>().enabled = false;
        spawnRessources.GetComponent<SpawningRock>().enabled = false;
        spawnRessources.GetComponent<SpawningMeat>().enabled = false;
    }

    public void DisableAllRessource()
    {
        EnableAll(false);
        UnactiveScripts();
        textToChange.text = "Ressources";
        isActive = false;
    }

    public void CloseOtherMenu()
    {
        _constructionMenu.CloseConstructionMenu();
    }
}