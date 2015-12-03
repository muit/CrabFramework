using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MainEditor : EditorWindow
{
    //TOOLS
    [MenuItem("Crab/Tools")]
    public static void ShowTools() {
        EditorWindow.GetWindow(typeof(ToolMenu));
    }

    [MenuItem("Crab/Tools", true)]
    private static bool ShowToolsValidation() {
        return IsSetup();
    }


    //DATABASES
    [MenuItem("Crab/Databases")]
    public static void ShowDatabases() {
        EditorWindow.GetWindow(typeof(Databases));
    }

    [MenuItem("Crab/Databases", true)]
    private static bool ShowDatabasesValidation() {
        return IsSetup();
    }


    //SETUP
    [MenuItem("Crab/Setup this Scene")]
    public static void Setup() {
        GameObject sceneObj = new GameObject("Scene Manager");
        sceneObj.AddComponent(typeof(SceneScript));
        sceneObj.AddComponent(typeof(Cache));
        sceneObj.AddComponent(typeof(GameStats));
        sceneObj.AddComponent(typeof(ItemDatabase));
    }

    [MenuItem("Crab/Setup this Scene", true)]
    private static bool SetupValidation() {
        return !IsSetup();
    }


    public static bool IsSetup() {
        return FindObjectOfType<Cache>() && FindObjectOfType<SceneScript>();
    }
}