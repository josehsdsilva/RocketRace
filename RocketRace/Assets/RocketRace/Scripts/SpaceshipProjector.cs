using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceshipProjector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private List<RenderTexture> renderTextures;
    [SerializeField] private List<SpaceshipParticleSystemsController> spaceshipParticleSystemsControllers;

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

    internal void OnAnswer(int spaceshipID, bool correct)
    {
        spaceshipParticleSystemsControllers[spaceshipID].OnAnswer(correct);
    }

    internal void ResetParticleEffects()
    {
        for (int i = 0; i < spaceshipParticleSystemsControllers.Count; i++)
        {
            spaceshipParticleSystemsControllers[i].ResetParticleSystems();
        }
    }

    private void ToggleAllSpaceships(bool active)
    {
        foreach (var spaceshipTypeData in spaceshipTypeDatas)
        {
            spaceshipTypeData.spaceshipObject.SetActive(active);
        }
    }
}
