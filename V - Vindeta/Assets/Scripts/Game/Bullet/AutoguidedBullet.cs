using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoguidedBullet : Bullet
{
    [SerializeField] private bool _isRotation = false;
    private Player _player;

    protected override void Awake()
    {
        base.Awake();
        _player = FindFirstObjectByType<Player>();
    }
    public override void Move()
    {
        MoveDirection = _player.transform.position - transform.position;
        Vector2 offset = MoveDirection.normalized * Speed * Time.fixedDeltaTime;
        _rigidbody2d.MovePosition(_rigidbody2d.position + offset);
        if (_isRotation)
        {
            Rotation();
        }
    }

    protected void Rotation()
    {
        var tempScale = _animator.gameObject.transform.localScale;
        tempScale.x = MoveDirection.normalized.x > 0 ? Mathf.Abs(tempScale.x) : -Mathf.Abs(tempScale.x);
        _animator.gameObject.transform.localScale = tempScale;
    }
}
