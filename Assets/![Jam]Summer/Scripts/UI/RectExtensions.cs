using UnityEngine;

public static class RectExtensions
{
    public static Rect Inflated(this Rect rect, float padding)
    {
        return new Rect(
            rect.x - padding,
            rect.y - padding,
            rect.width + padding * 2,
            rect.height + padding * 2);
    }
}
