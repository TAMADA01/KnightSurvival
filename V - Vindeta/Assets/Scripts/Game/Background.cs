using UnityEngine;

public class Background : MonoBehaviour
{
    private Player _player;
    private SpriteRenderer _renderer;

    [SerializeField]
    private float _originalSizeX;
    [SerializeField]
    private float _originalSizeY;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _originalSizeX = _renderer.bounds.size.x / 3;
        _originalSizeY = _renderer.bounds.size.y / 3;
    }

    private void Update()
    {
        float distanceX = _player.transform.position.x - transform.position.x;
        float distanceY = _player.transform.position.y - transform.position.y;

        if (Mathf.Abs(distanceX) >= _originalSizeX)
        {
            float direction = distanceX > 0 ? 1 : -1;
            transform.Translate(_originalSizeX * direction, 0, 0);
        }

        if (Mathf.Abs(distanceY) >= _originalSizeY)
        {
            float direction = distanceY > 0 ? 1 : -1;
            transform.Translate(0, _originalSizeY * direction, 0);
        }
    }

}
