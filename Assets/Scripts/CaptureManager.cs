using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;  // For RawImage

class CaptureManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Camera snapshotCamera;        // The second camera
    public RawImage virtualScreen;       // The RawImage where we display the snapshot

    [Header("Capture Settings")]
    public int captureWidth = 400;
    public int captureHeight = 300;

    [Header("Effect Settings")]
    [Range(0f,0.2f)] public float noiseStrength = 0.05f;

    public GameState gameState;

    public SplineMovementManager movementManager;

    public PhotoLocation[] photoLocations;


    [System.Serializable]
    public struct PhotoLocation
    {
        public bool isPhotoTaken;
        public SplineContainer spline;
        public float t;
    }



    private Texture2D capturedTexture;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CaptureSnapshot();
            CheckPhotoLocations();
        }

        gameState.photoEntry = photoLocations[0].isPhotoTaken;
        gameState.photoQueen = photoLocations[1].isPhotoTaken;
        gameState.photoVentilation = photoLocations[2].isPhotoTaken;
        gameState.photoWaste = photoLocations[3].isPhotoTaken;
        gameState.photoSpider = photoLocations[4].isPhotoTaken;
        gameState.photoEggs = photoLocations[5].isPhotoTaken;
        gameState.photoFood = photoLocations[6].isPhotoTaken;
        gameState.photoIntruder = photoLocations[7].isPhotoTaken;
        gameState.photoFlight = photoLocations[8].isPhotoTaken;
    }

    void CaptureSnapshot()
    {
        // Temporarily enable the camera if it's disabled
        bool wasEnabled = snapshotCamera.enabled;
        snapshotCamera.enabled = true;

        // Create a new RenderTexture (in code) with desired dimensions
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        snapshotCamera.targetTexture = rt;

        // Render the camera's view to the RenderTexture
        snapshotCamera.Render();

        // Read the RenderTexture into a new Texture2D
        RenderTexture.active = rt;
        if (capturedTexture == null)
        {
            capturedTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }
        capturedTexture.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        capturedTexture.Apply();

        // Apply effects
        FilterTexture(capturedTexture);

        // Show this texture on the RawImage (virtual screen)
        virtualScreen.texture = capturedTexture;

        // Cleanup
        snapshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Return camera to its original enabled state
        snapshotCamera.enabled = wasEnabled;
    }

    void FilterTexture(Texture2D tex)
    {
        Color[] pixels = tex.GetPixels();
        int size = pixels.Length;

        // Apply grayscale
        for (int i = 0; i < size; i++)
        {
            Color c = pixels[i];
            float gray = (c.r + c.g + c.b) / 3f;
            c.r = c.g = c.b = gray;
            pixels[i] = c;
        }

        // Apply noise
        for (int i = 0; i < size; i++)
        {
            float noise = UnityEngine.Random.Range(-noiseStrength, noiseStrength);
            float val = pixels[i].r + noise; // r=g=b because of grayscale
            val = Mathf.Clamp01(val);

            pixels[i] = new Color(val, val, val, 1f);
        }

        tex.SetPixels(pixels);
        tex.Apply();
    }

    void CheckPhotoLocations()
    {
        for (int i = 0; i < photoLocations.Length; i++)
        {
            if (movementManager.currentSpline != photoLocations[i].spline) 
            {
                continue;
            }
            
            // check if the t values are close enough
            if (math.abs(movementManager.t - photoLocations[i].t) > 0.05f)
            {
                continue;
            }

            // check if the orientation is correct
            if (photoLocations[i].t < 0.2f && movementManager.orientation > 0)
            {
                continue;
            }
            if (photoLocations[i].t > 0.8f && movementManager.orientation < 0)
            {
                continue;
            }
            photoLocations[i].isPhotoTaken = true;
        }
    }
}