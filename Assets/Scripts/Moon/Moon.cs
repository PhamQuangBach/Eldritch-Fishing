using Unity.VisualScripting;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField]
    private float maxTime = 600;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(Mathf.Lerp(-168, -168 - 90, GameManager.CurTime / maxTime), 0, 0);
    }
}
