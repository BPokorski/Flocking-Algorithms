using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    [Header("Boid parameters")]
    [Range(1f, 8f)][SerializeField] private float _minSpeed = 2f;
    [Range(1f, 8f)][SerializeField] private float _maxSpeed = 5f;
    [Range(1f, 5f)][SerializeField] private float _rotationSpeed = 5f;
    [Range(180f, 360f)][SerializeField] private float _fieldOfView = 270f;
    
    public float currentSpeed = 0;
    public HashSet<Boid> neighbours = new HashSet<Boid>();
    public Transform TransformProperty 
    { get ; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        TransformProperty = transform;
        currentSpeed = Random.Range(_minSpeed, _maxSpeed);
    }

    public Vector3 GetRotationDirection(Bounds bounds, Vector3 targetPosition, float obstacleDistance)
    {
        Vector3 direction = Vector3.zero;
        RaycastHit hit = new RaycastHit();
        if (!bounds.Contains(TransformProperty.position))
        {
            direction = targetPosition - TransformProperty.position;
        }
        else if (Physics.Raycast(TransformProperty.position, TransformProperty.forward * obstacleDistance, out hit)) 
        {
            
            direction = Vector3.Reflect(TransformProperty.forward, hit.normal);
        }

        return direction;
    }
    public void RotateTowardDirection(Vector3 direction)
    {
        TransformProperty.rotation = Quaternion.Slerp(TransformProperty.rotation,
            Quaternion.LookRotation(direction),
            _rotationSpeed * Time.deltaTime);
    }
    
    public Vector3 CalculateFlocking(float cohesionDistance, float separationDistance, 
        Vector3 targetPosition)
    {
        // return Vector3.zero;
        Vector3 direction = Vector3.zero;
        
        float groupSpeed = 0.01f;
        int groupSize = 0;
        int avoidanceGroupSize = 0;
        Vector3 averageGroupPosition = Vector3.zero;
        Vector3 averageAvoidance = Vector3.zero;

        foreach (var neighbour in neighbours)
        {
            if (neighbour.GetInstanceID() == GetInstanceID())
            {
                continue;
            }
            
            float neighbourDistanceSqrt = (neighbour.TransformProperty.position
                                           - TransformProperty.position).sqrMagnitude;

            float neighbourAngle = Vector3.Angle(
                TransformProperty.forward,
                neighbour.TransformProperty.position - TransformProperty.position);
            if (neighbourDistanceSqrt <= (cohesionDistance * cohesionDistance) && neighbourAngle < (_fieldOfView/2f))
            {
                averageGroupPosition += neighbour.transform.position;
                groupSize++;
                if (neighbourDistanceSqrt <= (separationDistance * separationDistance))
                {
                    averageAvoidance += (TransformProperty.position - neighbour.TransformProperty.position);
                    avoidanceGroupSize++;
                }
                groupSpeed += neighbour.currentSpeed;
            }
        }

        if (groupSize > 0)
        {
            averageGroupPosition =
                (averageGroupPosition / groupSize) + (targetPosition - TransformProperty.position);
            
            currentSpeed = Mathf.Clamp(groupSpeed / groupSize, _minSpeed, _maxSpeed);
            
            direction = (averageGroupPosition + averageAvoidance) - TransformProperty.position;
            
            if (direction != Vector3.zero)
            {
                TransformProperty.rotation = Quaternion.Slerp(TransformProperty.rotation, 
                    Quaternion.LookRotation(direction), 
                    _rotationSpeed * Time.deltaTime);
            }
        }

        return direction;
    }
    
    public void Draw(float size)
    {
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawWireSphere(TransformProperty.position, size);
        
        float halfFOV = _fieldOfView / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
        Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
        
        Quaternion upRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.right);
        Quaternion downRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.right);
        //
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        
        Vector3 upRayDirection = upRayRotation * transform.forward;
        Vector3 downRayDirection = downRayRotation * transform.forward;
        
        Gizmos.color = new Color(1, 1 ,1);
        Gizmos.DrawRay( transform.position, leftRayDirection * size );
        Gizmos.DrawRay( transform.position, rightRayDirection * size );
        Gizmos.DrawRay( transform.position, upRayDirection * size );
        Gizmos.DrawRay( transform.position, downRayDirection * size );
    }
}
