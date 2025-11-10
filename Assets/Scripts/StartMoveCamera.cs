using UnityEngine;

public class StartMoveCamera : MonoBehaviour
{
    [SerializeField, Min(0)] private float animationSpeed = 1f;
    [SerializeField, Min(0)] private float destroyTime = 0f;
    
    private void Start()
    {
        Destroy(this, destroyTime);
    }

    private void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * animationSpeed);
    }

    private void OnDestroy()
    {
        GameManager.Instance.SnapToGame();
    }
}