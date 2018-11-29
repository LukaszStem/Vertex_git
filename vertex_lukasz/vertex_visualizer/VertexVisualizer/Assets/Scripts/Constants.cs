using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {

    public static List<Color> NeuronColorList { get; set; }
    public static TissueParams TissueData { get; set; }
    public static List<float> GroupBoundaryIDArr { get; set; }
    public static List<TissueSlice> TissueSlices { get; set; }
    public static float timeScale { get; set; }
    public static float currentTime { get; set; }
    public static List<bool> finishedInitialization { get; set; }

    public static float connectionsLoaded = 0;

    public static void ChangeColor(GameObject obj, float alphaValue, float r = 0, float g = 0, float b = 0)
    {
        Color newColor = new Color(r, g, b, alphaValue);

        //Renderer rend = GetComponent<Renderer>()
        obj.GetComponent<Renderer>().material.color = newColor; //C sharp mat.SetColor("_Color", newColor);
    }
}
