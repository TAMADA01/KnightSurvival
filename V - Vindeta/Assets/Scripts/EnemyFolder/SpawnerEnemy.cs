using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : Enemy
{
    [Header("Spawner Enemy")]
    [SerializeField] protected Transform _spawnPoint;
    [SerializeField] protected Bullet _spawnBulletPrefab;

    [SerializeField] protected float _cooldownSpawn = 3f;
    [SerializeField] protected float _cooldownSpawnEnemy = 0.5f;
    [SerializeField] protected int _countSpawnEnemy = 3;

    [SerializeField] protected float _bulletSpeed = 6f;
    [SerializeField] protected int _bulletDamage = 10;

    protected SpriteWithAnimation _sprite;
    protected bool _isSpawning = false;

    protected override void Awake()
    {
        base.Awake();
        _sprite = GetComponentInChildren<SpriteWithAnimation>();
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void FixedUpdate()
    {
        if (_isPaused)
        {
            return;
        }

        _vectorToTarget = _player.gameObject.transform.position - transform.position;
        _isMoving = _vectorToTarget.magnitude > _distanceToTarget;

        if (_isMoving && !_isSpawning)
        {
            Move();
        }
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            yield return StartCoroutine(WaitForSecondsWithPause(_cooldownSpawn));

            _isSpawning = true;
            _animator.SetBool("isSpawn", true);
            
            yield return StartCoroutine(_sprite.WaitEndAnimation());

            int spawnCount = 0;
            while (spawnCount < _countSpawnEnemy)
            {
                var bullet = Instantiate(_spawnBulletPrefab, _spawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.Damage = _bulletDamage;
                bullet.Speed = _bulletSpeed;
                spawnCount++;

                yield return StartCoroutine(WaitForSecondsWithPause(_cooldownSpawnEnemy));
            }

            _animator.SetBool("isSpawn", false);

            yield return StartCoroutine(_sprite.WaitEndAnimation());

            _isSpawning = false;
        }
    }
}
