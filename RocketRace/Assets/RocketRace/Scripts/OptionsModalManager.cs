using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        soundSetDropdown.AddOptions(new List<string> { "instruments", "chords" });

        // Initialize Timer Dropdown
        timerDropdown.ClearOptions();
        timerDropdown.AddOptions(new List<string> { "60 seconds", "90 seconds", "120 seconds (Default)", "180 seconds" });
        timerDropdown.value = 2; // Default to 120 seconds

        // Initialize Rounds Dropdown
        roundsDropdown.ClearOptions();
        roundsDropdown.AddOptions(new List<string> { "3 rounds", "5 rounds (Default)", "7 rounds", "10 rounds" });
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
        if (teamEntries.Count >= gameSettings.maxTeams)
        {
            Debug.Log("Maximum number of teams reached!");
            return;
        }

        GameObject teamEntryObj = Instantiate(teamEntryPrefab, teamListContent);
        TeamEntry teamEntry = teamEntryObj.GetComponent<TeamEntry>();
        teamEntries.Add(teamEntry);

        UpdateButtonStates();
    }

    private void RemoveTeam()
    {
        if (teamEntries.Count <= 1)
        {
            Debug.Log("Cannot remove last team!");
            return;
        }

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
        // Update settings before loading new scene
        gameSettings.soundSet = soundSetDropdown.options[soundSetDropdown.value].text;
        gameSettings.timerDuration = GetTimerDuration();
        gameSettings.numberOfRounds = GetNumberOfRounds();
        
        // Update teams
        var teamDataList = new List<GameSettingsSO.TeamData>();
        foreach (var entry in teamEntries)
        {
            teamDataList.Add(entry.GetTeamData());
        }
        gameSettings.UpdateTeams(teamDataList);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        CloseModal();
    }

    private int GetTimerDuration()
    {
        switch (timerDropdown.value)
        {
            case 0: return 60;
            case 1: return 90;
            case 2: return 120;
            case 3: return 180;
            default: return 120;
        }
    }

    private int GetNumberOfRounds()
    {
        switch (roundsDropdown.value)
        {
            case 0: return 3;
            case 1: return 5;
            case 2: return 7;
            case 3: return 10;
            default: return 5;
        }
    }

    public void ShowModal()
    {
        modalPanel.SetActive(true);
    }
}