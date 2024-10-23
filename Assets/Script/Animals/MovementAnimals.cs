using UnityEngine;

public class MovementAnimals : MonoBehaviour
{
    public float speed = 1.5f;
    public float changeDirectionInterval = 3f;
    private Vector2 _targetDirection;
    private float _timeSinceLastChange;

    private void Start()
    {
        ChooseRandomDirection();
    }

    private void Update()
    {
        MoveAnimal();
        _timeSinceLastChange += Time.deltaTime;
        if (_timeSinceLastChange >= changeDirectionInterval)
        {
            ChooseRandomDirection();
            _timeSinceLastChange = 0f;
        }
    }

    private void MoveAnimal()
    {
        transform.Translate(_targetDirection * (speed * Time.deltaTime));
    }

    private void ChooseRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        _targetDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}