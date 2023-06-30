using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _damageBar;
    [SerializeField] private float _speed;
    [SerializeField] private float _delayTime;

    private float _maxHealth;
    private float _lastDamageTime;
    private float _deltaTime = 0f;
    private float _nowTime;
    private bool _isPaused => ProjectContext.Instance.PauseManager.IsPaused;

    private void Update()
    {
        if (_isPaused)
        {
            _deltaTime = Time.deltaTime - _nowTime;
            return;
        }

        _lastDamageTime += _deltaTime;
        _deltaTime = 0;

        _nowTime = Time.time;
        if (_nowTime - _lastDamageTime > _delayTime)
        {
            if (_healthBar.fillAmount < _damageBar.fillAmount)
            {
                _damageBar.fillAmount -= _speed * Time.deltaTime;
            }
            else
            {
                _damageBar.fillAmount = _healthBar.fillAmount;
            }
        }
    }

    public void SetMaxValue(float health)
    {
        _maxHealth = health;
        _healthBar.fillAmount = 1;
    }

    public void UpdateValue(float health)
    {
        _lastDamageTime = Time.time;
        _healthBar.fillAmount = health / _maxHealth;
    }
}
