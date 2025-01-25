using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Footsteps collection", menuName = "Create New Footsteps Collection")]
public class FootStepsCollection : ScriptableObject
{
    public List<AudioClip> FootStepClips = new();
}