using UnityEngine;
using System.Collections;
using System;

public class Electrode : MonoBehaviour
{
    private Material material;
    public float LFP;
    public float maxSizeScale;
    private float maxLFPValue;
    private float minLFPValue;
    private float currentValue;
    private int electrodeID;

    public void SetElectrode(int id, float maxValue, float minValue)
    {
        this.electrodeID = id;
        this.LFP = 0;
        this.maxLFPValue = maxValue;
        this.minLFPValue = minValue;
        this.material = GetComponent<Renderer>().material;
        this.gameObject.transform.SetParent(GameObject.Find("ElectrodeGroup").transform);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Constants.finishedInitialization)
        {
            int currentTimeStep = Convert.ToInt32(Math.Floor(Time.time));
            //Debug.Log("Current Time Step:" + currentTimeStep.ToString());
            //Debug.Log("ElectrodeID:" + ((int)(this.electrodeID)).ToString());
            float tmpLFP = Constants.LFPData.LFPValues[this.electrodeID, currentTimeStep];
            if (tmpLFP != this.LFP)
            {
                this.LFP = tmpLFP;
                float tmpScale;
                if(this.minLFPValue < 0)
                {
                    tmpScale = (this.LFP + Mathf.Abs(this.minLFPValue)) / this.maxLFPValue + Mathf.Abs(this.minLFPValue);
                }
                else
                {
                    tmpScale = (this.LFP - this.minLFPValue) / this.maxLFPValue - this.minLFPValue;
                }
                //float tmpScale = (Mathf.InverseLerp(this.minLFPValue, this.maxLFPValue, this.LFP)) * 10;
                tmpScale = tmpScale * 10;
                //Debug.Log(tmpScale);
                this.transform.localScale = new Vector3(maxSizeScale * tmpScale, this.transform.localScale.y, maxSizeScale * tmpScale);
            }
        }
    }
}
