using UnityEngine;
using TMPro;

public class FishCounter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = FishManager.FishCaught.ToString();
    }
}
