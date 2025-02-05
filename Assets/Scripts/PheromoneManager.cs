using UnityEngine;
using UnityEngine.Splines;

public class PheromoneManager : MonoBehaviour
{
    public SplineMovementManager movementManager;
    public GameState gameState;
    public Material indicatorMaterial;

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
