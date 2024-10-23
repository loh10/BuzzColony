using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class MessageManager : MonoBehaviour
{
    public List<Message> message = new List<Message>();
    private Message _message;
    public GameObject messageParent, messagePrefab;

    
    public void AddMessage(Message messageToAdd)
    {
        if (message.Count == 4)
        {
            message.RemoveAt(0);
        }

        GameObject newMessage = Instantiate(messagePrefab, messageParent.transform);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = messageToAdd.content;
        if (messageToAdd.haveBtn)
        {
            newMessage.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text =
                messageToAdd.btnText;
            Ressource ressource = Ressource.Bois;
            switch ( Regex.Match(messageToAdd.content, @"\b(Nourriture|Roche|Bois)\b", RegexOptions.IgnoreCase).Value)
            {
                case "Bois":
                    ressource = Ressource.Bois;
                    break;
                case "Roche":
                    ressource = Ressource.Roche;
                    break;
                case "Nourriture":
                    ressource = Ressource.Nourriture;
                    break;
            }
            newMessage.GetComponentInChildren<MessageBtn>().GetValue(int.Parse(Regex.Match(messageToAdd.content, @"\d+").Value), ressource);
        }
        else 
        {
            newMessage.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }

        message.Add(messageToAdd);
    }
}