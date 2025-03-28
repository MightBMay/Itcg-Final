using UnityEngine;

public class LightManager2D : MonoBehaviour
{
    public Material lightMaskMaterial; // Assign Shader Graph material
    public Light2D[] lights;        // Assign light GameObjects here (max 64)
    Camera targetCamera;       // The main camera reference

    [SerializeField] int maxLights = 64;

    [SerializeField]Texture2D lightDataTexture;


    void Start()
    {
        targetCamera = Camera.main;
        // Create the light data texture
        lightDataTexture = new Texture2D(maxLights, 1, TextureFormat.RGBAFloat, false);
        lightDataTexture.wrapMode = TextureWrapMode.Clamp;
        lightDataTexture.filterMode = FilterMode.Point;

        // Assign texture to the material
        lightMaskMaterial.SetTexture("_LightDataTex", lightDataTexture);
    }

    private void FixedUpdate()
    {
        UpdateLightData(lights, lights.Length);
    }

    public void UpdateLightData(Light2D[] lightPositions, int lightCount)
    {
        Color[] lightDataArray = new Color[maxLights];

        // Loop through each light and encode the screen space position into the texture
        for (int i = 0; i < maxLights; i++)
        {
            if (i < lightCount)
            {

                // Encode the normalized screen position in the texture (R and G channels)
                lightDataArray[i] = lightPositions[i].GetLightData(targetCamera);
            }
            else
            {
                lightDataArray[i] = Color.black; // No light
            }
        }

        // Apply the encoded data to the texture
        lightDataTexture.SetPixels(lightDataArray);
        lightDataTexture.Apply();

        // Pass the number of lights to the material
        lightMaskMaterial.SetInt("_LightCount", lightCount);
    }
}
