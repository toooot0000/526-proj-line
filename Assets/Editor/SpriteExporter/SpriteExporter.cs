using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.SpriteExporter{
    /// <summary>
    ///     导出精灵工具
    /// </summary>
    public static class ExportSpriteEditor{
        [MenuItem("Tools/导出精灵")]
        private static void ExportSprite(){
            var resourcesPath = "Assets/Resources/";
            foreach (var obj in Selection.objects){
                var selectionPath = AssetDatabase.GetAssetPath(obj);

                if (selectionPath.StartsWith(resourcesPath)){
                    var selectionExt = Path.GetExtension(selectionPath);
                    if (selectionExt.Length == 0){
                        Debug.LogError($"资源{selectionPath}的扩展名不对，请选择图片资源");
                        continue;
                    }

                    // 设置图片属性
//                string path = AssetDatabase.GetAssetPath(obj);
//                TextureImporter ti = (TextureImporter) TextureImporter.GetAtPath(path);
//                ti.textureType = TextureImporterType.Sprite;
//                ti.spriteImportMode = SpriteImportMode.Multiple;
//                // 旧版Unity方法
//                  ti.textureFormat = TextureImporterFormat.RGBA32;
//                ti.textureCompression = TextureImporterCompression.Uncompressed;
//                ti.isReadable = true;
//                AssetDatabase.ImportAsset(path);

                    // 如果selectionPath = "Assets/Resources/UI/Common.png"
                    // 那么loadPath = "UI/Common"
                    var loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                    loadPath = loadPath.Substring(resourcesPath.Length);

                    // 加载此文件下的所有资源
                    var sprites = Resources.LoadAll<Sprite>(loadPath);
                    if (sprites.Length > 0){
                        // 创建导出目录
                        var exportPath = Application.dataPath + "/ExportSprite/" + loadPath;
                        Directory.CreateDirectory(exportPath);

                        foreach (var sprite in sprites){
                            var tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height,
                                sprite.texture.format, false);
                            tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                                (int)sprite.rect.width, (int)sprite.rect.height));
                            tex.Apply();

                            // 将图片数据写入文件
                            File.WriteAllBytes(exportPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                        }

                        Debug.Log("导出精灵到" + exportPath);
                    }

                    Debug.Log("导出精灵完成");
                    // 刷新资源
                    AssetDatabase.Refresh();
                } else{
                    Debug.LogError($"请将资源放在{resourcesPath}目录下");
                }
            }
        }
    }
}