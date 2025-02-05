using UnityEngine;
using UnityEngine.Splines;

public class CreatureManager : MonoBehaviour
{
    [Header("Manager References")]
    public GameState gameState;
    public SplineMovementManager movementManager;

    
    [Header("Locations")]
    public SplineContainer guardAntSpline;
    public SplineContainer[] centipedeSplines;
    public SplineContainer spiderSpline;
    public SplineContainer queenSpline;
    public SplineContainer flyersSpline;
    
    [Header("Creatures")]
    public GameObject guardAnt;
    public GameObject centipede;
    public GameObject spider;
    public GameObject flyers;

    void Update()
    {
        // decide what creatures to show based on the current spline and game state
        guardAnt.SetActive(movementManager.currentSpline == guardAntSpline && !gameState.sprayedEntry);
        foreach (SplineContainer centipedeSpline in centipedeSplines)
        {
            centipede.SetActive(false);
            if (movementManager.currentSpline == centipedeSpline)
            {
                centipede.SetActive(true);
                break;
            }
            
        }
        spider.SetActive(movementManager.currentSpline == spiderSpline && !gameState.sprayedSpider);
        flyers.SetActive(movementManager.currentSpline == flyersSpline && gameState.photoQueen);
    }
}
