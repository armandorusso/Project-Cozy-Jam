using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    public BeeCongoLine.BeeType _attackingBeeType;
    
    private Enemy _enemy;
    private bool _isGoingToDie;

    public static Action<GameObject> EnemyDeathAction;
    
    void Start()
    {
        BeeAttack.KillEnemyAction += OnEnemySwarmed;
        _enemy = GetComponent<Enemy>();
        _attackingBeeType = BeeCongoLine.BeeType.None;
    }

    private void OnEnemySwarmed(GameObject enemy, BeeCongoLine.BeeType attackingBeeType)
    {
        if (gameObject == enemy)
        {
            _isGoingToDie = true;
            _attackingBeeType = attackingBeeType;
        }
    }

    private void KillEnemy()
    {
        StartCoroutine(DelayDeath());
    }

    private IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(0.128f);
        GameManager.Instance.CurrentEnemiesOnScene--;

        try
        {
            var hurtEffectTransform = _enemy.transform.GetChild(0);
            hurtEffectTransform.gameObject.SetActive(true);
            hurtEffectTransform.parent = null;
            hurtEffectTransform.localScale = Vector3.one;
            if (hurtEffectTransform.TryGetComponent<HurtEffect>(out var hurtEffect))
            {
                hurtEffect.EnableObject();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("Couldn't play the hurt effect");
        }
        finally
        {
            EnemyDeathAction?.Invoke(gameObject);
            Destroy(gameObject.GetComponent<Enemy>().EnemyAnimator.AnimatorComponent.gameObject);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("YellowBee") || col.CompareTag("BlackBee")) && _isGoingToDie)
        {
            if (col.tag.Equals(_attackingBeeType.ToString()))
            {
                _isGoingToDie = false;
                KillEnemy();
            }
        }
    }

    private void OnDestroy()
    {
        BeeAttack.KillEnemyAction -= OnEnemySwarmed;
    }
}
