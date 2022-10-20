using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAction : MonoBehaviour
{
    public GameObject enabledIcon;
    // Start is called before the first frame update

    void OnEnable()
    {
        EventManager.StartListening("UserUiClick", UserUiClick);
    }

    void OnDisable()
    {
        EventManager.StopListening("UserUiClick", UserUiClick);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UserUiClick(GameObject obj, string param)
    {
        // If clicked on this icon
        if (obj == gameObject)
        {
            if (enabledIcon.activeSelf == true)
            {
                Clicked();
            }
        }
    }

    protected virtual void Clicked()
    {

    }
}
