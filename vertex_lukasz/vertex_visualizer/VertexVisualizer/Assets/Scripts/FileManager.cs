
using System.Collections.Generic;
using System.IO;

public static class FileManager
{
    private static List<string>[] TissueSliceFiles;

    public static void Initialize(string topLevelPath)
    {
        string[] directories = Directory.GetDirectories(topLevelPath);

        TissueSliceFiles = new List<string>[directories.Length];

        for (int i = 0; i < directories.Length; i++)
        {
            TissueSliceFiles[i] = new List<string>();
            foreach (string file in Directory.GetFiles(directories[i]))
            {
                TissueSliceFiles[i].Add(file);
            }
        }
    }

    public static int GetNumberOfTissueSlices()
    {
        return TissueSliceFiles.Length;
    }

    public static string GetParamsFile(int i)
    {
        foreach(string file in TissueSliceFiles[i])
        {
            if (file.Contains("params.json"))
            {
                return file;
            }
        }
        return null;
    }

    public static string GetSpikesFile(int i)
    {
        foreach (string file in TissueSliceFiles[i])
        {
            if (file.Contains("spikes.json"))
            {
                return file;
            }
        }
        return null;
    }

    public static string GetLFPFile(int i)
    {
        foreach (string file in TissueSliceFiles[i])
        {
            if (file.Contains("LFP.json"))
            {
                return file;
            }
        }
        return null;
    }

    public static List<string> GetWeightFiles(int i)
    {
        List<string> weightFiles = new List<string>();
        foreach (string file in TissueSliceFiles[i])
        {
            if (file.Contains("weights"))
            {
                weightFiles.Add(file);
            }
        }
        return weightFiles;
    }

    public static List<string> GetConnectionsFiles(int i)
    {
        List<string> connectionFiles = new List<string>();
        foreach (string file in TissueSliceFiles[i])
        {
            if (file.Contains("connections"))
            {
                connectionFiles.Add(file);
            }
        }
        return connectionFiles;
    }
}