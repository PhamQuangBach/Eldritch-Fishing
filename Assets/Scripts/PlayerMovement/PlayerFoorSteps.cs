using UnityEngine;


public class PlayerFootSteps : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private FootStepsCollection defaultFootSteps;

    [SerializeField]
    private float footStepDelay = 0.5f;

    private int currentFootStepIndex = 0;

    private AudioSource audioSource;

    private float footStepTimer = 0f;

    private FootStepsCollection currentFootStepsCollection;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        currentFootStepsCollection = defaultFootSteps;
    }

    private void Update()
    {
        PlayFootStepSound();
    }

    private void PlayFootStepSound()
    {
        if (!(playerMovement.IsMoving && playerMovement.IsMoving))
        {
            audioSource.Stop();
            footStepTimer = footStepDelay;
            return;
        }

        if (footStepTimer >= footStepDelay)
        {
            footStepTimer = 0f;

            AudioClip footStep = currentFootStepsCollection.FootStepClips[currentFootStepIndex];
            audioSource.clip = footStep;
            audioSource.Play();

            if (currentFootStepIndex == defaultFootSteps.FootStepClips.Count - 1)
            {
                currentFootStepIndex = 0;
            }

            currentFootStepIndex++;
        }
        
        footStepTimer += Time.deltaTime;
    }

    public void SetFootStepCollection(FootStepsCollection collection)
    {
        audioSource.Stop();
        
        currentFootStepIndex = 0;
        currentFootStepsCollection = collection;
        audioSource.clip = collection.FootStepClips[currentFootStepIndex];

        audioSource.Play();

        footStepTimer = 0f;
    }

    public void ResetFootStepCollection()
    {
        audioSource.Stop();

        currentFootStepIndex = 0;
        currentFootStepsCollection = defaultFootSteps;
        audioSource.clip = defaultFootSteps.FootStepClips[currentFootStepIndex];

        audioSource.Play();

        footStepTimer = 0f;
    }
}