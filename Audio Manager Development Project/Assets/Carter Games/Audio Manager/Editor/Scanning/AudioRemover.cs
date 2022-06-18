using UnityEditor;

namespace CarterGames.Assets.AudioManager.Editor
{
    public class AudioRemover : UnityEditor.AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            AudioScanner.RemoveNullEntriesInLibrary(assetPath);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}