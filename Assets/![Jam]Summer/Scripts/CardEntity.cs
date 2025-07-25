using System.Collections;
using UnityEngine;

public class CardEntity : Card
{
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
        while (Step > 0)
        {
            if (GridMaster.instant.TryFindNearestEnemy(_pos, !IsPlayer, out Card enemy, out float dist))
            {
                if (dist > AttackDist)
                {
                    yield return MoveToPos(enemy.PosGrid);
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

    public IEnumerator MoveToPos(Vector2Int pos)
    {
        if (pos == _pos) yield break;
        Vector2Int move = NormalizedVec2Int(pos - PosGrid);
        Vector2Int nextPos = _pos + move;
        if (!GridMaster.instant.TryGetAtPos(nextPos, out var _null))
        {
            GridMaster.instant.Remove(_pos);
            GridMaster.instant.Add(this, nextPos);
            _pos = nextPos;
            yield return new WaitForSeconds(0.3f);
        }
        yield break;
    }
    public IEnumerator Attack(Card card)
    {
        card.TakeDamage(Damage);
        yield return new WaitForSeconds(0.3f);
    }

    protected Vector2Int NormalizedVec2Int(Vector2Int vector)
    {
        if (vector.x > 0) vector.x = 1;
        if (vector.y > 0) vector.y = 1;
        if (vector.x < 0) vector.x = -1;
        if (vector.y < 0) vector.y = -1;
        return vector;
    }
}
