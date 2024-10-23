using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public bool haveBtn;
    public string content;
    public string btnText;
    
    public Message(string content, bool haveBtn, string btnText)
    {
        this.content = content;
        this.haveBtn = haveBtn;
        this.btnText = btnText;
        CreateMessage();
    }
    public void CreateMessage()
    {
        MessageManager messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();
        messageManager.AddMessage(this);
    }
}
