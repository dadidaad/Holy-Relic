using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayFolderInspector : MonoBehaviour
{
    public GameObject pathwayPrefab;
    public Transform pathwayFolder;
    public GameObject capturePointPrefab;
    public Transform capturePointFolder;
    void OnEnable()
    {
        Debug.Assert(pathwayPrefab && pathwayFolder && capturePointPrefab && capturePointFolder, "Settings Fail");
    }

    public GameObject AddPathway()
    {
        Pathway[] array = GetComponentsInChildren<Pathway>();
        GameObject res = Instantiate(pathwayPrefab, pathwayFolder).gameObject;
        res.transform.SetAsLastSibling();
        res.name = pathwayPrefab.name + " (" + (array.Length + 1) + ")";
        return res;
    }

    public GameObject GetNextPathway(GameObject currentSelected)
    {
        return InspectorsUtil<Pathway>.GetNext(pathwayFolder, currentSelected);
    }

    /// <summary>
    /// Gets the previoust pathway.
    /// </summary>
    /// <returns>The previoust pathway.</returns>
    /// <param name="currentSelected">Current selected.</param>
    public GameObject GetPrevioustPathway(GameObject currentSelected)
    {
        return InspectorsUtil<Pathway>.GetPrevious(pathwayFolder, currentSelected);
    }

    public GameObject AddCapturePoint()
    {
        CapturePoint[] array = GetComponentsInChildren<CapturePoint>();
        GameObject res = Instantiate(capturePointPrefab, capturePointFolder).gameObject;
        res.transform.SetSiblingIndex(array.Length);
        res.name = capturePointPrefab.name + " (" + (array.Length + 1) + ")";
        return res;
    }

    /// <summary>
    /// Gets the next capture point.
    /// </summary>
    /// <returns>The next capture point.</returns>
    /// <param name="currentSelected">Current selected.</param>
    public GameObject GetNextCapturePoint(GameObject currentSelected)
    {
        return InspectorsUtil<CapturePoint>.GetNext(capturePointFolder, currentSelected);
    }

    /// <summary>
    /// Gets the previoust capture point.
    /// </summary>
    /// <returns>The previoust capture point.</returns>
    /// <param name="currentSelected">Current selected.</param>
    public GameObject GetPrevioustCapturePoint(GameObject currentSelected)
    {
        return InspectorsUtil<CapturePoint>.GetPrevious(capturePointFolder, currentSelected);

    }
}
