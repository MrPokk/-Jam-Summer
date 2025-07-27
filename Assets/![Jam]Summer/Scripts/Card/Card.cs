using System.Collections;
using BitterCMS.CMSSystem;
using DG.Tweening;
using UnityEngine;

public abstract class Card : CMSViewCore
{
    [SerializeField]
    protected TypeCard _type;
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

    protected SpriteRenderer _spriteRenderer;
    protected bool _isAnimDamage; 

    public virtual TypeCard Type => _type;
    public virtual CategoryCard Category => CategoryCard.None;
    public int MaxHealth => _maxHealth;
    public int Health => _health;
    public int Price => _price;
    public Vector2Int PosGrid => _pos;
    public bool IsPlayer => _isPlayer;

    public virtual void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = _maxHealth;
    }
    public virtual void TakeDamage(int damage)
    {
        CoroutineUtility.Run(TakeDamageAnim());
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
        GridMaster.Instance.Remove(_pos);
        _spriteRenderer.color = Color.red;
        transform.DOLocalRotate(new Vector3(0, 0, -90), 0.5f).Play().OnComplete( () => { Destroy(gameObject); });
    }
    public virtual IEnumerator TakeDamageAnim()
    {
        if (_isAnimDamage)
            yield break;
        _isAnimDamage = true;
        _spriteRenderer.color = Color.red;
        yield return transform.DOShakePosition(0.3f, .7f, 3).Play().WaitForCompletion();
        _spriteRenderer.color = Color.white;
        _isAnimDamage = false;
        yield break;
    }
    public abstract IEnumerator TurnStart();
    public abstract IEnumerator TurnEnd();
    public virtual void SetPos(Vector2Int pos) => _pos = pos;
    public virtual void SetTeam(bool team)
    {
        _isPlayer = team;

        var renderer = GetComponent<SpriteRenderer>();
        var colorComponent = GetComponent<ShaderColorController>();            

        if (_isPlayer)
        {
            colorComponent?.SetReplacementColor(Color.blue);
        }
        else
        {
            renderer.flipX = true;
            colorComponent?.SetReplacementColor(Color.red);
        }
    }
}
public enum CategoryCard
{
    None,
    Build,
    Entity,
    Other
}
public enum TypeCard
{
    None,
    Build,
    Castle,
    Bowman,
    Swordsman,
    Wizard,
    Cavalry
}
