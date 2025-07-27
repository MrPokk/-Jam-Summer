using UnityEngine;

public class DescriptionEntityComponent : MonoBehaviour
{
    [field: Header("Basic Info")]
    [field: SerializeField] public string EntityName { get; private set; } = "";

    [field: TextArea(2, 4)]
    [field: SerializeField] public string Description { get; private set; } = "";

    [field: TextArea(2, 4)]
    [field: SerializeField] public string Ability { get; private set; } = "";

    public int Health
    {
        get
        {
            return gameObject.GetComponent<Card>().MaxHealth;
        }
    }
    public int Attack
    {
        get
        {
            if (gameObject.TryGetComponent<CardEntity>(out var cardEntity))
                return cardEntity.Damage;
            return 0;
        }
    }
}
