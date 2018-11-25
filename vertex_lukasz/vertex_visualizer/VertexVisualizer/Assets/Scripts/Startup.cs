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
    public GameObject electrode;
    public List<GameObject> tissueLayerList;
    public SpikeData SpikeTimes { get; set; }

    private float startTime;
    private int currentSpikeIndex;
    public float timeScale = 1;

    //-------UI--------
    public Text timeText;
    public Text statusText;
    private bool holdingDownKey;
    private List<string> keys = new List<string>();
    private string keyHeldDownPrev;
    private string keyHeldDown;

	// Use this for initialization
	void Start () {
        this.keyHeldDown = "0";
        this.keyHeldDownPrev = "0";
        keys = new List<string>();
        for(int i = 0; i < 10; i++)
            keys.Add(i.ToString());
        Constants.finishedInitialization = new List<bool>();
        for (int i = 0; i < FileManager.GetNumberOfTissueSlices(); i++)
        {
            Constants.finishedInitialization.Add(false);
            TissueSlice slice = new TissueSlice(i, i * 3000);
            Constants.TissueSlices.Add(slice);
        }

        this.holdingDownKey = false;

    }

    void FixedUpdate()
    {
        
        if (Constants.finishedInitialization.TrueForAll(b => b))
        {
            //List<int> IDs = new List<int>();
            float endTime = this.startTime + (Time.fixedDeltaTime * timeScale);
            
            this.startTime = endTime;
            timeText.text = "Time:" + this.startTime.ToString("0.00");
        }
    }
}
