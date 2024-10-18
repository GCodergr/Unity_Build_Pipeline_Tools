using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityBuildPipelineTools.EditorExtensions
{
    public class GameBuilderEditor : Editor
    {
        [MenuItem("Build/Windows 64 Mono", priority = 0)]
        public static void BuildWindows64Mono()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GetEnabledScenePaths();
            string locationPathName = "Builds/Windows64Mono/" + Application.productName + ".exe";
            buildPlayerOptions.locationPathName = locationPathName;
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.None;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Standalone, ApiCompatibilityLevel.NET_Standard_2_0);

            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            ShowExplorerOnSuccess(buildReport.summary, locationPathName);
            LogBuildSummary(buildReport.summary);
        }

        [MenuItem("Build/Windows 64 IL2CPP", priority = 1)]
        public static void BuildWindows64()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GetEnabledScenePaths();
            string locationPathName = "Builds/Windows64IL2CPP/" + Application.productName + ".exe";
            buildPlayerOptions.locationPathName = locationPathName;
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.None;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Standalone, ApiCompatibilityLevel.NET_Standard_2_0);

            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            ShowExplorerOnSuccess(buildReport.summary, locationPathName);
            LogBuildSummary(buildReport.summary);
        }

        [MenuItem("Build/Android Mono", priority = 2)]
        public static void BuildAndroidMono()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GetEnabledScenePaths();
            string locationPathName = "Builds/AndroidMono/" + Application.productName + ".apk";
            buildPlayerOptions.locationPathName = locationPathName;
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Android, ApiCompatibilityLevel.NET_Standard_2_0);
            
            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            ShowExplorerOnSuccess(buildReport.summary, locationPathName);
            LogBuildSummary(buildReport.summary);
        }
        
        [MenuItem("Build/Android IL2CPP", priority = 3)]
        public static void BuildAndroidIL2CPP()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GetEnabledScenePaths();
            string locationPathName = "Builds/AndroidIL2CPP/" + Application.productName + ".apk";
            buildPlayerOptions.locationPathName = locationPathName;
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Android, ApiCompatibilityLevel.NET_Standard_2_0);

            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            ShowExplorerOnSuccess(buildReport.summary, locationPathName);
            LogBuildSummary(buildReport.summary);
        }
        
        #region Utilities

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

        private static string SizeSuffix(ulong value, int decimalPlaces = 0)
        {
            if (value < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "value");
            }

            var mag = (int) Math.Max(0, Math.Log(value, 1024));
            var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
            return $"{adjustedSize} {sizeSuffixes[mag]}";
        }
        
        /// <summary>
        /// Shows Explorer/Finder when a build completed successfully
        /// </summary>
        /// <param name="buildSummary"></param>
        /// <param name="locationPathName"></param>
        private static void ShowExplorerOnSuccess(BuildSummary buildSummary, string locationPathName)
        {
            if (buildSummary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder(locationPathName);
            }
        }
        
        #endregion
    }
}