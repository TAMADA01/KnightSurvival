using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    [Header("Shooting enemy")]
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Bullet _bulletPrefab;

    [SerializeField] protected float _cooldownShot = 1f;
    [SerializeField] protected float _bulletSpeed = 5f;
    [SerializeField] protected float _distanceToTargetForShot = 5f;
    [SerializeField] protected float _turnSpeed = 5f;

    protected bool _isShot;

    private void Start()
    {
        StartCoroutine(Shot());
    }

    private void FixedUpdate()
    {
        if (_isPaused)
        {
            return;
        }
        _vectorToTarget = _player.gameObject.transform.position - transform.position;
        _isMoving = _vectorToTarget.magnitude > _distanceToTarget;

        TurnFirePoint();

        if (_isMoving)
        {
            if (_isShot)
            {
                if (_vectorToTarget.magnitude >= _distanceToTargetForShot)
                {
                    _isShot = false;
                }
            }
            else
            {
                Move();
            }
        }
        else
        {
            _isShot = true;
        }
    }

    protected IEnumerator Shot()
    {
        while (true)
        {
            yield return new WaitUntil(() => _isShot);
            var bullet = Instantiate(_bulletPrefab.gameObject, _firePoint.position, _firePoint.rotation).GetComponent<Bullet>();

            bullet.Damage = _damage;
            bullet.Speed = _bulletSpeed;
            bullet.MoveDirection = -_firePoint.up;

            yield return StartCoroutine(WaitForSecondsWithPause(_cooldownShot));
        }
    }

    protected void TurnFirePoint()
    {
        float angel = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg + 90f;
        _firePoint.rotation = Quaternion.Euler(0, 0, angel);
    }
}
