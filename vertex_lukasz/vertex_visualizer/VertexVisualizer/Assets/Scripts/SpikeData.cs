using UnityEngine;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;

public class SpikeData
{
    public float[,] times;
    public SpikeData(dynamic jsonObj)
    {
        //dynamic spikeTimes = jsonObj;
        List<dynamic> spikeMat = jsonObj.ToObject<List<dynamic>>();
        this.times = new float[spikeMat.Count, 2];
        for (int i = 0; i < spikeMat.Count; i++)
        {
            List<float> tmpList = spikeMat[i].ToObject<List<float>>();
            this.times[i, 0] = tmpList[0]; //NeuronID
            this.times[i, 1] = tmpList[1]; //Time
        }
        Debug.Log("Hello from spikedata");
    }
}