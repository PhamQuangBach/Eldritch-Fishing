using UnityEngine;

public class LightSource : MonoBehaviour
{
    private float lightTarget = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, lightTarget, 0.1f);
    }

    public void SetLightLevel(float light){
        lightTarget = light;
    }
}
