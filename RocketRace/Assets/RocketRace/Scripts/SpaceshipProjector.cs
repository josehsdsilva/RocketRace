using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipProjector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private List<RenderTexture> renderTextures;

    [Header("Settings")]
    [SerializeField] private List<SpaceshipColorData> spaceshipColorDatas;
    [SerializeField] private List<SpaceshipTypeData> spaceshipTypeDatas;

    private SpaceshipTypeData spaceshipTypeData;

    internal void SetSpaceshipAndColor(SpaceshipType spaceshipType, SpaceshipColor spaceshipColor, int playerID)
    {
        ToggleAllSpaceships(false);
        spaceshipTypeData = spaceshipTypeDatas.Find(x => x.spaceshipType == spaceshipType);
        spaceshipTypeData.spaceshipObject.SetActive(true);
        spaceshipTypeData.spaceshipRenderer.material = spaceshipColorDatas.Find(x => x.spaceshipColor == spaceshipColor).material;

        cam.targetTexture = renderTextures[playerID];
    }

    private void ToggleAllSpaceships(bool active)
    {
        foreach (var spaceshipTypeData in spaceshipTypeDatas)
        {
            spaceshipTypeData.spaceshipObject.SetActive(active);
        }
    }
}
