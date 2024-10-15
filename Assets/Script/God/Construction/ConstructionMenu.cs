using System;
using UnityEngine;

public class ConstructionMenu : MonoBehaviour
{
    private bool isActive;
    [SerializeField] private GameObject marketMenu;
    [SerializeField] private SpawningMenu _spawningMenu;

    private void Start()
    {
        marketMenu.SetActive(false);
    }

    public void ActiveMenu()
    {
        isActive = !isActive;
        marketMenu.SetActive(isActive);
        _spawningMenu.DisableAllRessource();
    }

    public void CloseConstructionMenu()
    {
        isActive = false;
        marketMenu.SetActive(isActive);
    }
}