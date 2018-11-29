using System;
using System.Collections.Generic;

public class WeightInfo
{
    public List<float[,]> weight_mappings;
    public WeightInfo(dynamic jsonObjs)
    {
        this.weight_mappings = new List<float[,]>();
        List<dynamic> jsonObj = jsonObjs.ToObject<List<dynamic>>();
        for (int i = 0; i < jsonObj.Count; i++)
        {
            dynamic currentObj = jsonObj[i];
            List<dynamic> innerConnections1 = currentObj.ToObject<List<dynamic>>();
            for (int j = 0; j < innerConnections1.Count; j++)
            {
                List<float> weights = innerConnections1[j].ToObject<List<float>>();
                float currentSimulationTime = 1;
                float currentWeight = weights[0];
                List<float> tmpWeightsTimes = new List<float>();
                List<float> tmpWeightsValues = new List<float>();
                tmpWeightsTimes.Add(currentSimulationTime);
                tmpWeightsValues.Add(currentWeight);
                for(int k = 1; k < weights.Count; k++)
                {
                    if(weights[k] != tmpWeightsValues[tmpWeightsValues.Count-1])
                    {
                        tmpWeightsTimes.Add((float)(k));
                        tmpWeightsValues.Add(weights[k]);
                    }
                }
                float[,] somelist = new float[2, tmpWeightsValues.Count];

                for(int k = 0; k < tmpWeightsValues.Count; k++)
                {
                    somelist[0, k] = tmpWeightsTimes[k];
                    somelist[1, k] = tmpWeightsValues[k];
                }
                this.weight_mappings.Add(somelist);
            }
        }
    }
}