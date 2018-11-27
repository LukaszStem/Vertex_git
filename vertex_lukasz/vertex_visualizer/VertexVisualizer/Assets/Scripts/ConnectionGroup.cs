using System.Collections.Generic;
using UnityEngine;

public class ConnectionGroup
{
    private List<GameObject> connections;
    public int neuronId;
    public void CreateConnecitons(int neuronId, List<GameObject> connections)
    {
        this.neuronId = neuronId;
        this.connections = connections;
    }
}