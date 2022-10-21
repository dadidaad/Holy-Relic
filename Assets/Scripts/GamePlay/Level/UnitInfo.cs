using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public string unitName;
    public Sprite primaryIcon;
    public string primaryText;
    public Sprite secondaryIcon;
    public string secondaryText;

    [HideInInspector]
    public decimal level = 1;


    //For Tower
    public List<BulletArrow> bulletArrows;
}
