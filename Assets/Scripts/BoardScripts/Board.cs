using System;
using UnityEngine;

public class Board : BoardEntity {
    [SerializeField] private GameObject pfCell;
    [SerializeField] private GameObject pfEdge;
    [SerializeField] private GameObject pfDot;

    [SerializeField] private GameObject worldBackgroundSprite;
    [SerializeField] private GameObject boardBackgroundSprite;

    private BoardEntity[,] board;

    private void ClearPreviousBoard() {
        if (board == null) {
            return; //do not have to clear anything
        }

        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {
                Destroy(board[i, j].gameObject);
            }
        }
    }

    public void GenerateNewBoard(int width, int height) {
        if (width <= 0 || height <= 0) {
            throw new Exception("Width and Height fields of Board must be greater than zero.");
        }

        ClearPreviousBoard();

        boardBackgroundSprite.transform.localScale = new Vector3(width * 2 + 0.5f, height * 2 + 0.5f, 0);
        boardBackgroundSprite.transform.position = new Vector3(width, height, 0f);
        worldBackgroundSprite.transform.localScale = new Vector3(32 * (height / 2f), 18 * (height / 2f), 0f);
        worldBackgroundSprite.transform.position = new Vector3(width, height, 0f);

        board = new BoardEntity[width * 2 + 1, height * 2 + 1];

        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {
                if ((i % 2 == 0) && (j % 2 == 0)) {
                    // Create Dot
                    board[i, j] = CreateDot(i, j);
                } else if ((i % 2 == 1) && (j % 2 == 0)) {
                    // Create Edge horizontal
                    board[i, j] = CreateEdge(i - 1, j, -90f);
                } else if ((i % 2 == 0) && (j % 2 == 1)) {
                    // Create Edge Vertical
                    board[i, j] = CreateEdge(i, j - 1, 0f);
                } else if ((i % 2 == 1) && (j % 2 == 1)) {
                    board[i, j] = CreateCell(i, j);
                } else {
                    throw new Exception("You made a mistake");
                }
            }
        }

        // Attach Cells
        for (int i = 1; i < board.GetLength(0); i += 2) {
            for (int j = 1; j < board.GetLength(1); j += 2) {
                Cell cell = (Cell) board[i, j];
                cell.Attach(board, i, j);
            }
        }

        // Attach Edges Horizontal
        for (int i = 1; i < board.GetLength(0); i += 2) {
            for (int j = 0; j < board.GetLength(1); j += 2) {
                Edge edge = (Edge)board[i, j];
                edge.Attach(board, true, i, j);
            }
        }

        // Attach Edges Veritcal
        for (int i = 0; i < board.GetLength(0); i += 2) {
            for (int j = 1; j < board.GetLength(1); j += 2) {
                Edge edge = (Edge) board[i, j];
                edge.Attach(board, false, i, j);
            }
        }
    }

    private BoardEntity CreateDot(int x, int y) {
        Dot newDot = Instantiate(pfDot, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Dot>();

        if (newDot == null) {
            throw new Exception("The GameObject assinged to the field pfDot in Board must have a Dot component");
        }

        newDot.transform.SetParent(transform);

        return newDot;
    }

    private BoardEntity CreateEdge(int x, int y, float rotation) {
        Edge newEdge = Instantiate(pfEdge, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Edge>();
        newEdge.transform.Rotate(new Vector3(0, 0, rotation));

        if (newEdge == null) {
            throw new Exception("The GameObject assinged to the field pfEdge in Board must have an Edge component");
        }

        newEdge.transform.SetParent(transform);

        return newEdge;
    } 

    private BoardEntity CreateCell(int x, int y) {
        Cell newCell = Instantiate(pfCell, new Vector3(x, y), Quaternion.identity).GetComponent<Cell>();

        if (newCell == null) {
            throw new Exception("the GameObject assigned to the field pfCell in Board must have a Cell component");
        }

        newCell.transform.SetParent(transform);

        return newCell;
    }

    public bool IsGameOver() {
        for (int i = 1; i < board.GetLength(0); i += 2) {
            for (int j = 1; j < board.GetLength(1); j += 2) {
                Cell cell = (Cell)board[i, j];
                if (!cell.CheckCaptureStatus()) { return false; }
            }
        }

        return true;
    }
}
