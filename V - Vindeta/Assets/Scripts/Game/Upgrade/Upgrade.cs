using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _containerOfCollected;
    [SerializeField] private List<UpgradeButton> _buttonsPrefab;
    [SerializeField] private Text _discriptionText;
    private Player _player;
    private bool _onClick = false;
    private int _buttonCount = 3;
    private List<UpgradeButton> _buttonsCreate;
    private Dictionary<UpgradeButton, int> _numberOfChoice = new Dictionary<UpgradeButton, int>();

    private void Awake()
    {
        _player = FindFirstObjectByType<Player>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
        foreach (var button in _buttonsPrefab)
        {
            _numberOfChoice.Add(button, 0);
        }
    }

    public IEnumerator Choose()
    {
        _onClick = false;

        var buttons = ButtonSelection();

        gameObject.SetActive(true);

        yield return new WaitUntil(() => _onClick);

        gameObject.SetActive(false);

        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
    }

    private List<UpgradeButton> ButtonSelection()
    {
        _discriptionText.text = "";
        _buttonsCreate = SelectionOfButtonToCreate();
        var buttons = new List<UpgradeButton>();

        for (int i = 0; i < _buttonCount; i++)
        {
            int index = GetWeightedIndex();

            var button = Instantiate(_buttonsCreate[index], _container.transform);
            button.Player = _player;
            button.DiscriptionText = _discriptionText;
            button.GetComponent<Button>().onClick.AddListener(() => _onClick = true);

            var indexButton = _buttonsPrefab.IndexOf(_buttonsCreate[index]);
            button.GetComponent<Button>().onClick.AddListener(() => AddButtonInCollected(_buttonsPrefab[indexButton]));
            button.GetComponent<Button>().onClick.AddListener(() => _numberOfChoice[_buttonsPrefab[indexButton]]++);

            buttons.Add(button);

            _buttonsCreate.RemoveAt(index);
        }

        return buttons;
    }

    private List<UpgradeButton> SelectionOfButtonToCreate()
    {
        var buttons = new List<UpgradeButton>();

        foreach (var button in _buttonsPrefab)
        {
            if (_numberOfChoice[button] < button.MaxNumberOfChoice || button.MaxNumberOfChoice <= 0)
            {
                buttons.Add(button);
            }
        }

        return buttons;
    }

    private int GetWeightedIndex()
    {
        float sum = 0f;
        foreach (var button in _buttonsPrefab)
        {
            sum += button.Change;
        }

        var currentChange = Random.value;
        float sumChange = 0;
        for (int i = 0; i < _buttonsCreate.Count; i++)
        {
            sumChange += _buttonsCreate[i].Change / sum;

            if (currentChange <= sumChange && (_buttonsCreate[i].Change / sum) != 0)
            {
                return i;
            }
        }

        return 0;
    }

    private void AddButtonInCollected(UpgradeButton buttonPrefab)
    {
        var button = Instantiate(buttonPrefab, _containerOfCollected.transform);
        button.enabled = false;
        button.GetComponent<Button>().enabled = false;
    }
}
