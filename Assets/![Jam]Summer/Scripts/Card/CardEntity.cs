using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InGame.Script.Component_Sound;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CardEntity : Card
{
    public const float TimeMove = 0.2f;
    public const float TimeAttack = 0.1f;
    public const float TimeAniAttack = 0.3f;
    public int MaxStep;
    public int Step;
    public int Damage;
    public float AttackDist;
    public bool IsFront;
    public override CategoryCard Category => CategoryCard.Entity;
    public override void Init()
    {
        base.Init();
        Step = MaxStep;
    }
    public override IEnumerator TurnStart()
    {
        if (Health == 0) yield break;
        yield return SwapFront();
        while (Step > 0)
        {
            if (GridMaster.Instance.TryFindNearestEntity(_pos, !IsPlayer, out Card enemy, out float dist))
            {
                if (dist > AttackDist)
                {
                    yield return MoveTowardsTarget(enemy.PosGrid);
                }
                else
                {
                    yield return Attack(enemy);
                    break;
                }
            }
            Step--;
        }

        yield break;
    }
    public override IEnumerator TurnEnd()
    {
        Step = MaxStep;
        yield break;
    }

    public IEnumerator MoveTowardsTarget(Vector2Int targetPos)
    {
        if (targetPos == _pos) yield break;

        // Получаем следующий шаг пути
        Vector2Int nextStep = GetNextStepTowards(targetPos);

        if (nextStep != _pos) // Если нашли возможный шаг
        {
            GridMaster.Instance.Remove(_pos);
            GridMaster.Instance.Add(this, nextStep);
            _pos = nextStep;
            yield return new WaitForSeconds(TimeMove);
        }
    }

    private Vector2Int GetNextStepTowards(Vector2Int targetPos)
    {
        Vector2Int direction = NormalizedVec2Int(targetPos - _pos);
        Vector2Int straightMove = _pos + direction;

        if (!GridMaster.Instance.TryGetAtPos(straightMove, out var _null))
        {
            return straightMove;
        }
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector2Int neighbor = new Vector2Int(_pos.x + x, _pos.y + y);
                if (!GridMaster.Instance.TryGetAtPos(neighbor, out var __null) && GridMaster.Instance.Grid.CheckInSize(neighbor))
                {
                    float currentDist = Vector2Int.Distance(_pos, targetPos);
                    float newDist = Vector2Int.Distance(neighbor, targetPos);
                    if (newDist <= currentDist)
                    {
                        possibleMoves.Add(neighbor);
                    }
                }
            }
        }
        if (possibleMoves.Count > 0)
        {
            possibleMoves.Sort((a, b) =>
                Vector2Int.Distance(a, targetPos).CompareTo(Vector2Int.Distance(b, targetPos)));
            return possibleMoves[0];
        }
        return _pos;
    }
    public IEnumerator Attack(Card card)
    {
        card.TakeDamage(Damage);
        Vector3 oldPos = transform.position;
        transform.DOPunchPosition((Vector3)GridMaster.Instance.GridToWorldCentre(card.PosGrid) - oldPos, TimeAniAttack, 1, 0.2f).Play();
        yield return new WaitForSeconds(TimeAttack);
    }
    public IEnumerator SwapFront()
    {
        if (IsFront == true &&
            GridMaster.Instance.TryFindNearestEntity(_pos, !IsPlayer, out Card enemy, out float distEnemy) &&
            GridMaster.Instance.TryGetAtPos(_pos - NormalizedVec2Int(_pos - enemy.PosGrid), out Card frontCard) &&
            frontCard is CardEntity entity &&
            entity.IsPlayer == IsPlayer)
        {
            GridMaster.Instance.Remove(_pos);
            GridMaster.Instance.Remove(entity.PosGrid);
            GridMaster.Instance.Add(this, entity.PosGrid);
            GridMaster.Instance.Add(entity, _pos);
            (_pos, entity._pos) = (entity._pos, _pos);
            yield return new WaitForSeconds(TimeMove);
        }
        yield break;
    }
    protected Vector2Int NormalizedVec2Int(Vector2Int vector)
    {
        Vector2 vector2 = vector;
        vector2.Normalize();
        if (Mathf.Abs(vector2.x) == Mathf.Abs(vector2.y))
        {
            vector.x = System.Math.Sign(vector2.x);
            vector.y = System.Math.Sign(vector2.y);
        }
        else
        {
            if (Mathf.Abs(vector2.x) > Mathf.Abs(vector2.y))
            {
                vector.x = System.Math.Sign(vector2.x);
                vector.y = 0;
            }
            else
            {
                vector.x = 0;
                vector.y = System.Math.Sign(vector2.y);
            }
        }
        return vector;
    }
}
