using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class ScriptingDefineHandler : IActiveBuildTargetChanged
    {
        private static readonly string[] AssetDefinitions = new string[2] { "Use_CGAudioManager_Static", "USE_CG_AM_STATIC" };
        
        
        private static string GetScriptingDefines(BuildTarget buildTarget) 
        {
            var group = BuildPipeline.GetBuildTargetGroup(buildTarget);
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        }

        private static void SetScriptingDefines(string scriptingDefines, BuildTarget buildTarget) 
        {
            var group = BuildPipeline.GetBuildTargetGroup(buildTarget);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, scriptingDefines);
        }

        private static string[] GetScriptingDefinesCollection(BuildTarget buildTarget) 
        {
            string scriptingDefines = GetScriptingDefines(buildTarget);
            string[] separateScriptingDefines = scriptingDefines.Split(';');
            return separateScriptingDefines;
        }
        
        public static bool IsScriptingDefinePresent() 
        {
            string[] scriptingDefines = GetScriptingDefinesCollection(EditorUserBuildSettings.activeBuildTarget);
            return scriptingDefines.Contains(AssetDefinitions[0]) || scriptingDefines.Contains(AssetDefinitions[1]);
        }
        
        private static bool IsScriptingDefinePresent(string scriptingDefine, BuildTarget buildTarget) 
        {
            string[] scriptingDefines = GetScriptingDefinesCollection(buildTarget);
            return scriptingDefines.Contains(scriptingDefine);
        }

        public static void AddScriptingDefine(string define, params BuildTarget[] buildTargets)
        {
            if (buildTargets == null) return;

            foreach (BuildTarget buildTarget in buildTargets) 
            {
                if (IsScriptingDefinePresent(define, buildTarget)) continue;
                
                string scriptingDefines = GetScriptingDefines(buildTarget) + ";" + define;
                SetScriptingDefines(scriptingDefines, buildTarget);
            }
        }

        private static void RemoveScriptingDefine(string define, params BuildTarget[] buildTargets)
        {
            if (buildTargets == null) return;

            foreach (BuildTarget buildTarget in buildTargets)
            {
                if (!IsScriptingDefinePresent(define, buildTarget)) continue;
                
                List<string> scriptingDefines = GetScriptingDefinesCollection(buildTarget).ToList();
                int removeIndex = scriptingDefines.FindIndex(item => item == define);
                scriptingDefines.RemoveAt(removeIndex);
                string updatedScriptingDefines = string.Join(";", scriptingDefines);
                SetScriptingDefines(updatedScriptingDefines, buildTarget);
            }
        }

        public int callbackOrder { get; }
        
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            if (!AudioManagerEditorUtil.Settings.isUsingStatic) return;
            if (!IsScriptingDefinePresent(AssetDefinitions[0], EditorUserBuildSettings.activeBuildTarget)) return; 
            if (!IsScriptingDefinePresent(AssetDefinitions[1], EditorUserBuildSettings.activeBuildTarget)) return;
            
        }
    }
}