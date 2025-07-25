using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField]
    protected int _maxHealth;
    [SerializeField]
    protected int _health;
    [SerializeField]
    protected int _price;
    [SerializeField]
    protected Vector2Int _pos;
    [SerializeField]
    protected bool _isPlayer;
    public int Priority;

    public int MaxHealth => _maxHealth;
    public int Health => _health;
    public int Price => _price;
    public Vector2Int PosGrid => _pos;
    public bool IsPlayer => _isPlayer;

    public virtual void Init()
    {
        _health = _maxHealth;
    }
    public virtual void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Dead();
        }
    }
    public virtual void Healing(int count)
    {
        _health += count;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }
    public virtual void Dead()
    {
        GridMaster.instant.Delete(_pos);
    }

    public abstract IEnumerator TurnStart();
    public abstract IEnumerator TurnEnd();
    public virtual void SetPos(Vector2Int pos) => _pos = pos;
    public virtual void SetTeam(bool team)
    {
        _isPlayer = team;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (!team) renderer.color = new Color(1, 0.5f, 0.5f);
        else renderer.color = new Color(0.5f, 1, 0.5f);
    }
}
