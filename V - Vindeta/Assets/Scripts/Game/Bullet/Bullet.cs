using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Bullet : MonoBehaviour, IPauseHandler, IMoveable, IDamageable
{
    public int Damage { get; set; }
    public float Speed { get; set; }
    public Vector2 MoveDirection { get; set; }

    [SerializeField] protected GameObject _destroyEffect;
    [SerializeField] protected float _destroyTime = 3f;

    [SerializeField] protected GameUI _gameUI;
    protected Animator _animator;
    protected Rigidbody2D _rigidbody2d;
    protected Collider2D _collider2d;
    protected bool _isMove = true;
    [HideInInspector] public AppearingText ÑritText = null;

    protected bool _isPaused => ProjectContext.Instance.PauseManager.IsPaused;
    protected IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);

    protected virtual void Awake()
    {
        _gameUI = FindFirstObjectByType<GameUI>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();

        ProjectContext.Instance.PauseManager.Registered(this);
    }

    private void Start()
    {
        StartCoroutine(DestractionOnTime(_destroyTime));  
    }

    protected void OnDisable()
    {
        ProjectContext.Instance.PauseManager.UnRegistered(this);
    }

    protected void FixedUpdate()
    {
        if (_isPaused)
        {
            return;
        }

        if (_isMove)
        {
            Move();
        }
    }

    public abstract void Move();

    protected virtual IEnumerator DestractionOnTime(float waitTime)
    {
        yield return StartCoroutine(WaitForSecondsWithPause(waitTime));

        TakeDamage(-1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopAllCoroutines();

        collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage);

        if (ÑritText != null && collision.gameObject.GetComponent<Enemy>() != null)
        {
            var critText = Instantiate(ÑritText, collision.transform.position, Quaternion.identity);
        }

        TakeDamage(-1);
    }

    public void TakeDamage(int damage)
    {
        if (_destroyEffect != null)
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void IPauseHandler.SetPaused(bool isPaused)
    {
        if (_animator != null)
        {
            _animator.speed = isPaused ? 0f : 1f;
        }
    }
}
