using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour, ISpawned, IDespawned
{
    #region Variables

    public SpriteRenderer spriteRenderer;

    private Vector3 firstPos;
    private Pool<BlockBase> blockPool;

    #endregion

    #region

    private void Start()
    {
        firstPos = transform.position;
    }

    #endregion

    #region Other Methods

    public void OnSpawned()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawned()
    {
        gameObject.transform.position = firstPos;
    }

    #endregion
}
