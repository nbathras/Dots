using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField] private float gameOverRestartTimeLength = 2f;

    [SerializeField] private Board board;

    private Player playerOne;
    private Player playerTwo;
    private bool isPlayerOneTurn;

    private Camera mainCamera;

    /* Event system */
    public class PlayerEventArgs : EventArgs {
        public PlayerEventArgs() { player = instance.GetCurrentPlayer(); }

        public Player player;
    }
    public class PlayersEventArgs : EventArgs {
        public PlayersEventArgs() { playerOne = instance.playerOne; playerTwo = instance.playerTwo; }

        public Player playerOne;
        public Player playerTwo;
    }
    public class IsPlayerOneTurnEventArgs : EventArgs {
        public IsPlayerOneTurnEventArgs() { isPlayerOneTurn = (instance.GetCurrentPlayer() == instance.playerOne); }

        public bool isPlayerOneTurn;
    }

    public event EventHandler<PlayersEventArgs> OnGameSetupEvent;

    public event EventHandler<IsPlayerOneTurnEventArgs> OnNextTurnEvent;

    public event EventHandler<PlayerEventArgs> OnAddScoreEvent;

    public event EventHandler<PlayersEventArgs> OnGameOverEvent;

    public event EventHandler<EventArgs> OnGameRestartEvent;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        mainCamera = Camera.main;
    }

    public Player GetCurrentPlayer() {
        if (isPlayerOneTurn) {
            return playerOne;
        } else {
            return playerTwo;
        }
    }

    public bool IsPlayerOneTurn() {
        return playerOne == GetCurrentPlayer();
    }

    public void AddScore() {
        GetCurrentPlayer().score += 1;

        OnAddScoreEvent?.Invoke(this, 
            new PlayerEventArgs {
                player = GetCurrentPlayer()
            }
        );

        PrintScoreToConsole();

        if (board.IsGameOver()) {
            StartCoroutine(GameOverCoroutine());
        }
    }

    private IEnumerator GameOverCoroutine() {
        PrintGameOverToConsole(WinnerUI.GetWinnerText(playerOne, playerTwo));

        OnGameOverEvent?.Invoke(this, new PlayersEventArgs());

        yield return new WaitForSeconds(gameOverRestartTimeLength);

        OnGameRestartEvent?.Invoke(this, EventArgs.Empty);
    } 

    public void NextPlayerTurn() {
        isPlayerOneTurn = !isPlayerOneTurn;

        OnNextTurnEvent?.Invoke(this, new IsPlayerOneTurnEventArgs());
    }

    public void SetupGame(int width, int height, string playerOneName, string playerTwoName) {
        isPlayerOneTurn = true;

        playerOne = new Player(playerOneName, Color.red);
        playerTwo = new Player(playerTwoName, Color.blue);

        mainCamera.transform.position = new Vector3(width, height, -10f);
        mainCamera.orthographicSize = height * 1.2f + 1.9f;

        board.GenerateNewBoard(width, height);

        OnGameSetupEvent?.Invoke(this, new PlayersEventArgs());
    }

    private void PrintScoreToConsole() {
        Debug.Log("====================================================");
        Debug.Log("Score Update:");
        Debug.Log(playerOne.name + ": " + playerOne.score);
        Debug.Log(playerTwo.name + ": " + playerTwo.score);
        Debug.Log("====================================================");
    }

    private void PrintGameOverToConsole(string winnerTextString) {
        Debug.Log("====================================================");
        Debug.Log("GameOver");
        Debug.Log(winnerTextString);
        Debug.Log("====================================================");
    }
}
