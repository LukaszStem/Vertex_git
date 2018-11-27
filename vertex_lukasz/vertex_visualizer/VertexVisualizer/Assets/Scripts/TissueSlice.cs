using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System;

public class TissueSlice : MonoBehaviour
{
    public GameObject tissueLayer;
    public GameObject neuron;
    public GameObject electrode;
    public GameObject connection;

    public TissueParams TissueData;
    public ConnectionInfo ConnectionData;
    public GameObject[] NeuronList;
    public GameObject[] ElectrodeList;
    public RecordingSettings RecordingData;
    public LocalFieldPotential LFPData;
    public List<GameObject> tissueLayerList;
    public SpikeData SpikeTimes { get; set; }
    public List<ConnectionGroup> connectionGroups;

    private List<Color> NeuronColorList { get; set; }

    private float startTime;
    private int currentSpikeIndex;
    private int tissueInstance;
    private float yOffset;
    private float xOffset;
    private int electrodeIndex;
    private Dictionary<int, int> weightMappings;

    void CreateNeurons()
    {
        //NOTE: ensure NeuronColorList in constants is set first!!
        this.NeuronList = new GameObject[(int)this.TissueData.neuronCount];
        for (int i = 0; i < this.TissueData.neuronCount; i++)
        {
            GameObject obj = Instantiate(neuron, new Vector3(this.TissueData.somaPositionArr[i, 0] + xOffset, this.TissueData.somaPositionArr[i, 1] + yOffset, this.TissueData.somaPositionArr[i, 2]), Quaternion.identity);
            obj.GetComponent<Neuron>().SetID(i + 1);
            this.NeuronList[i] = obj;
        }
    }

    void CreateConnections()
    {
        this.connectionGroups = new List<ConnectionGroup>();
        for (int i = 0; i < weightMappings.Count; i++)
        {
            int preNeuron = weightMappings[i + 1];
            int[] postNeurons = this.ConnectionData.connection_mappings[i];
            List<GameObject> currentConnections = new List<GameObject>();
            for(int j = 0; j < postNeurons.Length; j++)
            {
                Vector3 preNeuronPos = new Vector3(this.TissueData.somaPositionArr[preNeuron-1, 0] + xOffset, this.TissueData.somaPositionArr[preNeuron-1, 1] + yOffset, this.TissueData.somaPositionArr[preNeuron-1, 2]);
                Vector3 postNeuronPos = new Vector3(this.TissueData.somaPositionArr[postNeurons[j]-1, 0] + xOffset, this.TissueData.somaPositionArr[postNeurons[j]-1, 1] + yOffset, this.TissueData.somaPositionArr[postNeurons[j]-1, 2]);
                if (preNeuronPos.Equals(postNeuronPos))
                    continue;
                GameObject obj = Instantiate(connection, new Vector3(0f, 0f, 0f), Quaternion.identity);
                obj.GetComponent<Connection>().CreateConneciton(1f, preNeuronPos, postNeuronPos);
                currentConnections.Add(obj);
                //Creating for just 1 connection now
                //j = postNeurons.Length;
            }
            ConnectionGroup group = new ConnectionGroup();
            group.CreateConnecitons(preNeuron, currentConnections);
            this.connectionGroups.Add(group);
            //Creating for just 1 neuron now
            break;
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
            GameObject obj = Instantiate(electrode, new Vector3(this.RecordingData.meaPosList[i].x + xOffset, this.RecordingData.meaPosList[i].y + yOffset, this.RecordingData.meaPosList[i].z), Quaternion.identity);
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

        if (Constants.NeuronColorList.Count == 0)
        {
            Constants.NeuronColorList = this.NeuronColorList;
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

            GameObject currentLayer = Instantiate(tissueLayer, new Vector3(xPos + xOffset, yPos + this.yOffset, zPosition - (zDepth / 2)), Quaternion.identity);
            Vector3 scale = new Vector3(this.TissueData.X, this.TissueData.Y, zDepth);
            currentLayer.transform.localScale = scale;

            tissueLayerList.Add(currentLayer);

            zPosition -= zDepth;
        }
    }

    dynamic ParseJson(string filename)
    {
        string myFileName = filename;
        string json = "";
        using (StreamReader r = new StreamReader(myFileName))
        {
            json = r.ReadToEnd();
        }
        int b = json.Length;
        Debug.Log("Finished parsing through " + filename);
        dynamic stuff = JsonConvert.DeserializeObject(json);
        Debug.Log("Finished creating dynamic object");
        Debug.Log(json.Length);
        return stuff;
    }

    void InitializeObjectsFromJson()
    {
        dynamic VertexLFP = ParseJson(FileManager.GetLFPFile(this.tissueInstance));
        dynamic Vertexparams = ParseJson(FileManager.GetParamsFile(this.tissueInstance));
        dynamic Vertexspikes = ParseJson(FileManager.GetSpikesFile(this.tissueInstance));
        List<dynamic> VertexConnections = new List<dynamic>();
        this.weightMappings = new Dictionary<int, int>();
        List<string> connectionFiles = FileManager.GetConnectionsFiles(this.tissueInstance);
        for(int i = 0; i < connectionFiles.Count; i++)
        {
            //int indexOfConnections = connectionFiles[i].IndexOf("/connections")
            string shortName = connectionFiles[i].Substring(connectionFiles[i].IndexOf("\\connections") + 1);
            string conSub = shortName.Substring("connections".Length, shortName.Length - ".json".Length - "connections".Length);
            string[] conMap = conSub.Split('_');
            if (conMap.Length != 2)
            {
                throw new Exception("Incorrect connection filenames!");
            }
            this.weightMappings.Add(Convert.ToInt32(conMap[0]), Convert.ToInt32(conMap[1]));

            VertexConnections.Add(ParseJson(connectionFiles[i]));
        }

        dynamic myTissueParams = Vertexparams.TissueParams;
        dynamic myRecordingSettings = Vertexparams.RecordingSettings;

        //GameObject tissueSlice = GameObject.Find("TissueSlice");
        this.TissueData = new TissueParams(myTissueParams);
        this.RecordingData = new RecordingSettings(myRecordingSettings);
        this.LFPData = new LocalFieldPotential(VertexLFP);
        this.ConnectionData = new ConnectionInfo(VertexConnections);
        this.SpikeTimes = new SpikeData(Vertexspikes);
        this.startTime = Time.time;
        this.currentSpikeIndex = 0;
    }

    public void Initialize(int tissueInstance, float xOffset, float yOffset)
    {
        this.tissueInstance = tissueInstance;
        this.xOffset = xOffset;
        this.yOffset = yOffset;
        this.electrodeIndex = 0;
        InitializeObjectsFromJson();
        CreateTissueLayers();
        SetTissueLayerColors();
        SetNeuronGroupColors();
        CreateNeurons();
        CreateElectrodes();
        CreateConnections();
        Constants.finishedInitialization[tissueInstance] = true;
    }

    void FixedUpdate()
    {
        if (Constants.finishedInitialization.TrueForAll(b => b))
        {

            //Neuronal spikes
            int wut = this.SpikeTimes.times.GetLength(0);
            //List<int> IDs = new List<int>();
            float endTime = this.startTime + (Time.fixedDeltaTime * Constants.timeScale);
            //float totalTime = endTime - this.startTime;

            //float currentNeuronTime = this.SpikeTimes.times[this.currentSpikeIndex, 1];
            //float currentNeuronTimeEnd = currentNeuronTime + Time.fixedDeltaTime;

            while (this.currentSpikeIndex < this.SpikeTimes.times.GetLength(0))
            {
                if (this.SpikeTimes.times[this.currentSpikeIndex, 1] < endTime)
                {
                    int neuronToSpike = (int)this.SpikeTimes.times[this.currentSpikeIndex, 0];
                    this.NeuronList[neuronToSpike - 1].GetComponent<Neuron>().spike();
                    this.currentSpikeIndex++;
                }
                else
                {
                    break;
                }
            }

            //Electrode values
            int currentElectrodeTimeStep = Convert.ToInt32(Math.Floor(endTime));
            for(this.electrodeIndex = 0; this.electrodeIndex < ElectrodeList.Length; this.electrodeIndex++)
            {
                if(currentElectrodeTimeStep < this.LFPData.LFPValues.GetLength(1))
                {
                    Electrode tmpElectrode = this.ElectrodeList[this.electrodeIndex].GetComponent<Electrode>();
                    if (tmpElectrode.LFP != this.LFPData.LFPValues[this.electrodeIndex, currentElectrodeTimeStep])
                    {
                        tmpElectrode.ChangeSize(this.LFPData.LFPValues[this.electrodeIndex, currentElectrodeTimeStep]);
                    }
                }
                else
                {
                    break;
                }
            }
            //Debug.Log("Start time = " + Convert.ToString(this.startTime));
            //Debug.Log("End time = " + Convert.ToString(endTime));
            //Debug.Log("Change in time = " + Convert.ToString((Time.fixedDeltaTime * Constants.timeScale)));

            this.startTime = endTime;
            
        }
    }
}