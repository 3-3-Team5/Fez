using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Char", menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [field: SerializeField] public CharacterBaseStats baseStats { get; private set; }
}
