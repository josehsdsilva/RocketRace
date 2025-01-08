using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnersDisplayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winnersUI;
    [SerializeField] private TeamScore winnerTeamPrefab;
    [SerializeField] private Transform winnersHolder;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        winnersUI.SetActive(false);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    internal void ShowWinners(List<TeamScore> winners)
    {
        winnersUI.SetActive(true);
        for (int i = 0; i < winners.Count; i++)
        {
            TeamScore winnerTeam = Instantiate(winnerTeamPrefab, winnersHolder);
            winnerTeam.SetTeamScore(winners[i].TeamName, winners[i].Score, winners[i].TeamID);
            winnerTeam.ToggleHighlight(true);
        }
        audioSource.Play();
    }
}
