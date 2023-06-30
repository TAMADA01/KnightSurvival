using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IPauseHandler, IMoveable, IDamageable
{
    [Header("Enemy")]

    public AnimationCurve SpawnChange;

    protected Rigidbody2D _rigidbody2d;
    protected Player _player;
    public EnemyUI EnemyUI;
    protected AppearanceOfEnemies _appearanceOfEnemies;
    protected Animator _animator;

    [SerializeField] protected GameObject _effectDie;

    [SerializeField] protected AnimationCurve _healthCurve;
    [SerializeField] protected AnimationCurve _damageCurve;
    protected int _health;
    protected int _damage;
    [SerializeField] protected float _speed = 2f;
    [SerializeField] protected float _chanceOfMiss = 0.1f;
    [SerializeField] protected int _armor = 5;
    [SerializeField] protected float _distanceToTarget = 1.5f;
    [SerializeField] protected bool _isRotation = true;

    protected Vector2 _vectorToTarget;
    protected float _currentSpeed;
    protected bool _isMoving;

    protected bool _isPaused => ProjectContext.Instance.PauseManager.IsPaused;
    protected IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);

    public int Health => (int)(_healthCurve.Evaluate(ProjectContext.Instance.Wave));

    protected virtual void Awake()
    {
        ProjectContext.Instance.PauseManager.Registered(this);

        _appearanceOfEnemies = FindAnyObjectByType<AppearanceOfEnemies>();
        _player = FindFirstObjectByType<Player>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        EnemyUI = GetComponent<EnemyUI>();
        _animator =  GetComponentInChildren<Animator>();

        _rigidbody2d.gravityScale = 0;
        _rigidbody2d.freezeRotation = true;

        _currentSpeed = _speed;
        _health = (int)(_healthCurve.Evaluate(ProjectContext.Instance.Wave));
        _damage = (int)(_damageCurve.Evaluate(ProjectContext.Instance.Wave));
    }

    protected void OnDisable()
    {
        ProjectContext.Instance.PauseManager.UnRegistered(this);
    }

    public void Move()
    {
        Vector2 offset = _vectorToTarget.normalized * _speed * Time.fixedDeltaTime;
        _rigidbody2d.MovePosition(_rigidbody2d.position + offset);
        if (_isRotation) 
        { 
            Rotation(); 
        }
    }

    protected void Rotation()
    {
        var tempScale = _animator.gameObject.transform.localScale;
        tempScale.x = _vectorToTarget.normalized.x > 0 ? Mathf.Abs(tempScale.x) : -Mathf.Abs(tempScale.x);
        _animator.gameObject.transform.localScale = tempScale;
    }

    protected void Die()
    {
        Instantiate(_effectDie, transform.position, Quaternion.identity);
        ProjectContext.Instance.ReducingEnemy();
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        float change = Random.value;
        if (_chanceOfMiss < change)
        {
            if (_health <= 0)
            {
                return;
            }
            _health -= (int)(damage * (1 - _armor / 100f));
            EnemyUI.UpdateHealth(_health);
            if (_health <= 0)
            {
                Die();
            }
        }
        else
        {
            EnemyUI.ShowMiss();
        }
    }

    public virtual void SetPaused(bool isPaused)
    {
        _animator.speed = isPaused ? 0f : 1f;
        _effectDie.GetComponent<Animator>().speed = isPaused ? 0f : 1f;
    }

}
