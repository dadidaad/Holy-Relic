using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesInfo : MonoBehaviour
{
    public List<float> wavesTimeouts = new List<float>();

    private float defaultWaveTimeout = 30f;

    private SpawnPoint[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        spawners = FindObjectsOfType<SpawnPoint>();

        int wavesCount = 0;
        // Get the max number of waves from spawners
        foreach (SpawnPoint spawner in spawners)
        {
            if (spawner.waves.Count > wavesCount)
            {
                wavesCount = spawner.waves.Count;
            }
        }

        if (wavesTimeouts.Count < wavesCount)
        {
            int i;
            for (i = wavesTimeouts.Count; i < wavesCount; ++i)
            {
                wavesTimeouts.Add(defaultWaveTimeout);
            }
        }
        else if (wavesTimeouts.Count > wavesCount)
        {
            wavesTimeouts.RemoveRange(wavesCount, wavesTimeouts.Count - wavesCount);
        }
    }
}
