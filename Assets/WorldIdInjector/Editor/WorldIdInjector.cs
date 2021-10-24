using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using VRC.Core;
using VRC.SDKBase;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Gatosyocora.WorldIdInjector
{
    public class WorldIdInjector : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -1;

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            if (ExistBlueprintIdFile())
            {
                var pipelineManager = GetPilelineManager();
                var blueprintid = GetBlueprintId();
                pipelineManager.blueprintId = blueprintid;
            }

            return true;
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    var pipelineManager = GetPilelineManager();
                    pipelineManager.blueprintId = string.Empty;
                    EditorUtility.SetDirty(pipelineManager);
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                }
            };
        }

        private static PipelineManager GetPilelineManager()
        {
            return SceneManager.GetActiveScene()
                .GetRootGameObjects()
                .SelectMany(rootGameObject => rootGameObject.GetComponentsInChildren<VRC_SceneDescriptor>())
                .Single()
                .GetComponent<PipelineManager>();
        }

        private static bool ExistBlueprintIdFile()
            => File.Exists(BlueprintIdFileCreator.BLUEPRINT_ID_FILE_PATH);

        private static string GetBlueprintId()
        {
            return File.ReadAllText(BlueprintIdFileCreator.BLUEPRINT_ID_FILE_PATH);
        }
    }
}
