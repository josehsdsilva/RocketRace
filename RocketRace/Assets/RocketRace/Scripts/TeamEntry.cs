using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamEntry : MonoBehaviour
{
    [SerializeField] private TMP_InputField teamNameInput;
    [SerializeField] private TMP_Dropdown rocketDropdown;

    private void Start()
    {
        InitializeDropdowns();
    }

    private void InitializeDropdowns()
    {
        // Setup rocket options
        rocketDropdown.ClearOptions();
        rocketDropdown.AddOptions(new List<string> { "Default Rocket", "Sleek Rocket", "Retro Rocket" });
    }

    public GameSettingsSO.TeamData GetTeamData()
    {
        return new GameSettingsSO.TeamData
        {
            teamName = string.IsNullOrEmpty(teamNameInput.text) ? "Team" : teamNameInput.text,
            rocket = rocketDropdown.options[rocketDropdown.value].text
        };
    }
}