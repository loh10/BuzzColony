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
        CloseOtherMenu();
        isActive = !isActive;
        textToChange.text = isActive ? "X" : "Ressources";
        EnableAll(isActive);
    }

    public void DisableAllRessource()
    {
        EnableAll(false);
        textToChange.text = "Ressources";
        isActive = false;
    }

    public void CloseOtherMenu()
    {
        _constructionMenu.CloseConstructionMenu();
    }
}