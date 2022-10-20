using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WavesInspector : MonoBehaviour
{
	[HideInInspector]
	// Timeouts between waves
	private WavesInfo wavesInfo;
	public List<float> timeouts
	{
		get
		{
			return wavesInfo.wavesTimeouts;
		}
		set
		{
			wavesInfo.wavesTimeouts = value;
		}
	}
	void OnEnable()
	{
		wavesInfo = GetComponent<WavesInfo>();
		Debug.Assert(wavesInfo, "Settings Fail");
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	public void Update()
	{
		wavesInfo.Update();
	}
}
