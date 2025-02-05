using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineMovementManager : MonoBehaviour
{
    [SerializeField] private SplineContainer currentSpline; // initial spline is set in the inspector 
    [SerializeField] private float moveSpeed = 4f;          
    private float t = 0f;                                   // Current progress on spline [0..1]
    private int orientation = 1;                            // 1 = forward, -1 = backward

    // The player's fork choice (true = left, false = right, for example)
    private bool takeLeftFork = true;

    private bool move = false;


    void Update()
    {
        HandleInput();
        MoveAlongSpline();
    }


    private void HandleInput()
    {
        move = false;

        // TOGGLE MOVEMENT
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move = true;
        }

        // TOGGLE DIRECTION
        if (Input.GetKeyDown(KeyCode.T))
        {
            orientation *= -1;
        }

        // TOGGLE LEFT/RIGHT FORK
        if (Input.GetKeyDown(KeyCode.F))
        {
            takeLeftFork = !takeLeftFork;
        }
    }


    private void MoveAlongSpline()
    {
        // Change t based on direction, speed, and spline length
        float delta = orientation * moveSpeed / currentSpline.Spline.GetLength() * Time.deltaTime;
        if (move)
        {
            t += delta;
        }

        // Check if we reached the end of the spline
        if (t > 1f)
        {
            SwitchToNextSpline(true);
        }
        else if (t < 0f)
        {
            SwitchToNextSpline(false);
        }

        // Evaluate the spline at the current t to get position, tangent, and up.
        transform.position = currentSpline.Spline.EvaluatePosition(t) + (float3)currentSpline.transform.position;

        // Rotate the object along the spline
        // code copied from AnimateSpline.cs
        Vector3 forward = orientation > 0 ? Vector3.forward : Vector3.back;
        Vector3 up = Vector3.up;
        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(forward, up));
        forward = currentSpline.EvaluateTangent(t);
        if (Vector3.Magnitude(forward) <= Mathf.Epsilon)
        {
            if (t < 1f)
                forward = currentSpline.EvaluateTangent(Mathf.Min(1f, t + 0.01f));
            else
                forward = currentSpline.EvaluateTangent(t - 0.01f);
        }
        forward.Normalize();
        up = currentSpline.EvaluateUpVector(t);
            
        var valid = math.isfinite(forward) & math.isfinite(up);
            
        if (math.all(valid))
            transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
    }

    private void SwitchToNextSpline(bool atEnd)
    {
        t = math.clamp(t, 0f, 1f); 

        
        if (!currentSpline.TryGetComponent<SplineConnections>(out var splineConnections))
        {
            Debug.LogError("No SplineConnections component found on the current spline.");
            return;
        }

        // Get the next spline based on the current spline and the fork choice
        if (atEnd)
        {
            var next = takeLeftFork ? splineConnections.leftEnd : splineConnections.rightEnd;

            if (next.spline == null)
            {
                return;
            }

            currentSpline = next.spline;

            if (next.backward)
            {
                orientation = -1;
                t = 1f;
            }
            else
            {
                orientation = 1;
                t = 0f; 
            }
        }
        else
        {
            var next = takeLeftFork ? splineConnections.leftStart : splineConnections.rightStart;

            if (next.spline == null)
            {
                return;
            }

            currentSpline = next.spline;

            if (next.backward)
            {
                orientation = -1;
                t = 1f;
            }
            else
            {
                orientation = 1;
                t = 0f;
            }
        }
    }
}