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
    private float currentTime;
    private int electrodeID;

    public void SetElectrode(int id, float maxValue, float minValue)
    {
        this.electrodeID = id;
        this.LFP = 0;
        this.maxLFPValue = maxValue;
        this.minLFPValue = minValue;
        this.material = GetComponent<Renderer>().material;
        this.gameObject.transform.SetParent(GameObject.Find("ElectrodeGroup").transform);
        this.currentTime = Time.time;
    }
    // Use this for initialization
    public void ChangeSize(float LFPVal)
    {
        //Debug.Log("Should see a value change!");
        this.LFP = LFPVal;
        float tmpScale;
        if (this.minLFPValue < 0)
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

    // Update is called once per frame
    void Update()
    {
        //if (Constants.finishedInitialization.TrueForAll(b => b))
        //{
        //    this.currentTime += Time.deltaTime * Constants.timeScale;
        //    int currentTimeStep = Convert.ToInt32(Math.Floor(this.currentTime));
        //    //Debug.Log("Current Time Step:" + currentTimeStep.ToString());
        //    //Debug.Log("ElectrodeID:" + ((int)(this.electrodeID)).ToString());
        //    float tmpLFP = this.LFP;
        //    if (currentTimeStep < Constants.LFPData.GetLength(1))
        //        tmpLFP = Constants.LFPData[this.electrodeID, currentTimeStep];
        //    if (tmpLFP != this.LFP)
        //    {
        //        this.LFP = tmpLFP;
        //        float tmpScale;
        //        if(this.minLFPValue < 0)
        //        {
        //            tmpScale = (this.LFP + Mathf.Abs(this.minLFPValue)) / this.maxLFPValue + Mathf.Abs(this.minLFPValue);
        //        }
        //        else
        //        {
        //            tmpScale = (this.LFP - this.minLFPValue) / this.maxLFPValue - this.minLFPValue;
        //        }
        //        //float tmpScale = (Mathf.InverseLerp(this.minLFPValue, this.maxLFPValue, this.LFP)) * 10;
        //        tmpScale = tmpScale * 10;
        //        //Debug.Log(tmpScale);
        //        this.transform.localScale = new Vector3(maxSizeScale * tmpScale, this.transform.localScale.y, maxSizeScale * tmpScale);
        //    }
        //}
    }
}
