using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calculs
{
    public static float LinearDistance;
    public static Vector2 FirstPosition;
    private static float offset = 0.1f;
    public static void CalculateDistances(BoxCollider2D coll, float size)
    {
        LinearDistance = coll.size.x / size;
        FirstPosition = new Vector2(-size / 2f, size / 2f);
    }
    public static Vector2 CalculatePoint(int x, int y)
    {
        return FirstPosition + new Vector2(x * LinearDistance, -y* LinearDistance);
    }
    public static int EvaluateWin(int[,] matrix)
    {
        int size = matrix.GetLength(0);
        int counterX = 0;
        int counterY = 0;
        int counterD1 = 0;
        int counterD2 = 0;

        for (int i = 0; i < size; i++)
        {
            counterX = 0;
            counterY = 0;
            for (int j = 0; j < size; j++)
            {
                counterY += matrix[i, j];
                counterX += matrix[j, i];
            }
            if (counterY == size || counterX == size) return 1;
            if (counterY == -size || counterX == -size) return -1;

            counterD1 += matrix[i, i];
            counterD2 += matrix[size - 1 - i, i];
        }

        if (counterD1 == size || counterD2 == size) return 1;
        if (counterD1 == -size || counterD2 == -size) return -1;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (matrix[i, j] == 0) return 2;
            }
        }
        return 0; // 0 empate, 1 gana jugador 1, -1 gana IA, 2 no ha terminado
    }
    public static bool CheckIfValidClick(Vector2 mousePosition, int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Vector2 point = CalculatePoint(i, j);
                if (Mathf.Abs(mousePosition.x - point.x) < LinearDistance / 2f - offset
                    && Mathf.Abs(mousePosition.y - point.y) < LinearDistance / 2f - offset)
                {
                    if (matrix[i, j] == 0)
                    {
                        GameManager.Instance.DoMove(i, j, 1);
                        return true;
                    }
                }
                //Debug.Log(CalculatePoint(i, j));
            }
        }
        return false;
    }

    public static (int, int) GetBestMove(int[,] matrix)
    {
        int bestScore = int.MinValue;
        int moveX = -1;
        int moveY = -1;
        int size = matrix.GetLength(0);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (matrix[i, j] == 0)
                {
                    matrix[i, j] = -1; 
                    int score = Minimax(matrix, 0, false, int.MinValue, int.MaxValue);
                    matrix[i, j] = 0; 
                    if (score > bestScore)
                    {
                        bestScore = score;
                        moveX = i;
                        moveY = j;
                    }
                }
            }
        }
        return (moveX, moveY);
    }

    private static int Minimax(int[,] matrix, int depth, bool isMaximizing, int alpha, int beta)
    {
        int result = EvaluateWin(matrix);
        if (result != 2)
        {
            // Si la IA (-1) gana, el score es alto. Si el humano (1) gana, el score es bajo.
            if (result == -1) return 10 - depth;
            if (result == 1) return -10 + depth;
            return 0;
        }

        int size = matrix.GetLength(0);
        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = -1;
                        int score = Minimax(matrix, depth + 1, false, alpha, beta);
                        matrix[i, j] = 0;
                        bestScore = Mathf.Max(score, bestScore);
                        alpha = Mathf.Max(alpha, bestScore);
                        if (beta <= alpha) break;
                    }
                }
                if (beta <= alpha) break;
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = 1;
                        int score = Minimax(matrix, depth + 1, true, alpha, beta);
                        matrix[i, j] = 0;
                        bestScore = Mathf.Min(score, bestScore);
                        beta = Mathf.Min(beta, bestScore);
                        if (beta <= alpha) break;
                    }
                }
                if (beta <= alpha) break;
            }
            return bestScore;
        }
    }
}
