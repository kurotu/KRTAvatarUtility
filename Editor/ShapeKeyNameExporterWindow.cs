using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace KRT.KRTAvatarUtility
{
    public class ShapeKeyNameExporterWindow : EditorWindow
    {
        GameObject gameObject;

        [MenuItem("Window/KRTAvatarUtility/Export ShapeKey Names")]
        static void InitOnMenu()
        {
            var window = GetWindow<ShapeKeyNameExporterWindow>();
            window.Show();
        }

        void OnGUI()
        {
            titleContent.text = "Export ShapeKey Names";
            gameObject = (GameObject)EditorGUILayout.ObjectField("FBX/Prefab", gameObject, typeof(GameObject), false);
            EditorGUILayout.Space();
            if (GUILayout.Button("Export ShapeKey Names"))
            {
                OnClickExport();
            }
        }

        private void OnClickExport()
        {
            var renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            var data = renderers.Select(r =>
            {
                var shapeKeys = new List<string>();
                for (var i = 0; i < r.sharedMesh.blendShapeCount; i++)
                {
                    shapeKeys.Add(r.sharedMesh.GetBlendShapeName(i));
                }
                return new KeyValuePair<string, List<string>>(r.sharedMesh.name, shapeKeys);
            })
            .ToDictionary(p => p.Key, p => p.Value)
            .ToDictionary(p => p.Key, p => ShrinkName(p.Value));

            var path = EditorUtility.SaveFilePanel("Export ShapeKey Names", "Assets", "ShapeKeyNames.json", "json");
            if (path == string.Empty)
            {
                return;
            }
            var s = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, s);
        }

        private Dictionary<string, string> ShrinkName(List<string> keys)
        {
            var knownShrunkCounts = new Dictionary<string, int>();
            var list = keys.ToDictionary(k =>
            {
                var s = k.Split('.').Last();
                if (knownShrunkCounts.ContainsKey(s))
                {
                    knownShrunkCounts[s] += 1;
                    s = s + string.Format(".{0:000}", knownShrunkCounts[s]);
                }
                else
                {
                    knownShrunkCounts[s] = 0;
                }
                return s;
            }, k => k);
            return list;
        }
    }
}
