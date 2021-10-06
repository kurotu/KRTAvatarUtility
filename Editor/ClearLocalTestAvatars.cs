using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;

namespace KRT.KRTAvatarUtility
{
    static class ClearLocalTestAvatars
    {
        [MenuItem("Window/KRTAvatarUtility/Clear Local Test Avatars")]
        static void Clear()
        {
            Guid localLowId = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
            var localLow = GetKnownFolderPath(localLowId);

            // %UserProfile%\AppData\LocalLow\VRChat\VRChat\Avatars
            var avatarsFolder = Path.Combine(localLow, "VRChat\\VRChat\\Avatars");
            if (EditorUtility.DisplayDialog("Clear Local Test Avatars", $"Would you really clear *.vrca in \"{avatarsFolder}\" folder?", "OK", "Cancel"))
            {
                var files = Directory.EnumerateFiles(avatarsFolder, "*.vrca");
                foreach (var f in files)
                {
                    File.Delete(f);
                }
            }
        }

        // https://stackoverflow.com/questions/4494290/detect-the-location-of-appdata-locallow
        static string GetKnownFolderPath(Guid knownFolderId)
        {
            IntPtr pszPath = IntPtr.Zero;
            try
            {
                int hr = SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out pszPath);
                if (hr >= 0)
                    return Marshal.PtrToStringAuto(pszPath);
                throw Marshal.GetExceptionForHR(hr);
            }
            finally
            {
                if (pszPath != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(pszPath);
            }
        }

        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);
    }
}
