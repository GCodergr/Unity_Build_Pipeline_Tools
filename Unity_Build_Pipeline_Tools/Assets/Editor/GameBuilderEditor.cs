using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GameBuilderEditor : Editor
{
    [MenuItem("Build/Windows 64 Mono")]
    public static void BuildWindows64Mono()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.scenes = GetEnabledScenePaths();
        buildPlayerOptions.locationPathName = "Builds/Windows64Mono/" + Application.productName + ".exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone,
            ApiCompatibilityLevel.NET_Standard_2_0);

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        LogBuildSummary(report.summary);
    }

    [MenuItem("Build/Windows 64 IL2CPP")]
    public static void BuildWindows64()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.scenes = GetEnabledScenePaths();
        buildPlayerOptions.locationPathName = "Builds/Windows64IL2CPP/" + Application.productName + ".exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone,
            ApiCompatibilityLevel.NET_Standard_2_0);

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        LogBuildSummary(report.summary);
    }

    #region Helper

    /// <summary>
    /// Returns the paths of the enabled scenes from Build Settings
    /// </summary>
    /// <returns></returns>
    private static string[] GetEnabledScenePaths()
    {
        var enabledScenePaths = new List<string>();
        // Get all the scenes from Build Settings
        var scenesInBuildSettings = EditorBuildSettings.scenes;

        for (int i = 0; i < scenesInBuildSettings.Length; i++)
        {
            // Check if the scene is enabled in build settings 
            if (scenesInBuildSettings[i].enabled)
            {
                enabledScenePaths.Add(scenesInBuildSettings[i].path);
            }
        }

        return enabledScenePaths.ToArray();
    }

    /// <summary>
    /// Logs a given build summary
    /// </summary>
    /// <param name="buildSummary"></param>
    private static void LogBuildSummary(BuildSummary buildSummary)
    {
        if (buildSummary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + SizeSuffix(buildSummary.totalSize) + " in " + buildSummary.totalTime +
                      " time");
        }

        if (buildSummary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    private static readonly string[] sizeSuffixes = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

    static string SizeSuffix(ulong value, int decimalPlaces = 0)
    {
        if (value < 0)
        {
            throw new ArgumentException("Bytes should not be negative", "value");
        }

        var mag = (int) Math.Max(0, Math.Log(value, 1024));
        var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
        return String.Format("{0} {1}", adjustedSize, sizeSuffixes[mag]);
    }

    #endregion
}