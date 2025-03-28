using UnityEngine;

public class Light2D : MonoBehaviour
{
    public float radius;
    public float intensity;
    public Color colour;

    public Color GetLightData(Camera camera)
    {
        Vector2 worldPos = transform.position;

        Vector2 screenPos = camera.WorldToScreenPoint(worldPos);
        Vector2 screenPosNormalized = new(
            screenPos.x/Screen.width, 
            screenPos.y/Screen.height
            );

        return new Color(screenPosNormalized.x, screenPosNormalized.y, radius, intensity);
    }


}
