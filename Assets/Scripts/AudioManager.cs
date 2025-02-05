using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Objects to Monitor")]
    public GameObject[] objectsToCheck;

    [Header("Audio")]
    public AudioSource audioSource;     // The audio source to play the sound
    public AudioClip activationSound;   // Sound to play when an object becomes active

    private bool[] previousStates;      // Remember the previous state of each object

    void Start()
    {
        // Initialize the previousStates array to match the length of objectsToCheck
        previousStates = new bool[objectsToCheck.Length];
        for (int i = 0; i < objectsToCheck.Length; i++)
        {
            previousStates[i] = objectsToCheck[i].activeSelf;
        }
    }

    void Update()
    {
        for (int i = 0; i < objectsToCheck.Length; i++)
        {
            bool currentActive = objectsToCheck[i].activeSelf;

            // Check if it transitioned from inactive to active, if so play the sound
            if (currentActive && !previousStates[i])
            {
                audioSource.PlayOneShot(activationSound);
            }

            // Update the stored state for next frame
            previousStates[i] = currentActive;
        }
    }
}