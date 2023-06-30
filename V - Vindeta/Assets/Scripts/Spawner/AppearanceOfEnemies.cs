using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearanceOfEnemies : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Enemy[] _enemiesTypePrefab;
    [SerializeField] AnimationCurve _difficultyCurve;
    [SerializeField] float _timeBetweenSpawnEnemy = 1f;

    [SerializeField] private float _offsetMin = 2;
    [SerializeField] private float _offsetMax = 5;

    private float _sumChange;

    private IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);

    public IEnumerator Spawn(int enemyCount, int wave)
    {
        AddChange(wave);
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(wave);

            yield return StartCoroutine(WaitForSecondsWithPause(_timeBetweenSpawnEnemy));
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 minCoordinate = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 maxCoordinate = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));

        float xPos = Random.Range(minCoordinate.x, maxCoordinate.x);
        float yPos = Random.Range(minCoordinate.y, maxCoordinate.y);

        int positionRelativeCamera = Random.Range(0, 4);
        switch ((BorderEnum.Border)positionRelativeCamera)
        {
            case BorderEnum.Border.Top:
                yPos = Random.Range(maxCoordinate.y + _offsetMin, maxCoordinate.y + _offsetMax);
                break;
            case BorderEnum.Border.Bottom:
                yPos = Random.Range(minCoordinate.y - _offsetMax, minCoordinate.y - _offsetMin);
                break;
            case BorderEnum.Border.Left:
                xPos = Random.Range(minCoordinate.x - _offsetMax, minCoordinate.x - _offsetMin);
                break;
            case BorderEnum.Border.Right:
                xPos = Random.Range(maxCoordinate.x + _offsetMin, maxCoordinate.x + _offsetMax);
                break;
        }
        return new Vector3(xPos, yPos, 0);
    }

    private void AddChange(int wave)
    {
        _sumChange = 0f;
        foreach (var enemy in _enemiesTypePrefab)
        {
            _sumChange += enemy.SpawnChange.Evaluate(wave - 1);
        }
    }

    private int GetRandomEnemy(int wave)
    {
        float randomChange = Random.value;
        float currentSumChange = 0; 
        for (int enemyIndex = 0; enemyIndex < _enemiesTypePrefab.Length; enemyIndex++)
        {
            currentSumChange += _enemiesTypePrefab[enemyIndex].SpawnChange.Evaluate(wave) / _sumChange;
            if (randomChange <= currentSumChange && _enemiesTypePrefab[enemyIndex].SpawnChange.Evaluate(wave) / _sumChange != 0)
            {
                return enemyIndex;
            }
        }
        return 0;
    }

    private void SpawnEnemy(int wave)
    {
        Instantiate(_enemiesTypePrefab[GetRandomEnemy(wave)], GetRandomPosition(), Quaternion.identity);
    }
}
