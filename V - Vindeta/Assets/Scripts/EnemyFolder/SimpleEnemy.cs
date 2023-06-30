using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    [Header("Simple Enemy")]
    [SerializeField] protected float _forceOfPush = 10f;
    [SerializeField] protected float _cooldownAfterAttack = 0.2f;

    private Coroutine _currentCoroutine;

    private void FixedUpdate()
    {
        if (_isPaused)
        {
            return;
        }
        _vectorToTarget = _player.gameObject.transform.position - transform.position;
        _isMoving = _vectorToTarget.magnitude > _distanceToTarget;

        if (_currentCoroutine == null)
        {
            if (_isMoving)
            {
                Move();
            }
            else
            {
                _player.TakeDamage(_damage);
                _currentCoroutine = StartCoroutine(PushAfterAttack());
            }
        }
    }

    protected IEnumerator PushAfterAttack()
    {
        float currentTime = 0f;
        while (currentTime < _cooldownAfterAttack)
        {
            Vector2 offset = -(_vectorToTarget.normalized) * _forceOfPush * Time.fixedDeltaTime;
            _rigidbody2d.MovePosition(_rigidbody2d.position + offset);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _currentCoroutine = null;
    }
}
