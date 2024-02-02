using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{

    [SerializeField]
    public BlockController _blockPrefab;

    private List<BlockController> _inGameBlocks;
    // Start is called before the first frame update
    void Start()
    {
        _inGameBlocks = new List<BlockController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBlockAt(Vector2 position, int level, PowerUpManager.PowerUpType? powerUpWithin = null)
    {
        BlockController block = Instantiate(_blockPrefab, position, Quaternion.identity);
        if (powerUpWithin != null)
        {
            block.SetPowerUpDrop((PowerUpManager.PowerUpType) powerUpWithin);
        }
            block.SetCloudLevel(level);
    }
}
