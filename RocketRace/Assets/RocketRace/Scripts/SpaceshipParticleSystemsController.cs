using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class SpaceshipParticleSystemsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<ParticleSystem> particleSystems;

    internal void ResetParticleSystems()
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            var emission = particleSystems[i].emission;
            emission.rateOverTime = 3;
        }
    }

    internal void OnAnswer(bool correct)
    {
        Debug.Log("OnAnswer");
        for (int i = 0; i < particleSystems.Count; i++)
        {
            Debug.Log(particleSystems[i].gameObject, particleSystems[i].gameObject);
            var emission = particleSystems[i].emission;
            emission.rateOverTime = correct ? 10 : 1;
        }
    }
}