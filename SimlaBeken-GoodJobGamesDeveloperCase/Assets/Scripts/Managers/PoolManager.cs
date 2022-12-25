using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoolManager: MonoSingleton<PoolManager>
{
    public Pool<BlockBase> blockPool { get; } = new Pool<BlockBase>();
    [SerializeField] private BlockBase blockPrefab;
    private void Awake()
    {
        blockPool.Initialize(blockPrefab);
    }
}
