using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private AppearingText _missText;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _healthBar.SetMaxValue(_enemy.Health);
    }

    public void UpdateHealth(int health)
    {
        _healthBar.UpdateValue(health);
    }
    public void Delete() 
    {
        _healthBar.gameObject.SetActive(false);
    }

    public void ShowMiss()
    {
        var missText = Instantiate(_missText, transform.position, Quaternion.identity, _canvas.transform);
        //StartCoroutine(missText.StartAnimation());
    }

    public void ShowCrit(AppearingText critTextPrefab)
    {
        var critText = Instantiate(critTextPrefab, transform.position, Quaternion.identity, _canvas.transform);
        //StartCoroutine(critText.StartAnimation());
    }
}
