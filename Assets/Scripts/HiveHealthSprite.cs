using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveHealthSprite : MonoBehaviour
{
    [SerializeField] public Sprite[] HiveHealthIcons;
    [SerializeField] public Image HiveHealthImage;
    
    void Start()
    {
        HiveHurt.HiveDamagedAction += OnHiveDamaged;
    }

    private void OnHiveDamaged(int currentHealth, int maxHealth)
    {
        var partitionSize = maxHealth / HiveHealthIcons.Length;
        var damageDifference = maxHealth - currentHealth;

        var spriteIndex = damageDifference / partitionSize;
        HiveHealthImage.sprite = HiveHealthIcons[spriteIndex];
    }

    private void OnDestroy()
    {
        HiveHurt.HiveDamagedAction -= OnHiveDamaged;
    }
}
