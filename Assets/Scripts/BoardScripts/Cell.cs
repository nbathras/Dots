using UnityEngine;

public class Cell : BoardEntity {
    private SpriteRenderer sr;

    private Edge[] edges;
    private Dot[] dots;

    private Player player;

    private void Awake() {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Attach(BoardEntity[,] board, int i, int j) {
        edges = new Edge[4];
        dots = new Dot[4];

        dots[0] = (Dot) board[i - 1, j - 1];
        dots[1] = (Dot) board[i - 1, j + 1];
        dots[2] = (Dot) board[i + 1, j + 1];
        dots[3] = (Dot) board[i + 1, j - 1];

        edges[0] = (Edge) board[i - 1, j];
        edges[1] = (Edge) board[i, j + 1];
        edges[2] = (Edge) board[i + 1, j];
        edges[3] = (Edge) board[i, j - 1];
    }

    public bool CheckCaptureStatus() {
        for (int i = 0; i < edges.Length; i++) {
            if (edges[i].GetPlayer() == null) { return false; }
        }

        return true;
    }

    public void Capture(Player inPlayer) {
        player = inPlayer;

        sr.color = player.color;
    }
}
