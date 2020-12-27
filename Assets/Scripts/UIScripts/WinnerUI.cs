using System;
using UnityEngine;

public class WinnerUI : MonoBehaviour {

    [SerializeField] private GameObject holder;

    [SerializeField] private TMPro.TextMeshProUGUI winnerText;

    private void Awake() {
        holder.SetActive(false);
    }

    private void Start() {
        GameManager.instance.OnGameOverEvent += WinnerUI_OnGameOverEvent;
        GameManager.instance.OnGameRestartEvent += WinnerUI_OnGameRestartEvent;
    }

    private void WinnerUI_OnGameRestartEvent(object sender, EventArgs e) {
        holder.SetActive(false);
    }

    public void Display(string winnerTextString) {
        holder.SetActive(true);
        winnerText.SetText(winnerTextString);
    }

    private void WinnerUI_OnGameOverEvent(object sender, GameManager.PlayersEventArgs e) {
        holder.SetActive(true);
        winnerText.SetText(GetWinnerText(e.playerOne, e.playerTwo));
    }

    public static string GetWinnerText(Player playerOne, Player playerTwo) {
        string winnerTextString;
        if (playerOne.score > playerTwo.score) {
            winnerTextString = playerOne.name + " Won!";
        } else if (playerOne.score < playerTwo.score) {
            winnerTextString = playerTwo.name + " Won!";
        } else {
            winnerTextString = "Tie";
        }

        return winnerTextString;
    }
}
