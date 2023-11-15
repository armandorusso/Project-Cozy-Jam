using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAlly : MonoBehaviour
{
    [SerializeField] public int Index;
    [SerializeField] public Transform ObjectToFollow;
    [SerializeField] private float MovementSpeed;
    [SerializeField] public float DistanceBetweenBees; // If MovementSpeed and _timeInterval is very high, it will look as though the movement stutters 

    public Vector2 PreviousPosition;
    private float _currentTime;

    private void Start()
    {
        PreviousPosition = ObjectToFollow.transform.position;
    }

    void Update()
    {
        FollowBeeOrPlayer();
    }

    private void FollowBeeOrPlayer()
    {
        _currentTime += Time.deltaTime;
        Vector2 currentPosition = transform.position;
        
        if (currentPosition != PreviousPosition)
        {
            transform.position = Vector2.Lerp(currentPosition, PreviousPosition,
                MovementSpeed * Time.deltaTime);
        }
        
        // Acts as a comparison of distance between each bee. This ensures that there is a gap between each bee
        // DistanceBetweenBees acts as a time interval threshold
        // This "delay" will ensure that it has more time to lerp to PreviousPosition 
        if (_currentTime >= DistanceBetweenBees)
        {
            PreviousPosition = ObjectToFollow.transform.position;
            _currentTime = 0f;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // Call event to trigger bee attack
    }
}
