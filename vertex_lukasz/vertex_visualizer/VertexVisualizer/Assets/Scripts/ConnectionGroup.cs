using System.Collections.Generic;
using UnityEngine;

public class ConnectionGroup
{
    private List<GameObject> connections;
    public int neuronId;
    private bool active;
    public void CreateConnections(int neuronId, List<GameObject> connections)
    {
        this.neuronId = neuronId;
        this.connections = connections;
    }

    public void setWeightValues(WeightInfo info)
    {
        for(int i = 0; i < info.weight_mappings.Count; i++)
        {
            connections[i].GetComponent<Connection>().SetWeights(info.weight_mappings[i]);
        }
        this.setActive(false);
    }

    private void alterAllConnections()
    {
        foreach(GameObject obj in connections)
            obj.SetActive(this.active);
    }

    public void setActive(bool active)
    {
        this.active = active;
        alterAllConnections();
    }
}