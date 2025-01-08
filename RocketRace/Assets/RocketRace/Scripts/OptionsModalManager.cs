using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

public class OptionsModalManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameSettingsSO gameSettings;

    [Header("References")]
    [SerializeField] private GameObject modalPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button addTeamButton;
    [SerializeField] private Button removeTeamButton;
    [SerializeField] private TMP_Dropdown timerDropdown;
    [SerializeField] private TMP_Dropdown roundsDropdown;
    [SerializeField] private TMP_Dropdown soundSetDropdown;
    [SerializeField] private Transform teamListContent;
    [SerializeField] private GameObject teamEntryPrefab;
    [SerializeField] private NotificationController notificationController;

    private List<TeamEntry> teamEntries = new List<TeamEntry>();

    private void Start()
    {
        InitializeUI();
        SetupEventListeners();
        AddTeam(); // Add first team by default
    }

    private void InitializeUI()
    {
        // Initialize Sound Set Dropdown
        soundSetDropdown.ClearOptions();
        soundSetDropdown.AddOptions(Enum.GetNames(typeof(QuestionTheme)).ToList());

        // Initialize Timer Dropdown
        timerDropdown.ClearOptions();
        timerDropdown.AddOptions(new List<string> { "15 seconds", "30 seconds", "45 seconds (Default)", "60 seconds" });
        timerDropdown.value = 2; // Default to 120 seconds

        // Initialize Rounds Dropdown
        roundsDropdown.ClearOptions();
        roundsDropdown.AddOptions(new List<string> { "4 rounds", "5 rounds (Default)", "6 rounds" });
        roundsDropdown.value = 1; // Default to 5 rounds

        removeTeamButton.interactable = false;
    }

    private void SetupEventListeners()
    {
        closeButton.onClick.AddListener(CloseModal);
        playButton.onClick.AddListener(StartGame);
        addTeamButton.onClick.AddListener(AddTeam);
        removeTeamButton.onClick.AddListener(RemoveTeam);
    }

    private void AddTeam()
    {
        GameObject teamEntryObj = Instantiate(teamEntryPrefab, teamListContent);
        TeamEntry teamEntry = teamEntryObj.GetComponent<TeamEntry>();
        teamEntries.Add(teamEntry);

        UpdateButtonStates();
    }

    private void RemoveTeam()
    {
        TeamEntry teamEntry = teamEntries[teamEntries.Count - 1];
        teamEntries.Remove(teamEntry);
        Destroy(teamEntry.gameObject);

        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        addTeamButton.interactable = teamEntries.Count < gameSettings.maxTeams;
        removeTeamButton.interactable = teamEntries.Count > 1;
    }

    private void CloseModal()
    {
        modalPanel.SetActive(false);
    }

    private void StartGame()
    {
        if(!CanStartGame()) return;
        
        // Update settings before loading new scene
        gameSettings.soundSet = soundSetDropdown.value;
        gameSettings.timerDuration = GetTimerDuration();
        gameSettings.numberOfRounds = GetNumberOfRounds();
        
        // Update teams
        var teamDataList = new List<GameSettingsSO.TeamData>();
        foreach (TeamEntry entry in teamEntries)
        {
            teamDataList.Add(entry.GetTeamData());
        }
        gameSettings.UpdateTeams(teamDataList);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        CloseModal();
    }

    private bool CanStartGame()
    {
        for (int i = 0; i < teamEntries.Count; i++)
        {
            for (int j = 0; j < teamEntries.Count; j++)
            {
                if(string.IsNullOrEmpty(teamEntries[i].TeamName))
                {
                    notificationController.ShowNotification("Empty name", "Make sure that all teams have an assigned name");
                    return false;
                }

                if (i == j) continue;
                
                if (teamEntries[i].SpaceshipColor == teamEntries[j].SpaceshipColor && teamEntries[i].SpaceshipType == teamEntries[j].SpaceshipType)
                {
                    notificationController.ShowNotification("Duplicate spaceships", "Make sure all teams have different spaceships types or colors!");
                    return false;
                }
            }
        }
        return true;
    }

    private int GetTimerDuration()
    {
        switch (timerDropdown.value)
        {
            case 0: return 15;
            case 1: return 30;
            case 2: return 45;
            case 3: return 60;
            default: return 15;
        }
    }

    private int GetNumberOfRounds()
    {
        switch (roundsDropdown.value)
        {
            case 0: return 4;
            case 1: return 5;
            case 2: return 6;
            default: return 5;
        }
    }

    public void ShowModal()
    {
        modalPanel.SetActive(true);
    }
}