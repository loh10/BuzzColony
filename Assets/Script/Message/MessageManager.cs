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
            ERessource eRessource = ERessource.Bois;
            switch ( Regex.Match(messageToAdd.content, @"\b(Nourriture|Roche|Bois)\b", RegexOptions.IgnoreCase).Value)
            {
                case "Bois":
                    eRessource = ERessource.Bois;
                    break;
                case "Roche":
                    eRessource = ERessource.Roche;
                    break;
                case "Nourriture":
                    eRessource = ERessource.Nourriture;
                    break;
            }
            newMessage.GetComponentInChildren<MessageBtn>().GetValue(int.Parse(Regex.Match(messageToAdd.content, @"\d+").Value), eRessource);
        }
        else 
        {
            newMessage.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }

        message.Add(messageToAdd);
    }
}