using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private AppearanceOfEnemies _appearanceOfEnemies;
    [SerializeField] private Upgrade _upgrade;

    [SerializeField] AnimationCurve _difficultyCurve;

    private int _enemyCount
    {
        get { return ProjectContext.Instance.EnemyCount; }
        set { ProjectContext.Instance.EnemyCount = value; }
    }

    private int _waveCount
    {
        get { return ProjectContext.Instance.Wave; }
        set { ProjectContext.Instance.Wave = value; }
    }

    private IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);

    private void Start()
    {
        _waveCount = -1;
        _enemyCount = 0;
        _gameUI.UpdateEnemyCountText(_enemyCount);
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        _gameUI.UpdateEnemyCountText(_enemyCount);
    }

    private IEnumerator StartGame()
    {
        while (true)
        {
            if (_enemyCount <= 0)
            {
                _waveCount++;
                if (_waveCount > 0)
                {
                    yield return StartCoroutine(WaitForSecondsWithPause(2f));
                    yield return StartCoroutine(_gameUI.ChooseUpgrade());
                }
                yield return StartCoroutine(_gameUI.ShowWave(_waveCount + 1));

                _enemyCount = GetCountEnemyInWave(_waveCount);

                yield return StartCoroutine(_appearanceOfEnemies.Spawn(_enemyCount, _waveCount));
            }
            yield return null;
        }
    }

    private int GetCountEnemyInWave(int wave)
    {
        return (int)_difficultyCurve.Evaluate(wave);
    }
}
