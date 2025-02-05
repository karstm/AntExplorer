using UnityEngine;
using UnityEngine.Splines;

public class PheromoneManager : MonoBehaviour
{
    [Header("Manager References")]
    public SplineMovementManager movementManager;
    public GameState gameState;
    
    [Header("Indicator Material")]
    public Material indicatorMaterial;

    [Header("Spline References")]
    public SplineContainer entrySpline;
    public SplineContainer[] alarmSplines;
    public SplineContainer[] sexSplines;
    public SplineContainer spiderSpline;
    
    private int Pheromone = 0;

    void Start()
    {
        indicatorMaterial.color = Color.grey;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {   
            CheckPheromone();    
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SprayPheromone();
        }  
    }


    public void CheckPheromone()
    {
        // Check if the player is on a specific spline and fullfills the requirements and set the pheromone accordingly

        if (movementManager.currentSpline == entrySpline && movementManager.t <= 0.55f && movementManager.t >= 0.2f)
        {
            Pheromone = 1;
            indicatorMaterial.color = Color.green;
            return;
        }
        
        foreach (SplineContainer alarmSpline in alarmSplines)
        {
            if (movementManager.currentSpline == alarmSpline)
            {
                Pheromone = 2;
                indicatorMaterial.color = Color.red;
                gameState.smelledWarriors = true;
                return;
            }
        }

        foreach (SplineContainer sexSpline in sexSplines)
        {
            if (movementManager.currentSpline == sexSpline && gameState.photoQueen)
            {
                Pheromone = 3;
                indicatorMaterial.color = Color.magenta;
                gameState.smelledSex = true;
                return;
            }
        }

        Pheromone = 0;
        indicatorMaterial.color = Color.grey;
    }

    public void SprayPheromone()
    {
        // Check if the player is on a specific spline and fullfills the requirements and set the game state accordingly
        
        if (movementManager.currentSpline == spiderSpline && Pheromone == 2)
        {
            gameState.sprayedSpider = true;
        }
        else if (movementManager.currentSpline == entrySpline && Pheromone == 1 && movementManager.t <= 0.55f && movementManager.t >= 0.45f)
        {
            gameState.sprayedEntry = true;
        }
        Pheromone = 0;
        indicatorMaterial.color = Color.grey;
    }
}
