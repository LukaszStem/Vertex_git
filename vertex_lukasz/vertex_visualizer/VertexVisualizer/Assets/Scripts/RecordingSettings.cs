using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CSharp.RuntimeBinder;

public class RecordingSettings
{
    public List<Vector3> meaPosList;
    public int numElectrodes;
    public RecordingSettings(dynamic jsonObj)
    {
        numElectrodes = jsonObj.numElectrodes.ToObject<int>();
        meaPosList = new List<Vector3>();
        
        float[] xPos = new float[numElectrodes];
        float[] yPos = new float[numElectrodes];
        float[] zPos = new float[numElectrodes];
        
        //Decipher X positions
        List<dynamic> meaMatX = jsonObj.meaXpositions.ToObject<List<dynamic>>();
        for (int i = 0; i < meaMatX.Count; i++)
        {
            List<dynamic> innerList = meaMatX[i].ToObject<List<dynamic>>();
            for (int j = 0; j < innerList.Count; j++)
            {
                List<float> valueList = innerList[j].ToObject<List<float>>();
                for(int k = 0; k < valueList.Count; k++)
                {
                    xPos[(j * valueList.Count) + k] = valueList[k];
                }
            }
        }

        //Decipher Y positions
        List<dynamic> meaMatY = jsonObj.meaYpositions.ToObject<List<dynamic>>();
        for (int i = 0; i < meaMatY.Count; i++)
        {
            List<dynamic> innerList = meaMatY[i].ToObject<List<dynamic>>();
            for (int j = 0; j < innerList.Count; j++)
            {
                List<float> valueList = innerList[j].ToObject<List<float>>();
                for (int k = 0; k < valueList.Count; k++)
                {
                    yPos[(j * valueList.Count) + k] = valueList[k];
                }
            }
        }

        //Decipher Z positions
        List<dynamic> meaMatZ = jsonObj.meaZpositions.ToObject<List<dynamic>>();
        for (int i = 0; i < meaMatZ.Count; i++)
        {
            List<dynamic> innerList = meaMatZ[i].ToObject<List<dynamic>>();
            for (int j = 0; j < innerList.Count; j++)
            {
                List<float> valueList = innerList[j].ToObject<List<float>>();
                for (int k = 0; k < valueList.Count; k++)
                {
                    zPos[(j * valueList.Count) + k] = valueList[k];
                }
            }
        }
        for (int i = 0; i < numElectrodes; i++)
        {
            meaPosList.Add(new Vector3(xPos[i], yPos[i], zPos[i]));
        }
    }
}