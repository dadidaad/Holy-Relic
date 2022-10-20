using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesTimer : MonoBehaviour
{
    // Waves descriptor for this game level
    private WavesInfo wavesInfo;
    // Waves list
    private List<float> waves = new List<float>();
    // Current wave
    private int currentWave;
    // TO before next wave
    private float currentTimeout;
    // Time counter
    private float counter;
    // Timer stopped
    private bool finished;

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Awake()
    {
        wavesInfo = FindObjectOfType<WavesInfo>();
    }
    void Start()
    {
        waves = wavesInfo.wavesTimeouts;
        currentWave = 0;
        counter = 0f;
        finished = false;
        GetCurrentWaveCounter();
    }

    private bool GetCurrentWaveCounter()
    {
        bool res = false;
        if (waves.Count > currentWave)
        {
            counter = currentTimeout = waves[currentWave];
            res = true;
        }
        return res;
    }

    void FixedUpdate()
    {
        if (finished == false)
        {
            // Timeout expired
            if (counter <= 0f)
            {
                // Send event about next wave start
                EventManager.InvokeEvent("WaveStart", null, currentWave.ToString());
                currentWave++;
                // Highlight the timer for short time
                //StartCoroutine("HighlightTimer");
                // All waves are sended
                if (GetCurrentWaveCounter() == false)
                {
                    finished = true;
                    // Send event about timer stop
                    return;
                }
            }
            counter -= Time.fixedDeltaTime;
        }
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
