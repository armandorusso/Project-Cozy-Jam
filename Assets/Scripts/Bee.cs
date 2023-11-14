using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] public int Index;
    [SerializeField] public Transform ObjectToFollow;
    [SerializeField] private float MovementSpeed;

    void Update()
    {
        FollowBeeOrPlayer();
    }

    private void FollowBeeOrPlayer()
    {
        if (Vector2.Distance(gameObject.transform.position, ObjectToFollow.position) >= 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, ObjectToFollow.transform.position,
                MovementSpeed * Time.deltaTime);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // Call event to trigger bee attack
    }
}
