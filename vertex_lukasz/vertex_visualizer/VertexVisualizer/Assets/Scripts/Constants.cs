using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {

    public static List<Color> NeuronColorList { get; set; }
    public static TissueParams TissueData { get; set; }
    public static GameObject[] NeuronList { get; set; }
    public static GameObject[] ElectrodeList { get; set; }

    public static RecordingSettings RecordingData { get; set; }
    public static LocalFieldPotential LFPData { get; set; }
    public static float timeScale { get; set; }
    public static bool finishedInitialization { get; set; }

    public static void ChangeColor(GameObject obj, float alphaValue, float r = 0, float g = 0, float b = 0)
    {
        Color newColor = new Color(r, g, b, alphaValue);

        //Renderer rend = GetComponent<Renderer>()
        obj.GetComponent<Renderer>().material.color = newColor; //C sharp mat.SetColor("_Color", newColor);
    }
}
