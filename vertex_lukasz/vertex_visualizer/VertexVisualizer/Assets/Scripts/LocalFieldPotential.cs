using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CSharp.RuntimeBinder;

public class LocalFieldPotential
{
    public float[,] LFPValues;
    public int firstDimLength;
    public int secondDimLength;
    public LocalFieldPotential(dynamic jsonObj)
    {
        List<dynamic> LFPMat = jsonObj.ToObject<List<dynamic>>();
        List<float> innerList = LFPMat[0].ToObject<List<float>>();
        
        LFPValues = new float[LFPMat.Count, innerList.Count];
        firstDimLength = LFPMat.Count;
        secondDimLength = innerList.Count;
        for (int i = 0; i < firstDimLength; i++)
        {
            innerList = LFPMat[i].ToObject<List<float>>();
            for (int j = 0; j < secondDimLength; j++)
            {
                LFPValues[i, j] = innerList[j];
            }
        }
        
    }
}