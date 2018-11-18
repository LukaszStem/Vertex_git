using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.IO;
using UnityEngine.UI;

public class Startup : MonoBehaviour {

    public GameObject tissueLayer;
    public GameObject neuron;
    public List<GameObject> tissueLayerList;
    public SpikeData SpikeTimes { get; set; }

    private bool finishedInitialization = false;
    private float startTime;
    private int currentSpikeIndex;
    public Text timeText;
    public float timeScale = 1;
    dynamic ParseJson(string instance, string filename)
    {
        string myFileName = "C:\\Users\\wassa\\vertex\\" + instance + filename;
        string json = "";
        using (StreamReader r = new StreamReader(myFileName))
        {
            json = r.ReadToEnd();
        }
        int b = json.Length;
        Debug.Log("Finished parsing through " + instance + filename);
        dynamic stuff = JsonConvert.DeserializeObject(json);
        Debug.Log("Finished creating dynamic object");
        Debug.Log(json.Length);
        //Debug.Log(stuff.Items.Count);
        return stuff;
    }

    void CreateNeurons()
    {
        //NOTE: ensure NeuronColorList in constants is set first!!
        Constants.NeuronList = new GameObject[(int)Constants.TissueData.neuronCount];
        for (int i = 0; i < Constants.TissueData.neuronCount; i++)
        {
            GameObject obj = Instantiate(neuron, new Vector3(Constants.TissueData.somaPositionArr[i,0], Constants.TissueData.somaPositionArr[i, 1], Constants.TissueData.somaPositionArr[i, 2]), Quaternion.identity);
            obj.GetComponent<Neuron>().SetID(i+1);
            Constants.NeuronList[i] = obj;
        }
    }

    void SetTissueLayerColors()
    {
        Color colorToUse;
        Color color1 = new Color(10/255, 10/255, 10/255, 0.17f); //Light Grey
        //Color color1 = new Color(221, 116, 116, 0.17f); //Light Red
        Color color2 = new Color(116/255, 128/255, 221/255, 0.17f); //Light Blue
        for (int i = 0; i < tissueLayerList.Count; i++)
        {
            //Alternates
            if (i % 2 == 0) colorToUse = color2; else colorToUse = color1;
            Constants.ChangeColor(tissueLayerList[i], colorToUse.a, colorToUse.r, colorToUse.g, colorToUse.b);
        }
    }

    void SetNeuronGroupColors()
    {
        Color[] colorArr = new Color[] { Color.magenta, Color.cyan, Color.green, Color.blue, Color.yellow, Color.red };

        Constants.NeuronColorList = new List<Color>();
        
        for(int i = 1; i < Constants.TissueData.groupBoundaryIDArr.Count; i++)
        {
            Constants.NeuronColorList.Add(colorArr[(i-1) % colorArr.Length]);
        }
    }

    void CreateTissueLayers()
    {
        float zPosition = 1240;
        float zDepth = 0;
        float xPos = Constants.TissueData.X / 2;
        float yPos = Constants.TissueData.Y / 2;
        Vector3 placement = new Vector3(0, 0, 0);
        
        for (int i = 0; i < Constants.TissueData.numLayers; i++)
        {
            zDepth = Constants.TissueData.layerBoundaryArr[i] - Constants.TissueData.layerBoundaryArr[i + 1];
            
            GameObject currentLayer = Instantiate(tissueLayer, new Vector3(xPos, yPos, zPosition - (zDepth/2)), Quaternion.identity);
            Vector3 scale = new Vector3(Constants.TissueData.X, Constants.TissueData.Y, zDepth);
            currentLayer.transform.localScale = scale;
            
            tissueLayerList.Add(currentLayer);

            zPosition -= zDepth;
        }
    }

    void InitializeObjectsFromJson(string instance)
    {
        dynamic VertexLFP = ParseJson(instance, "LFP.json");
        dynamic Vertexparams = ParseJson(instance, "params.json");
        dynamic Vertexspikes = ParseJson(instance, "spikes.json");
        dynamic myTissueParams = Vertexparams.TissueParams;

        //GameObject tissueSlice = GameObject.Find("TissueSlice");
        Constants.TissueData = new TissueParams(myTissueParams);

        this.SpikeTimes = new SpikeData(Vertexspikes);
        this.startTime = Time.time;
        this.currentSpikeIndex = 0;
        Constants.timeScale = timeScale;
        this.finishedInitialization = true;
    }
	// Use this for initialization
	void Start () {
        tissueLayerList = new List<GameObject>();
        InitializeObjectsFromJson("2");
        CreateTissueLayers();
        SetTissueLayerColors();
        SetNeuronGroupColors();
        CreateNeurons();
    }

    void FixedUpdate()
    {
        if (finishedInitialization)
        {
            //List<int> IDs = new List<int>();
            float endTime = this.startTime + (Time.fixedDeltaTime * timeScale);
            //float totalTime = endTime - this.startTime;

            //float currentNeuronTime = this.SpikeTimes.times[this.currentSpikeIndex, 1];
            //float currentNeuronTimeEnd = currentNeuronTime + Time.fixedDeltaTime;

            while (this.currentSpikeIndex < this.SpikeTimes.times.Length)
            {
                if(this.SpikeTimes.times[this.currentSpikeIndex, 1] < endTime)
                {
                    int neuronToSpike = (int)this.SpikeTimes.times[this.currentSpikeIndex, 0];
                    Constants.NeuronList[neuronToSpike - 1].GetComponent<Neuron>().spike();
                    this.currentSpikeIndex++;
                }
                else
                {
                    break;
                }
            }
            this.startTime = endTime;
            timeText.text = "Time:" + this.startTime.ToString("0.00");
        }
    }
}
