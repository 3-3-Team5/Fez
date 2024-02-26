using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterBaseStats
{
    [field: SerializeField][field: Range(0, 25f)] public float MovementSpeed { get; private set; } = 2f;
    [field: SerializeField][field: Range(0, 25f)] public float jumpForce { get; private set; } = 5f;
}
