using System.Collections.Generic;
using UnityEngine;

public class Edge : BoardEntity {
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color clickColor;

    private SpriteRenderer sr;

    private List<Cell> cell;

    private Player player;

    private bool isLeftClick;

    private void Awake() {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = defaultColor;
        player = null;
        cell = new List<Cell>();

        isLeftClick = false;
    }

    private void OnMouseEnter() {
        if (player == null) {
            sr.color = highlightColor;
        }
    }

    private void OnMouseExit() {
        if (player == null) {
            sr.color = defaultColor;
        }
    }

    private void OnMouseOver() {
        if (player == null && Input.GetMouseButtonDown(0)) {
            isLeftClick = true;
            sr.color = clickColor;
        } else if (player == null && isLeftClick && Input.GetMouseButtonUp(0)) {
            isLeftClick = false;
            sr.color = highlightColor;
            CaptureEdge();
        }
    }

    private void CaptureEdge() {
        player = GameManager.instance.GetCurrentPlayer();

        bool hasCaptured = false;
        for (int i = 0; i < cell.Count; i++) {
            if (cell[i].CheckCaptureStatus()) {
                cell[i].Capture(player);
                GameManager.instance.AddScore();
                hasCaptured = true;
            }
        }

        if (!hasCaptured) {
            GameManager.instance.NextPlayerTurn();
        }
    }

    public void Attach(BoardEntity[,] board, bool attachUp, int i, int j) {
        if (attachUp) {
            if (j - 1 >= 0) {
                cell.Add((Cell)board[i, j - 1]);
            }
            if (j + 1 < board.GetLength(1)) {
                cell.Add((Cell)board[i, j + 1]);
            }
        } else {
            if (i - 1 >= 0) {
                cell.Add((Cell)board[i - 1, j]);
            }
            if (i + 1 < board.GetLength(0)) {
                cell.Add((Cell)board[i + 1, j]);
            }
        }
    }

    public Player GetPlayer() {
        return player;
    }
}
