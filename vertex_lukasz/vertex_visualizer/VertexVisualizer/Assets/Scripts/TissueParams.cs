
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CSharp.RuntimeBinder;
public class TissueParams
{
    public float X;
    public float Y;
    public float Z;
    //public float neuronDensity;
    public float numStrips;
    public float numLayers;
    public float numGroups;
    public string sliceShape;
    public float neuronCount;
    public List<float> layerBoundaryArr;
    public List<float> groupBoundaryIDArr;
    public float[,] somaPositionArr;

    public TissueParams(dynamic jsonObj)
    {
        this.X = (float)jsonObj.X;
        this.Y = (float)jsonObj.Y;
        this.Z = (float)jsonObj.Z;

        this.numLayers = (float)jsonObj.numLayers;
        this.numGroups = (float)jsonObj.numGroups;

        dynamic somevar = jsonObj.layerBoundaryArr; // of type {Newtonsoft.Json.Linq.JArray}
        dynamic slice = jsonObj.sliceShape;
        dynamic neuronc = jsonObj.N;
        dynamic soma = jsonObj.somaPositionMat;
        this.sliceShape = slice.ToObject<string>();
        //float count = somevar.Count;
        //string lol = somevar[0];
        //string hey = somevar.GetType().ToString();
        this.layerBoundaryArr = somevar.ToObject<List<float>>();
        this.groupBoundaryIDArr = jsonObj.groupBoundaryIDArr.ToObject<List<float>>();
        if(Constants.GroupBoundaryIDArr.Count == 0)
        {
            Constants.GroupBoundaryIDArr = this.groupBoundaryIDArr;
        }
        this.neuronCount = (float)jsonObj.N;
        List<dynamic> somaPositionMat = soma.ToObject<List<dynamic>>();
        somaPositionArr = new float[(int)this.neuronCount, 3];
        for (int i = 0; i < this.neuronCount; i++)
        {
            List<float> tmpList = somaPositionMat[i].ToObject<List<float>>();
            somaPositionArr[i, 0] = tmpList[0];
            somaPositionArr[i, 1] = tmpList[1];
            somaPositionArr[i, 2] = tmpList[2];
        }

        this.sliceShape = slice.ToObject<string>();

    }
    
}
