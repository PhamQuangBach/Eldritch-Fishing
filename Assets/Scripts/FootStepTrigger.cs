using UnityEngine;



public class FootStepTrigger : MonoBehaviour
{
    [SerializeField]
    private FootStepsCollection footStepsCollection;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null)
            return;

        player.ChangeFootSteps(footStepsCollection);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null)
            return;

        player.ResetFootSteps();
    }
}