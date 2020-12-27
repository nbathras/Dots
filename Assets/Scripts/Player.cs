using UnityEngine;

public class Player {
    public Player(string name, Color color) {
        this.name = name;
        this.color = color;
        this.score = 0;
    }

    public string name;
    public Color color;
    public int score;
}