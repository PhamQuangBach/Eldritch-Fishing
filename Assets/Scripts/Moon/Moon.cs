using Unity.VisualScripting;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField]
    private float maxTime = 600;

    [SerializeField]
    private Material moonMaterial;

    [SerializeField]
    private Light pointLight;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = GameManager.CurTime / maxTime;
        transform.eulerAngles = new Vector3(Mathf.Lerp(120, 0, t), 0, 0);
        moonMaterial.SetColor("_Color", Color.Lerp(new Color(0.6f, 0.6f, 0.6f, 1), new Color(0.5f, 0, 0, 1), t));
        RenderSettings.fogColor = Color.Lerp(new Color(0f, 0f, 0f, 1), new Color(0.05f, 0, 0, 1), t);
        pointLight.color = Color.Lerp(Color.white, Color.red, t);
        pointLight.intensity = Mathf.Lerp(0, 0.05f, t);
    }
}
