using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXScript : MonoBehaviour
{
    ParticleSystem[] particles;
    AudioSource[] sources;
    static bool finished = false;
    static int i = 0;

    void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        sources = GetComponentsInChildren<AudioSource>();
    }


    void Update()
    {
        finished = true;
        for (i=0; i<particles.Length;i++)
            if (particles[i].isPlaying) finished = false;

        for (i=0; i<sources.Length;i++)
            if (sources[i].isPlaying) finished = false;
        
        if (finished) Destroy(gameObject);
    }
} // FIN DU SCRIPT
