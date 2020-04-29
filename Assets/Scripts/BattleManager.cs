using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    public PlayerScript player_2;
    public PlayerScript player_1;
    Vitrus_Genenator_2 vitrusGen;

    [Header("Game of Live vars")]
    public int matrixSize = 32;
    public int cellSize = 4;
    public int radius = 20;

    public struct ColCel
    {
       public int gol;
    }

    ColCel[,] matrix_1;
    ColCel[,] matrix_2;
    ColCel[,] toRender;


    [Header("Loop variables")]
    public bool paused = false;

    float refreshTime;
    float timer;
    // definiert welche matrix zur Updatefunktion als Referenz und welche als Output weitergegeben wird
    bool actualMatrix = true;

    // Start is called before the first frame update
    void Start()
    {
        vitrusGen = FindObjectOfType<Vitrus_Genenator_2>();
        paused = true;
        PrepareMatrixes();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                PlayRound();
                vitrusGen.SetRenderOutput(ConvertColCelMatrixToIntMatrix(toRender));
                timer = refreshTime;
            }
        }
    }

    public void StandardStartOfGOL()
    {
        SetUpRandomGameOfLife();
        StartSimulation();
    }

    #region public functions


    public void SetUpRandomGameOfLife()
    {
        for (int x = 0; x < matrixSize; x++)
        {
            for (int y = 0; y < matrixSize; y++)
            {
                // if the cell is not outside the petridish make it alive or dead
                if (matrix_1[x, y].gol != 2)
                {
                    matrix_1[x, y].gol = UnityEngine.Random.Range(0, 2);
                }
            }
        }
    }

    public void PrepareEmptyDish()
    {
        timer = refreshTime;
        actualMatrix = true;
        vitrusGen.PrepareRenderer();
        PrepareMatrixes();
    }

    public void StartSimulation()
    {
        vitrusGen.SetRenderOutput(ConvertColCelMatrixToIntMatrix(toRender));
        paused = false;
    }

    public void PauseSimulation()
    {
        paused = true;
    }

    public void UnpauseSimulation()
    {
        paused = false;
    }

    public void SwitchPauseSimulation()
    {
        paused = !paused;
    }

    #endregion

    public void SetAPointInMatrix(int x, int y)
    {
        matrix_1[x, y].gol = 1;
        matrix_2[x, y].gol = 1;
        vitrusGen.SetRenderOutput(ConvertColCelMatrixToIntMatrix(toRender));
    }


    private void PrepareMatrixes()
    {
        matrix_1 = MakePetriDishFromMatrix(matrixSize, radius);
        matrix_2 = MakePetriDishFromMatrix(matrixSize, radius);

        toRender = matrix_1;
     

    }

    private void PlayRound()
    {
        if (actualMatrix)
        {
            GameOfLife.UpdateColCellStatus(matrix_1, matrix_2);
            // funzione con matrix 1
            toRender = matrix_2;
        }
        else
        {
            GameOfLife.UpdateColCellStatus(matrix_2, matrix_1);
            // funzione con matrix 2
            toRender = matrix_1;
        }

        actualMatrix = !actualMatrix;
    }

    private ColCel[,] MakePetriDishFromMatrix(int size, int radius)
    {
        ColCel[,] matrix = new ColCel[size, size];
        // initialize matrix with 2 (status for the petridish)
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrix[i, j].gol = 2;
            }
        }

        // make the matrixes a petriDish
        int center = (int)size / 2;
        // make sure that the radius is not larger than it can be
        //if (center - radius < 0) radius = center;
        for (int x = center - radius; x <= center; x++)
        {
            for (int y = center - radius; y <= center; y++)
            {
                if ((x - center) * (x - center) + (y - center) * (y - center) < radius * radius)
                {
                    matrix[x, y].gol = 0;
                    matrix[x, center - y + center - 1].gol = 0;
                    matrix[center - x + center - 1, y].gol = 0;
                    matrix[center - (x - center) - 1, center - (y - center) - 1].gol = 0;
                }
            }
        }
        return matrix;
    }

    private int[,] ConvertColCelMatrixToIntMatrix(ColCel[,] toConvert)
    {
        int[,] toReturn = new int[matrixSize,matrixSize];
        for(int x = 0; x < matrixSize; x++)
        {
            for(int y = 0; y < matrixSize; y++)
            {
                toReturn[x, y] = toConvert[x, y].gol;
            }
        }
        return toReturn;

    }


}
