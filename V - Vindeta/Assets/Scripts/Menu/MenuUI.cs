using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Text _recordText;
    private int _record => ProjectContext.Instance.Record;

    private void Start()
    {
        _recordText.text = _record > 0 ? $"Ваш рекорд {_record} {GetWaveWord(_record)}" : string.Empty;
    }

    private string GetWaveWord(int number)
    {
        char lastChar = number.ToString()[number.ToString().Length - 1];
        if (lastChar == '1' && number / 10 % 10 != 1)
        {
            return "волна";
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

    public void StartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitClick()
    {
        ProjectContext.Instance.PauseManager.SetPaused(false);
        Application.Quit();
    }
}
