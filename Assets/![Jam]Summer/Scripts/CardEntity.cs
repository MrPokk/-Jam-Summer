using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEntity : Card
{
    public const float TimeMove = 0.1f;
    public const float TimeAttack = 0.05f;
    public int MaxStep;
    public int Step;
    public int Damage;
    public float AttackDist;
    public override void Init()
    {
        base.Init();
        Step = MaxStep;
    }
    public override IEnumerator TurnStart()
    {
        if (Health == 0) yield break;
        while (Step > 0)
        {
            if (GridMaster.instant.TryFindNearestEnemy(_pos, !IsPlayer, out Card enemy, out float dist))
            {
                if (dist > AttackDist)
                {
                    yield return MoveTowardsTarget(enemy.PosGrid);
                }
                else
                {
                    yield return Attack(enemy);
                    yield break;
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
            GridMaster.instant.Remove(_pos);
            GridMaster.instant.Add(this, nextStep);
            _pos = nextStep;
            yield return new WaitForSeconds(TimeMove);
        }
    }

    private Vector2Int GetNextStepTowards(Vector2Int targetPos)
    {
        // Сначала пробуем двигаться напрямую
        Vector2Int direction = NormalizedVec2Int(targetPos - _pos);
        Vector2Int straightMove = _pos + direction;

        if (!GridMaster.instant.TryGetAtPos(straightMove, out var _null))
        {
            return straightMove;
        }

        // Если прямой путь заблокирован, ищем обходной путь
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        // Проверяем все соседние клетки
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // Пропускаем текущую позицию

                Vector2Int neighbor = new Vector2Int(_pos.x + x, _pos.y + y);

                // Если клетка свободна и ближе к цели, чем текущая позиция
                if (!GridMaster.instant.TryGetAtPos(neighbor, out var __null) && GridMaster.instant.Grid.CheckInSize(neighbor))
                {
                    // Предпочитаем движения, которые приближают к цели
                    float currentDist = Vector2Int.Distance(_pos, targetPos);
                    float newDist = Vector2Int.Distance(neighbor, targetPos);

                    if (newDist <= currentDist)
                    {
                        possibleMoves.Add(neighbor);
                    }
                }
            }
        }

        // Если нашли возможные ходы, выбираем лучший (ближайший к цели)
        if (possibleMoves.Count > 0)
        {
            possibleMoves.Sort((a, b) =>
                Vector2Int.Distance(a, targetPos).CompareTo(Vector2Int.Distance(b, targetPos)));
            return possibleMoves[0];
        }

        // Если нет возможных ходов, остаемся на месте
        return _pos;
    }
    public IEnumerator Attack(Card card)
    {
        card.TakeDamage(Damage);
        yield return new WaitForSeconds(TimeAttack);
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
