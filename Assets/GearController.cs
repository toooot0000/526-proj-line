using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class GearController : MonoBehaviour
{
    public Image image;
    public TextMeshPro textMeshPro;
    
    public void UpdatePanelInfo(Gear gear)
    {
        // this.image = gear.
        this.textMeshPro.text = gear.desc;
    }

}
