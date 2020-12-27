using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private GameObject holder;

    [SerializeField] private InputField widthInputField;
    [SerializeField] private InputField heightInputField;
    [SerializeField] private InputField playerOneInputField;
    [SerializeField] private InputField playerTwoInputField;

    [SerializeField] private Button startButton;

    private string boardWidth;
    private string boardHeight;
    private string playerOneName;
    private string playerTwoName;

    private void Start() {
        widthInputField.onValueChanged.AddListener(delegate { UpdateField(ref boardWidth, ref widthInputField); });
        heightInputField.onValueChanged.AddListener(delegate { UpdateField(ref boardHeight, ref heightInputField); });
        playerOneInputField.onValueChanged.AddListener(delegate { UpdateField(ref playerOneName, ref playerOneInputField); });
        playerTwoInputField.onValueChanged.AddListener(delegate { UpdateField(ref playerTwoName, ref playerTwoInputField); });

        startButton.onClick.AddListener(delegate { StartGame(); });

        GameManager.instance.OnGameRestartEvent += MainMenuUI_OnGameRestartEvent;
    }

    private void UpdateField(ref string value, ref InputField field) {
        value = field.text;
    }

    public void MainMenuUI_OnGameRestartEvent(object sender, EventArgs e) {
        widthInputField.SetTextWithoutNotify("");
        heightInputField.SetTextWithoutNotify("");
        playerOneInputField.SetTextWithoutNotify("");
        playerTwoInputField.SetTextWithoutNotify("");

        holder.SetActive(true);
    }

    public void StartGame() {
        Debug.Log("====================================================");
        Debug.Log("Starting new game with the following parameters");
        Debug.Log("Width: " + boardWidth);
        Debug.Log("Height: " + boardHeight);
        Debug.Log("Player One Name: " + playerOneName);
        Debug.Log("Player Two Name: " + playerTwoName);
        Debug.Log("====================================================");
        GameManager.instance.SetupGame(int.Parse(boardWidth), int.Parse(boardHeight), playerOneName, playerTwoName);
        holder.SetActive(false);
    }
}
