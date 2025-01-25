using Unity.VisualScripting;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    private float lightTarget = 6;

    private float lightModifier = 1;

    private Transform mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xRotation = Mathf.Asin(mainCamera.transform.forward.y) * Mathf.Rad2Deg;

        if (xRotation > 20){
            lightModifier = Mathf.Lerp(1, 0, Mathf.Clamp01((xRotation - 20) / 30));
            lightModifier = 1;
        }
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, lightTarget * lightModifier, 0.1f);
    }

    public void SetLightLevel(float light){
        lightTarget = light;
    }
}
