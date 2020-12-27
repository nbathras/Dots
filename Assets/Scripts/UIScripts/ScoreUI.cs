using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private GameObject holder;

    [SerializeField] private Image arrowImage;

    [SerializeField] private TextMeshProUGUI playerOneNameText;
    [SerializeField] private TextMeshProUGUI playerTwoNameText;

    [SerializeField] private TextMeshProUGUI playerOneScoreText;
    [SerializeField] private TextMeshProUGUI playerTwoScoreText;

    private void Awake() {
        holder.SetActive(false);
    }

    private void Start() {
        GameManager.instance.OnGameSetupEvent += ScoreUI_OnGameSetupEvent;
        GameManager.instance.OnAddScoreEvent += ScoreUI_OnAddScoreEvent;
        GameManager.instance.OnNextTurnEvent += ScoreUI_OnNextTurnEvent;
        GameManager.instance.OnGameRestartEvent += ScoreUI_OnGameRestartEvent;
    }

    private void ScoreUI_OnGameSetupEvent(object sender, GameManager.PlayersEventArgs e) {
        holder.SetActive(true);

        playerOneNameText.SetText(e.playerOne.name);
        playerTwoNameText.SetText(e.playerTwo.name);

        playerOneScoreText.SetText("0");
        playerTwoScoreText.SetText("0");

        SetArrowDirection(true);
    }

    private void ScoreUI_OnAddScoreEvent(object sender, GameManager.PlayerEventArgs e) {
        if (playerOneNameText.text == e.player.name) {
            playerOneScoreText.SetText(e.player.score.ToString());
        } else if (playerTwoNameText.text == e.player.name) {
            playerTwoScoreText.SetText(e.player.score.ToString());
        } else {
            throw new Exception("Error: ScoreUI's player text does not align with player names in GameManager.");
        }
    }

    private void ScoreUI_OnNextTurnEvent(object sender, GameManager.IsPlayerOneTurnEventArgs e) {
        SetArrowDirection(e.isPlayerOneTurn);
    }

    private void ScoreUI_OnGameRestartEvent(object sender, EventArgs e) {
        holder.SetActive(false);
    }

    private void SetArrowDirection(bool isPlayerOneTurn) {
        if (isPlayerOneTurn) {
            arrowImage.transform.eulerAngles = new Vector3(0, 0, 180);
        } else {
            arrowImage.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
