using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    #region Variables

    [SerializeField] private BlockData blockData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    #endregion

    #region Callbacks

    private void Start()
    {
        spriteRenderer.sprite = blockData.defaultSprite;
    }

    #endregion
}
