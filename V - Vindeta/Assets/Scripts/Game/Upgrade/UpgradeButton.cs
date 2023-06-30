using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Player Player;
    [HideInInspector] public Text DiscriptionText;

    [TextArea]
    [SerializeField] private string _discription;

    [SerializeField] private int _maxNumberOfChoices = 0;
    [Range(0f, 1f)]
    [SerializeField] private float _change = 1;


    public int MaxNumberOfChoice => _maxNumberOfChoices;
    public float Change => _change;

    public void Heal()
    {
        Player.Health += Mathf.CeilToInt(Player.MaxHealth * 0.7f);
    }

    public void IncreaseDamage()
    {
        Player.Shooting.Damage += Mathf.CeilToInt(Player.Shooting.Damage * 0.2f);
    }

    public void IncreaseMaxHealth()
    {
        Player.MaxHealth += Mathf.CeilToInt(Player.MaxHealth * 0.2f);
        Player.Health += Mathf.CeilToInt(Player.Health * 0.2f);
    }

    public void IncreaseArmor()
    {
        Player.Armor += Mathf.CeilToInt(Player.Armor * 0.15f);
    }

    public void IncreaseCritChange() 
    { 
        Player.Shooting.ChangeCrit += 0.01f;
    }

    public void IncreaseCritMultiplier()
    {
        Player.Shooting.CritMultiplier += 0.5f;
    }

    public void IncreaseMoveSpeed()
    {
        Player.Movement.Speed += 0.15f;
    }

    public void IncreaseReloadSpeed()
    {
        Player.Shooting.CooldownShot -= Player.Shooting.CooldownShot * 0.1f;
    }

    public void AddDiagonaShot()
    {
        Player.Shooting.BulletCount += 2;
        Player.Shooting.Angle += 30f;
    }

    public void FireShot()
    {

    }

    public void IceShot()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DiscriptionText.text = _discription;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DiscriptionText.text = string.Empty;
    }
}
