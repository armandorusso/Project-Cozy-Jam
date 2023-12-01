using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnnouncementMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve XCurve;
    [SerializeField] private AnimationCurve YCurve;
    [SerializeField] public Transform StartPoint;
    [SerializeField] public Transform EndPoint;
    [SerializeField] private float AnimationLength;
    
    
    private bool _hasNewWaveCommenced;
    private bool _hasReachedHalfwayPoint;
    private float _timeElapsed;
    private float _animationLengthProgress;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    
    void Start()
    {
        _hasNewWaveCommenced = true;
        _startPosition = StartPoint.position;
        _endPosition = EndPoint.position;

        GameManager.StartWaveAnnouncementAnimationAction += OnNewWave;
    }

    void Update()
    {
        if (_hasNewWaveCommenced)
        {
            MoveWaveAnnouncement();
        }
    }

    private void MoveWaveAnnouncement()
    {
        // Make sure the time doesn't exceed the animation length
        _animationLengthProgress = Mathf.Clamp(_animationLengthProgress + Time.deltaTime, 0f, AnimationLength);

        if (_hasReachedHalfwayPoint)
            return;

        if ((AnimationLength / 2f - _animationLengthProgress) <= 0 && (AnimationLength / 2f - _animationLengthProgress) >= -0.8f)
        {
            StartCoroutine(PauseAtMidpoint());
            return;
        }
        
        _timeElapsed = Mathf.Clamp(_timeElapsed + Time.deltaTime, 0f, AnimationLength);
        // Divide by animation length to ensure that the parabolic movement lasts for the whole animation length
        var linearT = _timeElapsed / AnimationLength;
        
        // Use that value to get the associated curve values
        var verticalT = YCurve.Evaluate(linearT);
        var horizontalT = XCurve.Evaluate(linearT);

        float newX = Mathf.Lerp(_startPosition.x, _endPosition.x, linearT) + horizontalT;
        float newY = Mathf.Lerp(_startPosition.y, _endPosition.y, linearT) + verticalT;

        transform.position = new Vector2(newX, newY);
    }

    private IEnumerator PauseAtMidpoint()
    {
        _hasReachedHalfwayPoint = true;
        yield return new WaitForSeconds(0.1f);
        _hasReachedHalfwayPoint = false;
    }

    private void OnNewWave(bool isNewWaveStarted)
    {
        _hasNewWaveCommenced = isNewWaveStarted;

        if (_hasNewWaveCommenced)
        {
            transform.position = _startPosition;
            _timeElapsed = 0f;
            _animationLengthProgress = 0f;
        }
    }

    public void OnDestroy()
    {
        GameManager.StartWaveAnnouncementAnimationAction -= OnNewWave;
    }
}
