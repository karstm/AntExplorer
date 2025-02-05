using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;          // The 3D object we want to track (y,z pos, x rot)
    public RectTransform mapImageRect;      // The RectTransform of your MapImage
    public RectTransform iconRect;          // The RectTransform of your MapIcon

    [Header("Calibration Points")]
    public CalibrationData cal1;            // Calibration data for the first point
    public CalibrationData cal2;            // Calibration data for the second point

    [System.Serializable]
    public struct CalibrationData
    {
        public Vector2 target_point;
        public Vector2 icon_point;
    }

    private float alpha_x, beta_x, alpha_y, beta_y;     // Variables for the linear transformation

    void Start()
    {
        // Calculate the linear transformation
        alpha_x = (cal2.icon_point.x - cal1.icon_point.x) / (cal2.target_point.x - cal1.target_point.x);
        beta_x = cal1.icon_point.x - alpha_x * cal1.target_point.x;

        alpha_y = (cal2.icon_point.y - cal1.icon_point.y) / (cal2.target_point.y - cal1.target_point.y);
        beta_y = cal1.icon_point.y - alpha_y * cal1.target_point.y;
    }

    void Update()
    {
            if (targetObject == null) return;

            // Get the object's y/z position
            float objZ = targetObject.position.z;
            float objY = targetObject.position.y;

            // Convert to map pixel coordinates
            float mapX = alpha_x * objZ + beta_x;
            float mapY = alpha_y * objY + beta_y;

            // Set the icon position 
            iconRect.anchoredPosition = new Vector2(mapX, mapY);

            // Get the targets forward vector
            Vector3 forward = targetObject.forward;
            Vector2 forward2D = new(forward.z, forward.y);

            // Calculate the angle
            float angle = 180 - Mathf.Atan2(forward2D.y, forward2D.x) * Mathf.Rad2Deg;

            // Apply to icon
            Vector3 iconEuler = iconRect.localEulerAngles;
            iconEuler.z = angle;
            iconRect.localEulerAngles = iconEuler;
    }
}
