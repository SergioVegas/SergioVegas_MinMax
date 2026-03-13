using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum States
{
    CanMove,
    CantMove,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoxCollider2D collider;
    public GameObject token1, token2;
    public int Size = 3;
    public int[,] Matrix;
    [SerializeField] private States state = States.CanMove;
    public Camera camera;

    private List<GameObject> spawnedTokens = new List<GameObject>();

    void Start()
    {
        Instance = this;
        InitGame();
    }

    void InitGame()
    {
        Matrix = new int[Size, Size];
        Calculs.CalculateDistances(collider, Size);
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Matrix[i, j] = 0;
            }
        }
        state = States.CanMove;
    }

    private void Update()
    {
        if (state == States.CanMove)
        {
            Vector3 m = Input.mousePosition;
            m.z = 10f;
            Vector3 mousepos = camera.ScreenToWorldPoint(m);
            if (Input.GetMouseButtonDown(0))
            {
                if (Calculs.CheckIfValidClick((Vector2)mousepos, Matrix))
                {
                    state = States.CantMove;
                    int winStatus = Calculs.EvaluateWin(Matrix);
                    if (winStatus == 2)
                        StartCoroutine(WaitingABit());
                    else
                        HandleEndGame(winStatus);
                }
            }
        }
    }

  

    private IEnumerator WaitingABit()
    {
        yield return new WaitForSeconds(0.5f);
        BestMoveAI();
    }

    public void BestMoveAI()
    {
        (int x, int y) = Calculs.GetBestMove(Matrix);
        if (x != -1 && y != -1)
        {
            DoMove(x, y, -1);
            int winStatus = Calculs.EvaluateWin(Matrix);
            if (winStatus != 2)
                HandleEndGame(winStatus);
            else
                state = States.CanMove;
        }
    }

    public void DoMove(int x, int y, int team)
    {
        Matrix[x, y] = team;
        GameObject prefab = (team == 1) ? token1 : token2;
        GameObject token = Instantiate(prefab, Calculs.CalculatePoint(x, y), Quaternion.identity);
        spawnedTokens.Add(token);
    }

    private void HandleEndGame(int result)
    {
        state = States.GameOver;
        switch (result)
        {
            case 0: Debug.Log("Empate"); break;
            case 1: Debug.Log("Has ganado!"); break;
            case -1: Debug.Log("Te han ganado!"); break;
        }
    }
}
