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
    public List<WeightInfo> WeightData;
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
    private Dictionary<int, int> connectionMappings;
    private List<int> weightMappings;

    private int currentConnectionGroupIndex;
    private bool selectingNextConnectionGroup;

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

    void CreateConnections()
    {
        this.connectionGroups = new List<ConnectionGroup>();
        List<dynamic> VertexWeights = new List<dynamic>();
        List<string> weightFiles = FileManager.GetWeightFiles(this.tissueInstance);
        this.weightMappings = new List<int>();
        float count = 0f;
        foreach (int key in connectionMappings.Keys)
        {
            int preNeuron = connectionMappings[key];
            int[] postNeurons = this.ConnectionData.connection_mappings[this.connectionGroups.Count];
            List<GameObject> currentConnections = new List<GameObject>();
            for (int j = 0; j < postNeurons.Length; j++)
            {
                Vector3 preNeuronPos = new Vector3(this.TissueData.somaPositionArr[preNeuron - 1, 0] + xOffset, this.TissueData.somaPositionArr[preNeuron - 1, 1] + yOffset, this.TissueData.somaPositionArr[preNeuron - 1, 2]);
                Vector3 postNeuronPos = new Vector3(this.TissueData.somaPositionArr[postNeurons[j] - 1, 0] + xOffset, this.TissueData.somaPositionArr[postNeurons[j] - 1, 1] + yOffset, this.TissueData.somaPositionArr[postNeurons[j] - 1, 2]);
                if (preNeuronPos.Equals(postNeuronPos))
                    continue;
                GameObject obj = Instantiate(connection, new Vector3(0f, 0f, 0f), Quaternion.identity);
                obj.GetComponent<Connection>().CreateConnections(1f, preNeuronPos, postNeuronPos);
                currentConnections.Add(obj);
                //Creating for just 1 connection now
                //j = postNeurons.Length;
            }
            ConnectionGroup group = new ConnectionGroup();
            //if (key == 10)
            //    group.CreateConnections(preNeuron, currentConnections, true);
            //else
            //    group.CreateConnections(preNeuron, currentConnections, false);
            group.CreateConnections(preNeuron, currentConnections);

            //int indexOfConnections = connectionFiles[i].IndexOf("/connections")
            string leftPath = weightFiles[this.connectionGroups.Count].Substring(0,weightFiles[this.connectionGroups.Count].IndexOf("\\weights") + 1);
            //string weightSub = shortName.Substring("weights".Length, shortName.Length - ".json".Length - "weights".Length);
            //Will need to modulo against minimum neuron ID
            string fullPath = leftPath + "weights" + (preNeuron%799).ToString() + ".json";
            Debug.Log("Full path");
            //this.weightMappings.Add(Convert.ToInt32(weightSub));

            WeightInfo info = new WeightInfo(ParseJson(fullPath));
            //this.WeightData.Add(info);
            group.setWeightValues(info);

            //i = weightFiles.Count;
            this.connectionGroups.Add(group);
            count++;
            Constants.connectionsLoaded = count / connectionMappings.Keys.Count;
        }

        this.connectionGroups[0].setActive(true);
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

    

    void UpdateConnectionGroupText()
    {
        string text = "Switching to view connection group:" + this.currentConnectionGroupIndex + " with neuronID:" + this.connectionGroups[this.currentConnectionGroupIndex].neuronId.ToString();
        //TODO:implement in seperate elements
    }

    void InitializeObjectsFromJson()
    {
        dynamic VertexLFP = ParseJson(FileManager.GetLFPFile(this.tissueInstance));
        dynamic Vertexparams = ParseJson(FileManager.GetParamsFile(this.tissueInstance));
        dynamic Vertexspikes = ParseJson(FileManager.GetSpikesFile(this.tissueInstance));
        List<dynamic> VertexConnections = new List<dynamic>();
        List<dynamic> VertexWeights = new List<dynamic>();
        this.connectionMappings = new Dictionary<int, int>();
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
            this.connectionMappings.Add(Convert.ToInt32(conMap[0]), Convert.ToInt32(conMap[1]));

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
        this.currentConnectionGroupIndex = 0;
        this.selectingNextConnectionGroup = false;
        this.WeightData = new List<WeightInfo>();
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

            if(this.tissueInstance == 0)
            {
                Constants.currentTime = this.startTime;
            }
            
        }
    }

    private void Update()
    {
        if (!this.selectingNextConnectionGroup && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.selectingNextConnectionGroup = true;
            this.connectionGroups[this.currentConnectionGroupIndex].setActive(false);
            this.currentConnectionGroupIndex = (int)Mathf.Repeat((float)(this.currentConnectionGroupIndex - 1), (float)this.connectionGroups.Count);
            this.connectionGroups[this.currentConnectionGroupIndex].setActive(true);
            
            Debug.Log("Switching to view connection group:" + this.currentConnectionGroupIndex + " with neuronID:" + this.connectionGroups[this.currentConnectionGroupIndex].neuronId.ToString());
        }
        else if(!this.selectingNextConnectionGroup && Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.selectingNextConnectionGroup = true;
            this.connectionGroups[this.currentConnectionGroupIndex].setActive(false);
            this.currentConnectionGroupIndex = (int)Mathf.Repeat((float)(this.currentConnectionGroupIndex + 1), (float)this.connectionGroups.Count);
            this.connectionGroups[this.currentConnectionGroupIndex].setActive(true);

            Debug.Log("Switching to view connection group:" + this.currentConnectionGroupIndex + " with neuronID:" + this.connectionGroups[this.currentConnectionGroupIndex].neuronId.ToString());
        }
        else if(this.selectingNextConnectionGroup && Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.selectingNextConnectionGroup = false;
        }
        else if (this.selectingNextConnectionGroup && Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.selectingNextConnectionGroup = false;
        }
    }
}