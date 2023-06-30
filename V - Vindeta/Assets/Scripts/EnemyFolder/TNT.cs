using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : SimpleEnemy
{
    private void FixedUpdate()
    {
        if (_isPaused)
        {
            return;
        }
        _vectorToTarget = _player.gameObject.transform.position - transform.position;
        _isMoving = _vectorToTarget.magnitude > _distanceToTarget;

        if (_isMoving)
        {
            Move();
        }
        else
        {
            _player.TakeDamage(_damage);
            Die();
        }
    }
}
