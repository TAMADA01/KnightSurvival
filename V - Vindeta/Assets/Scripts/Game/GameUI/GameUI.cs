using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour, IPauseHandler
{
    public HealthBar HealthBarPlayer;
    public bool IsOpenPausePanel = true;
    public bool IsSetPaused = true;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _diePanel;
    [SerializeField] private Text _finalText;
    [SerializeField] private TextWave _textWave;
    [SerializeField] Text _enemyCountText;
    [SerializeField] private Upgrade _upgradePanel;

    private int _wave => ProjectContext.Instance.Wave;
    private int _record => ProjectContext.Instance.Record;

    private PauseManager _pauseManager => ProjectContext.Instance.PauseManager;

    private void Start()
    {
        _pauseManager.Registered(this);
        _textWave.gameObject.SetActive(true);
        _pausePanel.SetActive(false);
        _diePanel.SetActive(false);
    }

    private void OnDisable()
    {
        _pauseManager.UnRegistered(this);
    }

    public void UpdateEnemyCountText(int enemyCount)
    {
        _enemyCountText.text = enemyCount > 0 ? $"{GetStayWord(enemyCount)} {enemyCount} {GetEnemyWord(enemyCount)}" : string.Empty;
    }

    public void OnContinue()
    {
        if (IsSetPaused)
        {
            _pauseManager.SetPaused(false);
        }
        else
        {
            SwitchPausePanel();
        }
    }

    public void OnRestart()
    {
        _pauseManager.SetPaused(false);
        ProjectContext.Instance.EnemyCount = 0;
        SceneManager.LoadScene(1);
    }

    public void OnExit()
    {
        _pauseManager.SetPaused(false);
        SceneManager.LoadScene(0);
    }

    public void OnDiePanel()
    {
        IsOpenPausePanel = false;
        UpdateFinalText();
        _pauseManager.SetPaused(true);
        _diePanel.SetActive(true);
    }

    private void UpdateFinalText()
    {
        ProjectContext.Instance.SaveGame();
        _finalText.text = $"Вы продержались {_wave} {GetWaveWord(_wave)}\r\nВаш рекорд {_record} {GetWaveWord(_record, true)}";
    }

    private string GetWaveWord(int number, bool islastCharA = false)
    {
        char lastChar = number.ToString()[number.ToString().Length - 1];
        if (lastChar == '1' && number/10%10 != 1)
        {
            return islastCharA ? "волна" : "волну";
        }
        else if ("234".IndexOf(lastChar) != -1 && number / 10 % 10 != 1)
        {
            return "волны";
        }
        else
        {
            return "волн";
        }
    }

    private string GetEnemyWord(int number)
    {
        char lastChar = number.ToString()[number.ToString().Length - 1];
        if (lastChar == '1' && number / 10 % 10 != 1)
        {
            return "враг";
        }
        else if ("234".IndexOf(lastChar) != -1 && number / 10 % 10 != 1)
        {
            return "врага";
        }
        else
        {
            return "врагов";
        }
    }

    private string GetStayWord(int number)
    {
        char lastChar = number.ToString()[number.ToString().Length - 1];
        if (lastChar == '1' && number / 10 % 10 != 1)
        {
            return "Остался";
        }
        else
        {
            return "Осталось";
        }
    }

    public void SwitchPausePanel()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
    }

    public IEnumerator ShowWave(int wave)
    {
        yield return StartCoroutine(_textWave.Show(wave));
    }

    public IEnumerator ChooseUpgrade()
    {
        IsSetPaused = false;
        IsOpenPausePanel = false;
        _pauseManager.SetPaused(true);

        yield return StartCoroutine(_upgradePanel.Choose());

        _pauseManager.SetPaused(false);
        IsSetPaused = true;
        IsOpenPausePanel = true;
    }

    void IPauseHandler.SetPaused(bool isPaused)
    {
        if (IsOpenPausePanel)
        {
            _pausePanel.SetActive(isPaused);
        }
    }

}
