using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Connection : MonoBehaviour
{
    public float weight;
    public float radialScale;

    private int currentIndex;
    private float minValue;
    private float maxValue;

    private Color maxColor;
    private Color minColor;

    private Material material;

    float[,] weightValues;
    
    public void CreateConnections(float weight, Vector3 start, Vector3 end)
    {
        this.weight = weight;
        float xPos = (start.x + end.x) / 2;
        float yPos = (start.y + end.y) / 2;
        float zPos = (start.z + end.z) / 2;
        Vector3 scale = new Vector3(3f, Vector3.Distance(start, end) / 2, 3f);

        this.transform.localScale = scale;
        this.transform.localPosition = new Vector3(xPos, yPos, zPos);

        this.transform.up = end - start;
        this.material = GetComponent<Renderer>().material;
        this.currentIndex = 0;
        this.minColor = new Color(1, 1, 1);
        this.maxColor = Color.magenta;
        this.material.color = Color.yellow;
    }

    public void SetWeights(float[,] values)
    {
        minValue = 1000;
        maxValue = -1000;
        this.weightValues = values;
        for(int i = 0; i < values.GetLength(1); i++)
        {
            if(values[1,i] < minValue)
            {
                minValue = values[1, i];
            }
            else if(values[1, i] > maxValue)
            {
                maxValue = values[1, i];
            }
        }
        this.material.color = minColor;
    }

    void Update()
    {
        if (Constants.finishedInitialization.TrueForAll(b => b))
        {
            if (this.weightValues != null)
            {
                float previousIndex = this.currentIndex;
                while (this.currentIndex + 1 < this.weightValues.GetLength(1) && this.weightValues[0, this.currentIndex] < Constants.currentTime)
                {
                    this.currentIndex++;
                }

                if (previousIndex != this.currentIndex)
                {
                    float colorPercent = Mathf.InverseLerp(minValue, maxValue, this.weightValues[1, this.currentIndex]);

                    Color lerpedColor = Color32.Lerp(minColor, maxColor, colorPercent);
                    this.material.color = lerpedColor;
                }
            }
        }
    }
}
