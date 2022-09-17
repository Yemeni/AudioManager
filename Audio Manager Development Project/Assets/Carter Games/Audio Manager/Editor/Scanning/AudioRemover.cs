using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    /// <summary>
    /// Removes AudioClips from the library when the clip is removed the project...
    /// </summary>
    public class AudioRemover : UnityEditor.AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            AudioScanner.RemoveNullEntriesInLibrary(assetPath);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}