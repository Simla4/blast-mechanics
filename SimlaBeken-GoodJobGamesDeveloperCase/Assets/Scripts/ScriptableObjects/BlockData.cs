using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlockData", order = 1)]
public class BlockData : ScriptableObject
{
    public Sprite defaultSprite;
    public Sprite firstSprite;
    public Sprite secondSprite;
    public Sprite thirthSprite;
}
