using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrailIntersection : MonoBehaviour
{
    [SerializeField] public LineRenderer LineRenderer;

    private float _currentTime;
    private float _timeBetweenPoints = 2f;

    private int _currentIndex;
    private bool _hasTriggeredTrail;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (_hasTriggeredTrail)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _timeBetweenPoints)
            {
                SetNewLineRendererPoint();
                _currentTime = 0f;
            }
        }
    }
    
    public void OnTrailTrigger(InputAction.CallbackContext context)
    {
        if (context.started && !_hasTriggeredTrail)
        {
            _hasTriggeredTrail = true;
            LineRenderer.enabled = true;
        }

        if (context.started && _hasTriggeredTrail)
        {
            if (_currentIndex >= 4 && _currentIndex <= 10)
            {
                FindIntersection();
            }
        }
    }

    private void SetNewLineRendererPoint()
    {
        LineRenderer.SetPosition(_currentIndex, transform.position);
        
        if(_currentIndex >= 1)
            Debug.DrawLine(LineRenderer.GetPosition(_currentIndex - 1), LineRenderer.GetPosition(_currentIndex), Color.green);
        
        _currentIndex++;
    }

    private void FindIntersection()
    {
        var line1Start = LineRenderer.GetPosition(0);
        var line1End = LineRenderer.GetPosition(1);
        
        var line2Start = LineRenderer.GetPosition(_currentIndex - 1);
        var line2End = LineRenderer.GetPosition(_currentIndex);

        var pointOfIntersection = FindPointOfIntersection(line1Start, line1End, line2Start, line2End);

        if (Vector2.Distance(pointOfIntersection, line1Start) <= 3f)
        {
            Debug.Log("Created Loop!");
        }

        TurnOffTrail();
    }

    private void TurnOffTrail()
    {
        _currentIndex = 0;
        _hasTriggeredTrail = false;
        LineRenderer.enabled = false;
        LineRenderer.positionCount = 0;
        LineRenderer.positionCount = 20;
    }

    private Vector2 FindPointOfIntersection(Vector3 line1Start, Vector3 line1End, Vector3 line2Start, Vector3 line2End)
    {
        // First Line
        var A1 = line1End.y - line1Start.y;
        var B1 = line1Start.x - line1End.x;
        var C1 = A1 * line1Start.x + B1 * line1Start.y;
        
        // Second Line
        var A2 = line2End.y - line2Start.y;
        var B2 = line2Start.x - line2End.x;
        var C2 = A2 * line2Start.x + B2 * line2Start.y;

        var determinant = A1 * B2 + A2 * B1;

        if (determinant == 0) // lines are parallel
            return Vector2.zero;

        var x = (B2 * C1 - B1 * C2) / determinant;
        var y = (A1 * C2 - A2 * C1) / determinant;

        return new Vector2(x, y);
    }

    private float CalculateSlope(Vector3 firstPoint, Vector3 secondPoint)
    {
        return (secondPoint.y - firstPoint.y) / (secondPoint.x - firstPoint.x);
    }
}
