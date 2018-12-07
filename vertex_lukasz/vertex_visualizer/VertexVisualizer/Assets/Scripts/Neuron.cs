using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour {

    
    private Color SpikingColor;
    private int NeuronID;
    private Material material;
    private bool spiking = false;
    private float spikeStartTime;
    public float spikeDuration; //In seconds
    public float emissionIntensity;
    public float emission;
    private bool justSpiked;
    private float currentTime;
    // Use this for initialization
    void Start () {
		
	}

    public void SetID(int x)
    {
        this.NeuronID = x;

        this.gameObject.transform.SetParent(GameObject.Find("NeuronGroup").transform);
        this.material = GetComponent<Renderer>().material;
        this.emission = 0;
        this.justSpiked = false;
        
        /* Not in use
        int lowerNeuronRange = 99999;
        int higherNeuronRange = 9999999;

        if(this.NeuronID > lowerNeuronRange && this.NeuronID < higherNeuronRange)
        {
            float newXScale = this.transform.localScale.x * 3;
            float newYScale = this.transform.localScale.y * 3;
            float newZScale = this.transform.localScale.z * 3;

            this.transform.localScale = new Vector3(newXScale, newYScale, newZScale);
        }
        else if(this.NeuronID == 874)
        {
            float newXScale = this.transform.localScale.x * 6;
            float newYScale = this.transform.localScale.y * 6;
            float newZScale = this.transform.localScale.z * 6;

            this.transform.localScale = new Vector3(newXScale, newYScale, newZScale);
        }
        else if (this.NeuronID == 875)
        {
            float newXScale = this.transform.localScale.x * 9;
            float newYScale = this.transform.localScale.y * 9;
            float newZScale = this.transform.localScale.z * 9;

            this.transform.localScale = new Vector3(newXScale, newYScale, newZScale);
        }
        */

        SetColor();
    }

    private void SetColor()
    {
        for(int i = 1; i < Constants.GroupBoundaryIDArr.Count; i++)
        {
            if(this.NeuronID < Constants.GroupBoundaryIDArr[i])
            {
                this.SpikingColor = Constants.NeuronColorList[i-1];
                break;
            }
        }
        Constants.ChangeColor(this.gameObject, this.SpikingColor.a, this.SpikingColor.r, this.SpikingColor.g, this.SpikingColor.b);
        this.material.SetColor("_EmissionColor", this.SpikingColor * Mathf.LinearToGammaSpace(0f));
        currentTime = Time.time;
    }

    public void spike()
    {
        if(!this.spiking)
        {
            //Debug.Log("Should see a spike!");
            this.spikeStartTime = Time.time;
            this.spiking = true;
        }
        else
        {
            Debug.LogWarning("Tried spiking Neuron #" + this.NeuronID + " when it was already spiking!");
        }
    }

    // Update is called once per frame
    void Update () {
        if(this.spiking)
        {
            float endTime = this.spikeStartTime + this.spikeDuration;
            if (Time.time < endTime)
            {
                // If LERP is required, spike flash is too fast....
                /*float timeOfHighestEmission = this.spikeStartTime + this.spikeDuration / 2;
                if(Time.time < timeOfHighestEmission)
                {
                    this.emission = emissionIntensity * (Mathf.Lerp(this.spikeStartTime, timeOfHighestEmission, Time.time));
                }
                else
                {
                    this.emission = emissionIntensity * (1 - Mathf.Lerp(timeOfHighestEmission, endTime, Time.time));
                }*/

                //Just on/off instead of LERP
                if (!this.justSpiked)
                {
                    this.emission = emissionIntensity;

                    Color finalColor = this.SpikingColor * Mathf.LinearToGammaSpace(emission);

                    this.material.SetColor("_EmissionColor", finalColor);
                    this.justSpiked = true;
                }
            }
            else
            {
                Color finalColor = this.SpikingColor * Mathf.LinearToGammaSpace(0);
                this.material.SetColor("_EmissionColor", finalColor);
                this.justSpiked = false;
                this.spiking = false;
            }
        }
        // = Mathf.PingPong(Time.time * 5, 2.0f);
        //Color baseColor = this.SpikingColor; //Replace this with whatever you want for your base color at emission level '1'

        //Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        //this.material.SetColor("_EmissionColor", finalColor);
    }
}
