using UnityEditor;

namespace CarterGames.Assets.AudioManager
{
    public class AmAssetHelper : AssetPostprocessor
    {
        private static AudioManagerSettings settingsAsset;

        private static bool HasSettings => settingsAsset != null;
        
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            CheckForSettingsAsset();
        }

        
        private static void CheckForSettingsAsset()
        {
            if (HasSettings || AmEditorUtils.Settings != null) return;
            settingsAsset = AmEditorUtils.GenerateSettings(null);
            AmEditorUtils.Settings = settingsAsset;
        }
    }
}