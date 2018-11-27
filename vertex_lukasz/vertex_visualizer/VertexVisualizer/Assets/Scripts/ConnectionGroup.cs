using System.Collections.Generic;
using UnityEngine;

public class ConnectionGroup
{
    private List<GameObject> connections;
    public int neuronId;
    private bool active;
    public void CreateConnecitons(int neuronId, List<GameObject> connections, bool active)
    {
        this.neuronId = neuronId;
        this.connections = connections;
        this.active = active;
        alterAllConnections();
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