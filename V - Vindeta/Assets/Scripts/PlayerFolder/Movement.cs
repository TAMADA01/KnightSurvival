using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IPauseHandler, IMoveable
{
    [SerializeField] private Rigidbody2D _rigidbody2d;
    [SerializeField] private Animator _animator;

    public float Speed = 10f;

    private Vector2 moveInput;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ProjectContext.Instance.PauseManager.Registered(this);
        _rigidbody2d.gravityScale = 0;
        _rigidbody2d.freezeRotation = true;
    }

    private void OnDisable()
    {
        ProjectContext.Instance.PauseManager.UnRegistered(this);
    }

    public void UpdateInput()
    {
        moveInput = new Vector2(Input.GetAxisRaw(InputData.Horizontal), Input.GetAxisRaw(InputData.Vertical));
        _animator.SetFloat("X", moveInput.normalized.x);
        _animator.SetFloat("Y", moveInput.normalized.y);
    }

    public void Move()
    {
        Vector2 offset = moveInput.normalized * Speed * Time.fixedDeltaTime;
        _rigidbody2d.MovePosition(_rigidbody2d.position + offset);
    }

    void IPauseHandler.SetPaused(bool isPaused)
    {
        _animator.speed = isPaused ? 0f : 1f;
    }
}
