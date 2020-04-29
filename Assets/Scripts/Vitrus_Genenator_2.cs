using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vitrus_Genenator_2 : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Image myImage;
    Texture2D texture;
    Sprite mySprite;
    int textureWidth = 256;
    int textureHeight = 256;

    public Color[] colors;

    [Header("Game of Live vars")]
    public int matrixSize = 32;
    public int cellSize = 4;
    public int radius = 20;

    [Header("Loop variables")]
    public bool paused = false;

    int[,] matrix_1;
    int[,] matrix_2;
    int[,] toRender;

    public float refreshTime = 1f;
    float timer;

    // definiert welche matrix zur Updatefunktion als Referenz und welche als Output weitergegeben wird
    bool actualMatrix = true;

    // Start is called before the first frame update
    void Start()
    {
        paused = true;
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
                SetRenderOutput();
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
                if (matrix_1[x, y] != 2)
                {
                    matrix_1[x, y] = UnityEngine.Random.Range(0, 2);
                }
            }
        }
    }

    public void PrepareEmptyDish()
    {
        timer = refreshTime;
        actualMatrix = true;
        PrepareRenderer();
        PrepareMatrixes();
    }

    public void StartSimulation()
    {
        SetRenderOutput();
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

    private void PrepareMatrixes()
    {
        // prepare matrix_1
        matrix_1 = MakePetriDishFromMatrix(matrixSize, radius);
        matrix_2 = MakePetriDishFromMatrix(matrixSize, radius);

        toRender = matrix_1;
        
    }

    private int[,] MakePetriDishFromMatrix(int size, int radius)
    {
        int[,] matrix = new int[size, size];
        // initialize matrix with 2 (status for the petridish)
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrix[i, j] = 2;
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
                    matrix[x, y] = 0;
                    matrix[x, center - y + center - 1] = 0;
                    matrix[center - x + center - 1, y] = 0;
                    matrix[center - (x - center) -1 , center - (y - center) - 1] = 0;
                }
            }
        }
        return matrix;
    }


    /// <summary>
    /// creates the Texture2D and assign it to the SpriteRenderer
    /// </summary>
    private void PrepareRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myImage = GetComponent<Image>();
        textureWidth = matrixSize * cellSize;
        textureHeight = matrixSize * cellSize;
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        Rect myRect = new Rect(0, 0, textureWidth, textureHeight);
        mySprite = Sprite.Create(texture, myRect, new Vector2(.5f, .5f));
        //spriteRenderer.sprite = mySprite;
        myImage.sprite = mySprite;
    }

    



    private void PlayRound()
    {
        if (actualMatrix)
        {
            GameOfLife.UpdateCellStatus(matrix_1, matrix_2);
            // funzione con matrix 1
            toRender = matrix_2;
        } else
        {
            GameOfLife.UpdateCellStatus(matrix_2, matrix_1);
            // funzione con matrix 2
            toRender = matrix_1;
        }

        actualMatrix = !actualMatrix;
    }


    private void SetRenderOutput()
    {
        Color[] cols = texture.GetPixels();

        for (int x = 0; x < matrixSize; x++)
        {
            for (int y = 0; y < matrixSize; y++)
            {

                    for (int xa = x * cellSize; xa < x * cellSize + cellSize; xa++)
                    {
                        for (int ya = y * cellSize; ya < y * cellSize + cellSize; ya++)
                        {   
                        cols[(xa * (matrixSize*cellSize)) + ya] = colors[toRender[x, y]];
                        }
                    }
               
            }

        }

        texture.SetPixels(cols);
        texture.Apply();
    }
}
