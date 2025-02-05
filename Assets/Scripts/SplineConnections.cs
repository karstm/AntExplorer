using UnityEngine;
using UnityEngine.Splines;

public class SplineConnections : MonoBehaviour
{
    /// <summary>
    /// Spline connections to take at the start and end of the track.
    /// Tick the "Backward" checkbox if the next spline should be taken in reverse.
    /// </summary>
    /// 
    [System.Serializable]
    public struct SplinePath
    {
        public SplineContainer spline;
        public bool backward;
    }

    [Header("Start Splines")]
    public SplinePath leftStart;
    public SplinePath rightStart;

    [Header("End Splines")]
    public SplinePath leftEnd;
    public SplinePath rightEnd;
}