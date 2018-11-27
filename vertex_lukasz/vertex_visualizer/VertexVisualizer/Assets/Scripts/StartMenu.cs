using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
    // Use this for initialization
    void Start () {
        string path = EditorUtility.OpenFolderPanel("Select Data folder", "", "");

        FileManager.Initialize(path);
        SceneManager.LoadScene("Main");

        
    }
	
}
