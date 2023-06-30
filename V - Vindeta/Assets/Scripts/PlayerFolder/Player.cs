using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Shooting))]
[RequireComponent(typeof(Movement))]

public class Player : MonoBehaviour, IDamageable
{
    private InputData _inputData => ProjectContext.Instance.InputData;
    private PauseManager _pauseManager => ProjectContext.Instance.PauseManager;
    private bool _isPause => ProjectContext.Instance.PauseManager.IsPaused;

    [HideInInspector] public Movement Movement;
    [HideInInspector] public Shooting Shooting;
    private GameUI _gameUI;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health;
    [SerializeField] public int Armor = 5;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }

        set
        {
            _maxHealth = value;
            _gameUI.HealthBarPlayer.SetMaxValue(_maxHealth);
        }
    }

    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            if (value <= _maxHealth)
            {
                _health = value;
            }
            else
            {
                _health = _maxHealth;
            }


            if (_health <= 0)
            {
                _gameUI.OnDiePanel();
            }
            _gameUI.HealthBarPlayer.UpdateValue(Health);
        }
    }

    private void Awake()
    {
        Movement = GetComponent<Movement>();
        Shooting = GetComponent<Shooting>();
        _gameUI = FindObjectOfType<GameUI>();
    }

    private void Start()
    {
        _health = _maxHealth;
        _gameUI.HealthBarPlayer.SetMaxValue(_maxHealth);
    }

    private void Update()
    {
        if (_health <= 0)
        {
            return;
        }

        if (Input.GetKeyDown(_inputData.EscKey))
        {
            if (_gameUI.IsSetPaused)
            {
                _pauseManager.SetPaused(!_pauseManager.IsPaused);
            }
            else
            {
                _gameUI.SwitchPausePanel();
            }
        }

        if (_isPause)
        {
            return;
        }

        Movement.UpdateInput();
        Shooting.Rotation();

        if (Input.GetKey(_inputData.ShotKey))
        {
            Shooting.Shot();
        }

    }

    private void FixedUpdate()
    {
        if (_isPause)
        {
            return;
        }
        Movement.Move();
    }

    public void TakeDamage(int damage)
    {
        Health -= (int)(damage * (1 - Armor/100f));
    }
}
