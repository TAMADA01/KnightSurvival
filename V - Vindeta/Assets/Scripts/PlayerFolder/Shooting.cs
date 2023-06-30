using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _gunTransform;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private AppearingText _critPrefab;

    public float BulletSpeed = 10f;
    public float CooldownShot = 0.1f;
    public int Damage = 5;
    public float ChangeCrit = 0.05f;
    public float CritMultiplier = 2f;

    [HideInInspector] public int BulletCount = 1;
    public float Angle = 0f;

    private Vector2 _mousePos;

    protected Coroutine _currentCoroutine = null;

    private IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);

    public void Shot()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(ShotCoroutine());
        }
    }

    public IEnumerator ShotCoroutine()
    {
        for (int i = 0; i < BulletCount; i++)
        {
            var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

            float changeCrit = Random.value;
            if (changeCrit < ChangeCrit)
            {
                bullet.Damage = Mathf.CeilToInt(Damage * CritMultiplier);
                bullet.ÑritText = _critPrefab;
            }
            else
            {
                bullet.Damage = Damage;
            }

            bullet.Speed = BulletSpeed;

            Vector2 moveDirection = _firePoint.up;

            if (BulletCount > 1)
            {
                float angle = (-Angle + i * Angle * 2 / (BulletCount-1)) * Mathf.PI / 180f;
                float x = moveDirection.x * Mathf.Cos(angle) - moveDirection.y * Mathf.Sin(angle);
                float y = moveDirection.y * Mathf.Cos(angle) + moveDirection.x * Mathf.Sin(angle);
                moveDirection = new Vector2(x, y);
            }

            bullet.MoveDirection = moveDirection;
        }

        yield return StartCoroutine(WaitForSecondsWithPause(CooldownShot));

        _currentCoroutine = null;
    }

    public void Rotation()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 gunPos = new Vector2(_gunTransform.position.x, _gunTransform.position.y);
        Vector2 lookDir = _mousePos - gunPos;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float scale = lookDir.normalized.x > 0 ? 1 : -1;
        float angleFirePoint = lookDir.normalized.x > 0 ? -90 : 90;

        _gunTransform.rotation = Quaternion.Euler(0, 0, angle);
        _gunTransform.localScale = new Vector3(1, scale, 0);

        _firePoint.localRotation = Quaternion.Euler(0, 0, angleFirePoint);
    }
}
