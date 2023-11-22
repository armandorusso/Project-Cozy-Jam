using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrailIntersection : MonoBehaviour
{
    private Player _player;
    [SerializeField] public LineRenderer LineRenderer;
    [SerializeField] public PolygonCollider2D TrailHitbox;
    [SerializeField] private SpriteRenderer TailSprite;
    [SerializeField] private float TailDistanceThreshold;
    [SerializeField] public float TimeBetweenPoints;
    [SerializeField] public float PointLifeTime;
    [SerializeField] private ParticleSystem BeeParticles;
    [SerializeField] private float ForceFieldLifeTime;
    

    private float _currentTime;
    private float _activePointLifetime;

    private int _currentIndex;
    private bool _hasTriggeredTrail;

    private List<Vector2> points;

    public static Action<bool, Vector2, float> BeeSwarmAction;
    
    void Start()
    {
        _player = GetComponent<Player>();
        points = new List<Vector2>();

        StartTrail();
        TailSprite.enabled = false;
    }

    void Update()
    {
        if (!_player.PlayerMovement.IsMoving) return;
        
        if (_hasTriggeredTrail)
        {
            _currentTime += Time.deltaTime;
            _activePointLifetime += Time.deltaTime;

            if (_currentTime >= TimeBetweenPoints)
            {
                SetNewLineRendererPoint();
                _currentTime = 0f;
            }

            TuneParticleLifetime();
            
            if (_activePointLifetime >= PointLifeTime || _currentIndex >= 19)
            {
                RemovePoint();
            }
            
            if (_hasTriggeredTrail && _currentIndex >= 4 && _currentIndex <= 19)
            {
                if (IsOverlapWithTail())
                {
                    Debug.Log("Tail Hit!");
                    CreateHitbox();
                    RemoveTrail();
                    _player.AnimatorComponent.Play("Connect");
                }
            }
        }
    }

    private void TuneParticleLifetime()
    {
        switch (_currentIndex)
        {
            case <= 10:
            {
                var main = BeeParticles.main;
                main.startLifetime = 0.75f;
                break;
            }
            case >= 10 and <= 15:
            {
                var main = BeeParticles.main;
                main.startLifetime = 1.45f;
                break;
            }
            default:
            {
                var main = BeeParticles.main;
                main.startLifetime = 1.56f;
                break;
            }
        }
    }

    private void RemovePoint()
    {
        Vector3[] pointsArr = new Vector3[LineRenderer.positionCount];
        LineRenderer.GetPositions(pointsArr);
        var pointList = pointsArr.ToList();
        pointList.RemoveAt(0);
        _currentIndex--;
        
        LineRenderer.SetPositions(pointList.ToArray());
        LineRenderer.positionCount--;
        TailSprite.transform.position = LineRenderer.GetPosition(0);
        _activePointLifetime = 0;
    }

    private void StartTrail()
    {
        _hasTriggeredTrail = true;
        SetNewLineRendererPoint();
        TailSprite.transform.position = LineRenderer.GetPosition(0);
        LineRenderer.enabled = true;
    }

    private void SetNewLineRendererPoint()
    {
        TailSprite.enabled = true;
        LineRenderer.positionCount++;
        LineRenderer.SetPosition(_currentIndex, transform.position);
        
        points.Add(transform.position);
        
        // TrailHitbox.SetPath(0, points);

        if(_currentIndex >= 1)
            Debug.DrawLine(LineRenderer.GetPosition(_currentIndex - 1), LineRenderer.GetPosition(_currentIndex), Color.green);
        
        _currentIndex++;
    }

    public bool IsOverlapWithTail()
    {
        return Mathf.Abs(Vector2.Distance(TailSprite.transform.position, transform.position)) <= TailDistanceThreshold;
    }

    private void CreateHitbox()
    {
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < LineRenderer.positionCount; i++)
        {
            points.Add(LineRenderer.GetPosition(i));
        }

        TrailHitbox.SetPath(0, points);
        TrailHitbox.enabled = true;
        Vector2 midpoint = Vector2.zero;

        foreach (var point in TrailHitbox.points)
        {
            midpoint += point;
        }

        var pointsLength = TrailHitbox.points.Length;
        midpoint /= pointsLength;
        
        // Average distance between the midpoint and each point
        float averageDistance = 0;
        
        foreach (var point in TrailHitbox.points)
        {
            averageDistance += Vector2.Distance(point, midpoint);
        }

        averageDistance /= pointsLength;

        BeeSwarmAction?.Invoke(true, midpoint, averageDistance);
        StartCoroutine(DisableHitbox());
    }

    private IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(ForceFieldLifeTime);
        TrailHitbox.pathCount = 0;
        TrailHitbox.enabled = false;
        BeeSwarmAction?.Invoke(false, TailSprite.transform.position, 0);
    }

    private void RemoveTrail()
    {
        _currentIndex = 0;
        _activePointLifetime = 0f;
        _currentTime = 0f;
        _hasTriggeredTrail = false;
        LineRenderer.enabled = false;
        LineRenderer.positionCount = 0;
        points.Clear();

        StartTrail();
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

    private void FindIntersection()
    {
        var line1Start = LineRenderer.GetPosition(0);
        Debug.Log(LineRenderer.GetPosition(0));
        var line1End = LineRenderer.GetPosition(1);

        Debug.Log("Current index " + _currentIndex);
        Debug.Log("Current line renderer count " + LineRenderer.positionCount++);
        var line2Start = LineRenderer.GetPosition(_currentIndex - 1);
        var line2End = LineRenderer.GetPosition(_currentIndex);

        var pointOfIntersection = FindPointOfIntersection(line1Start, line1End, line2Start, line2End);

        if (Vector2.Distance(pointOfIntersection, line1Start) <= 1f)
        {
            Debug.Log("Created Loop!");
            // CreateHitbox(i - 1, pointOfIntersection);
            RemoveTrail();
        }
    }
}
