using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamEntry : MonoBehaviour
{
    [SerializeField] private TMP_InputField teamNameInput;
    [SerializeField] private TMP_Dropdown rocketDropdown;

    private void Start()
    {
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        // Setup rocket design options
        rocketDropdown.ClearOptions();
        rocketDropdown.AddOptions(new List<string> { "Blue", "Red", "Green", "Yellow" });
    }

    public OptionsModalManager.TeamData GetTeamData()
    {
        return new OptionsModalManager.TeamData
        {
            teamName = teamNameInput.text,
            rocket = rocketDropdown.options[rocketDropdown.value].text,
        };
    }
}
