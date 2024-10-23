using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using Random = UnityEngine.Random;

public class Colon : MonoBehaviour
{
    #region State

    public bool isMine;
    public int nbRessources;
    public Ressource nameRessource;
    private bool notAsk;
    #endregion

    #region Clothes

    public List<Sprite> _head = new List<Sprite>();
    public List<Sprite> _body = new List<Sprite>();
    public List<Sprite> _legs = new List<Sprite>();
    public SpriteRenderer head, body, legs;

    #endregion

    public float speed = 1.5f;
    public GameObject _constructionTarget;

    // Start is called before the first frame update
    void Start()
    {
        ChooseClothe(head, body, legs);
        
    }

    private void ChooseRessourceToAsk()
    {
        nbRessources = Random.Range(3, 10);

        switch (Random.Range(0, 3))
        {
            case 0:
                nameRessource= Ressource.Bois;
                break;
            case 1:
                nameRessource=Ressource.Roche;
                break;
            case 2:
                nameRessource=Ressource.Nourriture;
                break;
        }
        new Message($"Sauvage veut {nbRessources} de {nameRessource}" , true, "Recruter");
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (_constructionTarget)
        {
            Vector2 targetPosition = _constructionTarget.transform.position;
            Vector2 currentPosition = transform.position; 
            Vector2 direction = (targetPosition - currentPosition).normalized;

            if (Vector2.Distance(currentPosition, targetPosition) > 3f)
            {
                transform.Translate(direction * (speed * Time.deltaTime));
            }
            if(Vector2.Distance(transform.position, _constructionTarget.transform.position) < 3f && !notAsk)
            {
                ChooseRessourceToAsk();
                notAsk = true;
            }
        }
    }

    public void ChooseClothe(SpriteRenderer head, SpriteRenderer body, SpriteRenderer legs)
    {
        int randomPart = Random.Range(0, _head.Count);
        head.sprite = _head[randomPart];
        randomPart = Random.Range(0, _body.Count);
        body.sprite = _body[randomPart];
        randomPart = Random.Range(0, _legs.Count);
        legs.sprite = _legs[randomPart];
    }
}