using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class MovementAnimals : MonoBehaviour
{
    private Direction _direction = Direction.None;
    private int speed = 8;

    private void Start()
    {
        print("j'existe");
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        CheckDirection();
        print("je me dirige");
        yield return new WaitForSeconds(5);
        StartCoroutine(Move());
    }

    private Direction ChooseDirection()
    {
        int random = Random.Range(0, 4);
        _direction = (Direction)random;
        print("je vais vers le " + _direction);
        return _direction;
    }

    private void CheckDirection()
    {
        Vector3 target = new Vector3();
        switch (ChooseDirection())
        {
            case Direction.Down:
                target = Vector3.down;
                break;
            case Direction.Up:
                target = Vector3.up ;
                break;
            case Direction.Left:
                target = Vector3.left;
                break;
            case Direction.Right:
                target = Vector3.right ;
                break;
            default:
                print("wahou");
                break;
        }
        print("ma cible est " + target);
        if (Physics.Raycast(transform.position, target, out RaycastHit hit, float.PositiveInfinity))
        {
            if (Vector2.Distance(transform.position, hit.transform.position) < 15 &&
                Vector2.Distance(transform.position, hit.transform.position) >= 2)
            {
                GetComponent<NavMeshAgent>().SetDestination(hit.transform.position);
            }
            else
            {
                print("il n y a rien pour moi ici   ");
            }
        }
    }
}