using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageMenu : MonoBehaviour
{
    public GameObject messageMenu;
    
    void Start()
    {
        messageMenu.SetActive(false);
    }
    public void OpenMessageMenu()
    {
        messageMenu.SetActive(!messageMenu.activeSelf);
    }
}
