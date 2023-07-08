using UnityEngine;
using UnityEditor;

namespace gmtk_gamejam
{
    public class SpriteImporter : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            if(textureImporter.textureType == TextureImporterType.Sprite)
            {
                textureImporter.spritePixelsPerUnit = 32;
                textureImporter.filterMode = FilterMode.Point;
                textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            }
        }
    }
}
