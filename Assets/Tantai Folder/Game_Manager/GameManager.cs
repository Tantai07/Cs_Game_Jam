using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // ✅ ใช้ระบบ Input ใหม่

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform gameBoard;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private int boardSize = 4;

    [Header("Tile Settings")]
    [SerializeField] private float gap = 0.01f;

    private List<Transform> pieces = new List<Transform>();
    private int emptyIndex;
    private bool isShuffling = false;

    private void Start()
    {
        GenerateBoard();
    }

    private void Update()
    {
        if (isShuffling) return;

        if (Mouse.current.leftButton.wasPressedThisFrame) // ✅ ใช้ระบบใหม่
        {
            HandleClick();
        }

        if (CheckWin())
        {
            isShuffling = true;
            StartCoroutine(ShuffleAfterDelay(0.5f));
        }
    }

    #region Board Setup

    private void GenerateBoard()
    {
        float tileWidth = 1f / boardSize;

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                int index = row * boardSize + col;
                Transform tile = Instantiate(piecePrefab, gameBoard);
                tile.name = index.ToString();
                tile.localPosition = new Vector3(
                    -1 + (2 * tileWidth * col) + tileWidth,
                    +1 - (2 * tileWidth * row) - tileWidth,
                    0
                );
                tile.localScale = ((2 * tileWidth) - gap) * Vector3.one;

                pieces.Add(tile);

                if (index == (boardSize * boardSize - 1))
                {
                    emptyIndex = index;
                    tile.gameObject.SetActive(false);
                }
                else
                {
                    SetUV(tile, row, col, tileWidth);
                }
            }
        }
    }

    private void SetUV(Transform tile, int row, int col, float tileWidth)
    {
        float g = gap / 2;
        Mesh mesh = tile.GetComponent<MeshFilter>().mesh;
        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2((tileWidth * col) + g, 1 - ((tileWidth * (row + 1)) - g));
        uv[1] = new Vector2((tileWidth * (col + 1)) - g, 1 - ((tileWidth * (row + 1)) - g));
        uv[2] = new Vector2((tileWidth * col) + g, 1 - ((tileWidth * row) + g));
        uv[3] = new Vector2((tileWidth * (col + 1)) - g, 1 - ((tileWidth * row) + g));

        mesh.uv = uv;
    }

    #endregion

    #region Gameplay

    private void HandleClick()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ✅ ใช้ระบบใหม่
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (!hit) return;

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] == hit.transform)
            {
                if (TrySwap(i, -boardSize, boardSize)) return; // Up
                if (TrySwap(i, +boardSize, boardSize)) return; // Down
                if (TrySwap(i, -1, 0)) return; // Left
                if (TrySwap(i, +1, boardSize - 1)) return; // Right
            }
        }
    }

    private bool TrySwap(int current, int offset, int invalidCol)
    {
        if ((current % boardSize) == invalidCol) return false;
        int target = current + offset;
        if (target != emptyIndex) return false;

        (pieces[current], pieces[target]) = (pieces[target], pieces[current]);
        (pieces[current].localPosition, pieces[target].localPosition) = (pieces[target].localPosition, pieces[current].localPosition);

        emptyIndex = current;
        return true;
    }

    private bool CheckWin()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != i.ToString()) return false;
        }
        return true;
    }

    #endregion

    #region Shuffle

    private IEnumerator ShuffleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShuffleBoard();
        isShuffling = false;
    }

    private void ShuffleBoard()
    {
        int count = 0;
        int lastMove = -1;

        while (count < boardSize * boardSize * 3)
        {
            int rnd = Random.Range(0, pieces.Count);
            if (rnd == lastMove) continue;

            lastMove = emptyIndex;

            if (TrySwap(rnd, -boardSize, boardSize) ||
                TrySwap(rnd, +boardSize, boardSize) ||
                TrySwap(rnd, -1, 0) ||
                TrySwap(rnd, +1, boardSize - 1))
            {
                count++;
            }
        }
    }

    #endregion
}
