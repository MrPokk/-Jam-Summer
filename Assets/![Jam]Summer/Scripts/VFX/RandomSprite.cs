using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] _sprites; 
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
