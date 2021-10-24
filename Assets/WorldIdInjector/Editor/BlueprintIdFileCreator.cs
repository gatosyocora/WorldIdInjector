using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gatosyocora.WorldIdInjector
{
    public class BlueprintIdFileCreator : EditorWindow
    {
        public const string BLUEPRINT_ID_FILE_PATH = "Assets/blueprintId.txt";

        private string blueprintId;

        [MenuItem("VRChat SDK/Create Blueprint Id file", priority = 9999)]
        private static void Open()
        {
            GetWindow<BlueprintIdFileCreator>("BlueprintIdFileCreator");
        }

        private void OnGUI()
        {
            blueprintId = EditorGUILayout.TextField("Blueprint Id", blueprintId);

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(blueprintId)))
            {
                if (GUILayout.Button("Create/Update Blueprint Id file"))
                {
                    CreateOrUpdateBlueprintId(blueprintId);
                }
            }
        }

        private void CreateOrUpdateBlueprintId(string blueprintId)
        {
            if (Directory.Exists(Path.GetDirectoryName(BLUEPRINT_ID_FILE_PATH)))
            {
                File.WriteAllText(BLUEPRINT_ID_FILE_PATH, blueprintId);
                AssetDatabase.Refresh();
            }
        }
    }
}
