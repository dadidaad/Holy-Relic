using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressDescriptionInspector : MonoBehaviour
{
	public Image mapImage;
	public Text heading;
	public Text saveInfo;
	public Text current;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		Debug.Assert(mapImage && heading && saveInfo && current, "Wrong level description stuff settings");
	}
}
