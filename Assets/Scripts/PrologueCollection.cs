using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Prologue collection", menuName = "Create New Prologue Collection")]
public class PrologueCollection : ScriptableObject
{
    public List<string> prologues = new();
}