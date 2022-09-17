using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    /// <summary>
    /// Handles any helper methods for managing the settings asset for the asset...
    /// </summary>
    public sealed class SettingsAssetEditorUtil : AssetPostprocessor
    {
        //
        //  Properties
        //
        
        
        /// <summary>
        /// Checks to see if the system has a settings asset...
        /// </summary>
        private static bool HasSettings => AudioManagerEditorUtil.Settings != null;
        
        
        //
        //  Asset Post Process Method
        //
        
        
        /// <summary>
        /// Runs when Unity processes assets...
        /// </summary>
        /// <remarks>Automatically runs...</remarks>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            CheckForSettingsAsset();
        }

        
        //
        //  Methods
        //
        
        
        /// <summary>
        /// Generates a settings asset if one doesn't already exist...
        /// </summary>
        private static void CheckForSettingsAsset()
        {
            if (HasSettings) return;
            AudioManagerEditorUtil.Settings = AudioManagerEditorUtil.GenerateSettings(null);
        }
    }
}