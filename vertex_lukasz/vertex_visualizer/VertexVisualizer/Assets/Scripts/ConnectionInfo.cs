using System;
using System.Collections.Generic;

public class ConnectionInfo
{
    public List<int[]> connection_mappings;
    public ConnectionInfo(List<dynamic> jsonObjs)
    {
        connection_mappings = new List<int[]>();
        for (int i = 0; i < jsonObjs.Count; i++)
        {
            dynamic currentObj = jsonObjs[i];
            List<dynamic> innerConnections1 = currentObj.ToObject<List<dynamic>>();
            for(int j = 0; j < innerConnections1.Count; j++)
            {
                List<float> innerConnections2 = innerConnections1[j].ToObject<List<float>>();
                int[] connection_arr  = Array.ConvertAll(innerConnections2.ToArray(), x => (int)x);
                connection_mappings.Add(connection_arr);
            }
        }
    }
}