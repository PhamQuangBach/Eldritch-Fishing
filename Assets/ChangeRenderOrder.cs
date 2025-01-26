using UnityEngine;

public class ChangeRenderOrder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 1500;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
