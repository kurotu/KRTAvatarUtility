using System.IO;
using UnityEditor;
using UnityEngine;

namespace KRT.KRTAvatarUtility
{
    public static class KRTAvatarUtility
    {
        private const string EditorGUID = "9287031fcccc8c646b13d734fc9f0cdc";
        internal static string AssetRoot => Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(EditorGUID));

        public static void ExportUnityPackage(string filename)
        {
            AssetDatabase.ExportPackage(AssetRoot, filename, ExportPackageOptions.Recurse);
        }

        private static void Export()
        {
            ExportUnityPackage("KRTAvatarUtility.unitypackage");
        }
    }
}
