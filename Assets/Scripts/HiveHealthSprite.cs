using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHealthSprite : MonoBehaviour
{
    [SerializeField] public Sprite[] HiveHealthIcons;
    [SerializeField] public SpriteRenderer HiveHealthSpriteRenderer;
    
    void Start()
    {
        HiveHurt.HiveDamagedAction += OnHiveDamaged;
    }

    private void OnHiveDamaged(int currentHealth, int maxHealth)
    {
        var partitionSize = maxHealth / HiveHealthIcons.Length;
        var damageDifference = maxHealth - currentHealth;

        var spriteIndex = damageDifference / partitionSize;
        HiveHealthSpriteRenderer.sprite = HiveHealthIcons[spriteIndex];
    }

    private void OnDestroy()
    {
        HiveHurt.HiveDamagedAction -= OnHiveDamaged;
    }
}
