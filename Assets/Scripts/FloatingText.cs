using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    private void Start()
    {
        Destroy(gameObject, GameManager.Instance.rng.Next(3, 7));
        speed = GameManager.Instance.rng.Next(60, 100);
        direction = new Vector2(GameManager.Instance.rng.Next(-100, 100), GameManager.Instance.rng.Next(1, 100));
        direction = direction.normalized;
    }

    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }
}