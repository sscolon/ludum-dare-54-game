using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DDCore.Editor
{
    [CustomEditor(typeof(AnimationDirector))]
    public class AnimationDirectorEditor : UnityEditor.Editor
    {
        private AnimationDirector _animationDirector;
        private void OnEnable()
        {
            _animationDirector = target as AnimationDirector;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Import Sprite Sheet"))
            {
                string path = EditorUtility.OpenFilePanel("Import a Sprite Sheet", "", "png");
                string relativePath = string.Empty;
                if (path.StartsWith(Application.dataPath))
                {
                    relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                }

                if (relativePath == string.Empty)
                {
                    DebugWrapper.LogError($"Invalid Path {path}, make sure the path is inside the assets folder.");
                    return;
                }


                int animationNameStartIndex = relativePath.LastIndexOf("_") + 1;
                int animationNameEndIndex = relativePath.IndexOf(".", animationNameStartIndex);
                string animationName = relativePath.Substring(animationNameStartIndex, animationNameEndIndex - animationNameStartIndex);

                DebugWrapper.Log($"Animation Name: {animationName}");
                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(relativePath).OfType<Sprite>().ToArray();
                DebugWrapper.Log($"Animation Length: {sprites.Length}");
                DebugWrapper.Log($"Importing...");

                AnimationDirector.Animation animation = new AnimationDirector.Animation(animationName, sprites);
                if (animationName == "idle")
                {
                    animation.isLooping = true;
                }

                _animationDirector.AddAnimation(animation);
                EditorUtility.SetDirty(_animationDirector);
                AssetDatabase.SaveAssets();

                DebugWrapper.Log($"Animation Import Successful!");
            }
        }
    }
}
