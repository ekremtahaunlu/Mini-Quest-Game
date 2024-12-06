using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviourPun {
    [SerializeField] private TextMeshProUGUI timerText;
    //[SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private float gameDuration = 60f;

    private float _remainingTime;
    private bool _isGameActive;

    private void Start() {
        _remainingTime = gameDuration;
        _isGameActive = true;
    }

    private void Update() {
        if (_isGameActive) {
            UpdateTimer();
        }
    }

    private void UpdateTimer() {
        _remainingTime -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.Max(0, Mathf.CeilToInt(_remainingTime)) + "s";

        if (_remainingTime <= 0) {
            EndGame();
        }
    }

    public void EndGame() {
        _isGameActive = false;

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        PlayerController winner = null;
        int highestScore = 0;

        foreach (PlayerController player in players) {
            int playerScore = player.GetScore();
            if (playerScore > highestScore) {
                highestScore = playerScore;
                winner = player;
            }
        }

        // Kazananý duyur
        /*if (winner != null) {
            winnerText.text = $"Kazanan: {winner.photonView.Owner.NickName} Skor: {highestScore}";
        } else {
            winnerText.text = "Kazanan Yok!";
        }*/

        Invoke(nameof(ReturnToLobby), 5f);
    }

    private void ReturnToLobby() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Login");
    }

    public bool IsGameActive() {
        return _isGameActive;
    }
}
