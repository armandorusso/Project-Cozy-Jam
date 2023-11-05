using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator AnimatorComponent => _animator;

    public void EnableObject()
    {
        _animator.enabled = true;
    }
    
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
