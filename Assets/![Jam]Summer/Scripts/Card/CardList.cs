using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardList", fileName = "CardList")]
public class CardList : ScriptableObject
{
    public List<CardBuild> Builds;
    public List<CardEntity> Entities;
}
