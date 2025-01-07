using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipProjectorsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<SpaceshipProjector> spaceshipProjectors;

    [Header("Data")]
    [SerializeField] private GameSettingsSO gameSettings;

    private int teamsCount;
    private List<GameSettingsSO.TeamData> teamEntries;

    private void Start()
    {
        teamsCount = gameSettings.currentTeams.Count;
        teamEntries = gameSettings.currentTeams;
        ToggleProjectors();
    }

    private void ToggleProjectors()
    {
        for (int i = 0; i < spaceshipProjectors.Count; i++)
        {
            spaceshipProjectors[i].gameObject.SetActive(i < teamsCount);

            if(i >= teamsCount) continue;

            spaceshipProjectors[i].SetSpaceshipAndColor(teamEntries[i].spaceshipType, teamEntries[i].spaceshipColor, i);
        }
    }
}
