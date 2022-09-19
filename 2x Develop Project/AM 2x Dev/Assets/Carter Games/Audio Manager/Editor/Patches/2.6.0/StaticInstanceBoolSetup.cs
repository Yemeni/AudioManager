using System;
using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class StaticInstanceBoolSetup : AssetPostprocessor
    {
        private static bool hasRun;


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (hasRun) return;

            AudioManagerEditorUtil.Settings.isUsingStatic = ScriptingDefineHandler.IsScriptingDefinePresent();
            hasRun = true;
        }
    }
}