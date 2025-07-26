using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class ScaleToCameraSize : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastScreenSize;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateScale();
        lastScreenSize = GetCurrentScreenSize();
    }

    private void Update()
    {
        Vector2 currentScreenSize = GetCurrentScreenSize();
        
        if (currentScreenSize != lastScreenSize)
        {
            UpdateScale();
            lastScreenSize = currentScreenSize;
        }
    }

    private void UpdateScale()
    {
        if (mainCamera == null || spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        transform.localScale = new Vector3(
            cameraWidth / spriteWidth,
            cameraHeight / spriteHeight,
            transform.localScale.z);
    }

    private Vector2 GetCurrentScreenSize()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
        #endif
        
        return new Vector2(Screen.width, Screen.height);
    }
}
