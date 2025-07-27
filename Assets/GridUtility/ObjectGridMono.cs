using UnityEngine;
using Utility.Grid;
public class ObjectGridMono<T> : GridMonoBehaviour<T> where T : MonoBehaviour
{
    public bool AutoPos = true;
    protected virtual void SetPosObj(T value, Vector2Int pos, bool res)
    {
        if (res && AutoPos)
        {
            value.transform.position = GridToWorldCentre(pos);
        }
    }

    public void Clear()
    {
        foreach (T obj in Grid.GetDictionary().Values)
        {
            Destroy(obj.gameObject);
        }
        Grid.Clear();
    }

    public bool Add(T value, Vector2Int pos) => Add(value, pos, AutoPos);
    public bool Add(T value, Vector2Int pos, bool setPos)
    {
        bool res = Grid.Add(value, pos);
        SetPosObj(value, pos, res); 
        return res;
    }

    public bool AddNearest(T value) => AddNearest(value, out Vector2Int _null);
    public bool AddNearest(T value, out Vector2Int pos)
    {
        bool res = Grid.AddNearest(value, out pos);
        SetPosObj(value, pos, res);
        return res;
    }

    public bool AddRandomPos(T value) => AddRandomPos(value, out Vector2Int _null);
    public bool AddRandomPos(T value, out Vector2Int pos)
    {
        bool res = Grid.AddRandomPos(value, out pos);
        SetPosObj(value, pos, res);
        return res;
    }

    public bool Remove(Vector2Int pos) => Grid.Remove(pos);
    public bool Delete(Vector2Int pos)
    {
        bool res = Grid.TryGetAtPos(pos, out T value);
        if (res)
        {
            Grid.Remove(pos);
            Destroy(value.gameObject);
        }
        return res;
    }
    public bool TryGetAtPos(Vector2Int pos, out T value) => Grid.TryGetAtPos(pos, out value);
}
