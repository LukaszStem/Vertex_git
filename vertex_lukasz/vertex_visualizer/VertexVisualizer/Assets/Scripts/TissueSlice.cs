using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;

public class TissueSlice : MonoBehaviour
{
    public GameObject tissueLayer;
    public GameObject neuron;
    public GameObject electrode;

    public TissueParams TissueData;
    public GameObject[] NeuronList;
    public GameObject[] ElectrodeList;
    public RecordingSettings RecordingData;
    public LocalFieldPotential LFPData;
    public List<GameObject> tissueLayerList;
    public SpikeData SpikeTimes { get; set; }

    private List<Color> NeuronColorList { get; set; }


    private float startTime;
    private int currentSpikeIndex;

    void CreateNeurons()
    {
        //NOTE: ensure NeuronColorList in constants is set first!!
        this.NeuronList = new GameObject[(int)this.TissueData.neuronCount];
        for (int i = 0; i < this.TissueData.neuronCount; i++)
        {
            GameObject obj = Instantiate(neuron, new Vector3(this.TissueData.somaPositionArr[i, 0], this.TissueData.somaPositionArr[i, 1], this.TissueData.somaPositionArr[i, 2]), Quaternion.identity);
            obj.GetComponent<Neuron>().SetID(i + 1);
            this.NeuronList[i] = obj;
        }
    }

    void CreateElectrodes()
    {
        //First compute largest and smallest LFP values

        float smallestVal = 100;
        float largestVal = -100;
        for (int i = 0; i < this.LFPData.LFPValues.GetLength(0); i++)
        {
            for (int j = 0; j < this.LFPData.LFPValues.GetLength(1); j++)
            {
                if (this.LFPData.LFPValues[i, j] < smallestVal)
                {
                    smallestVal = this.LFPData.LFPValues[i, j];
                }
                if (this.LFPData.LFPValues[i, j] > largestVal)
                {
                    largestVal = this.LFPData.LFPValues[i, j];
                }
            }
        }

        this.ElectrodeList = new GameObject[(int)this.RecordingData.numElectrodes];
        for (int i = 0; i < this.RecordingData.numElectrodes; i++)
        {
            GameObject obj = Instantiate(electrode, new Vector3(this.RecordingData.meaPosList[i].x, this.RecordingData.meaPosList[i].y, this.RecordingData.meaPosList[i].z), Quaternion.identity);
            obj.GetComponent<Electrode>().SetElectrode(i, largestVal, smallestVal);
            this.ElectrodeList[i] = obj;
        }
    }

    void SetTissueLayerColors()
    {
        Color colorToUse;
        Color color1 = new Color(10 / 255, 10 / 255, 10 / 255, 0.17f); //Light Grey
        //Color color1 = new Color(221, 116, 116, 0.17f); //Light Red
        Color color2 = new Color(116 / 255, 128 / 255, 221 / 255, 0.17f); //Light Blue
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

        this.NeuronColorList = new List<Color>();

        for (int i = 1; i < this.TissueData.groupBoundaryIDArr.Count; i++)
        {
            this.NeuronColorList.Add(colorArr[(i - 1) % colorArr.Length]);
        }
    }

    void CreateTissueLayers()
    {
        float zPosition = 1240;
        float zDepth = 0;
        float xPos = this.TissueData.X / 2;
        float yPos = this.TissueData.Y / 2;
        Vector3 placement = new Vector3(0, 0, 0);

        for (int i = 0; i < this.TissueData.numLayers; i++)
        {
            zDepth = this.TissueData.layerBoundaryArr[i] - this.TissueData.layerBoundaryArr[i + 1];

            GameObject currentLayer = Instantiate(tissueLayer, new Vector3(xPos, yPos, zPosition - (zDepth / 2)), Quaternion.identity);
            Vector3 scale = new Vector3(this.TissueData.X, this.TissueData.Y, zDepth);
            currentLayer.transform.localScale = scale;

            tissueLayerList.Add(currentLayer);

            zPosition -= zDepth;
        }
    }

    dynamic ParseJson(string instance, string directory, string filename)
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
        return stuff;
    }

    void InitializeObjectsFromJson(string directory, string instance)
    {
        dynamic VertexLFP = ParseJson(instance, directory, "LFP.json");
        dynamic Vertexparams = ParseJson(instance, directory, "params.json");
        dynamic Vertexspikes = ParseJson(instance, directory, "spikes.json");
        dynamic myTissueParams = Vertexparams.TissueParams;
        dynamic myRecordingSettings = Vertexparams.RecordingSettings;

        //GameObject tissueSlice = GameObject.Find("TissueSlice");
        this.TissueData = new TissueParams(myTissueParams);
        this.RecordingData = new RecordingSettings(myRecordingSettings);
        this.LFPData = new LocalFieldPotential(VertexLFP);

        this.SpikeTimes = new SpikeData(Vertexspikes);
        this.startTime = Time.time;
        this.currentSpikeIndex = 0;
    }

    public TissueSlice(string directory, string instance)
    {
        InitializeObjectsFromJson(directory, instance);
        CreateTissueLayers();
        SetTissueLayerColors();
        SetNeuronGroupColors();
        CreateNeurons();
        CreateElectrodes();
    }

}