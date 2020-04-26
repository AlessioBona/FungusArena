using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitrus_Genenator_2 : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    Texture2D texture;
    Sprite mySprite;
    int textureWidth = 256;
    int textureHeight = 256;

    public Color[] colors;
    //public Color[] colors2;
    //int actualColor = 0;

    [Header("Game of Live vars")]
    public int matrixSize = 32;
    public int cellSize = 4;
    public int radius = 20;

    int[,] matrix_1;
    int[,] matrix_2;
    int[,] toRender;

    // definiert welche matrix zur Updatefunktion als Referenz und welche als Output weitergegeben wird
    bool actualMatrix = true;

    // Start is called before the first frame update
    void Start()
    {
        PrepareRenderer();
        Restart();
        //matrix_1 = new int[,] { {1 , 0 }, {0 , 1 } };
        //matrix_2 = matrix_1;
        //ChangeArray(matrix_2);
        //Debug.Log(matrix_1[0, 0]);


    }

    public void Restart()
    {
        PrepareMatrixes();
        timer = refreshTime;
        actualMatrix = true;
    }

    //private void ChangeArray(int[,] toChange)
    //{
    //    toChange[0, 0] = 0;
    //}

    private void PrepareMatrixes()
    {
        // prepare matrix_1
        matrix_1 = MakePetriDishFromMatrix(matrixSize, radius);
        matrix_2 = MakePetriDishFromMatrix(matrixSize, radius);

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

        toRender = matrix_1;
        SetRenderOutput();
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



    private void PrepareRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textureWidth = matrixSize * cellSize;
        textureHeight = matrixSize * cellSize;
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        Rect myRect = new Rect(0, 0, textureWidth, textureHeight);
        mySprite = Sprite.Create(texture, myRect, new Vector2(.5f, .5f));
        spriteRenderer.sprite = mySprite;
    }

    public float refreshTime = 1f;
    float timer;

    // Update is called once per frame
    void Update()
    {
        // mit timer
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            SetRenderOutput();
            PlayRound();
            timer = refreshTime;
        }
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

    private void FakeGame(int[,] intro, int[,] outro)
    {

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

        //Debug.Log("mips: " + texture.mipmapCount);
        texture.SetPixels(cols);
        texture.Apply();
    }
}
