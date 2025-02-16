using UnityEditor;

namespace Klak.Spout.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SpoutSender))]
    internal sealed class SpoutSenderEditor : UnityEditor.Editor
    {
        private SerializedProperty _captureMethod;
        private SerializedProperty _graphicsFormat;
        private SerializedProperty _keepAlpha;
        private SerializedProperty _sourceCamera;
        private SerializedProperty _sourceTexture;
        private SerializedProperty _spoutName;

        private void OnEnable()
        {
            var finder = new PropertyFinder(serializedObject);
            _spoutName = finder["_spoutName"];
            _keepAlpha = finder["_keepAlpha"];
            _captureMethod = finder["_captureMethod"];
            _sourceCamera = finder["_sourceCamera"];
            _sourceTexture = finder["_sourceTexture"];
            _graphicsFormat = finder["_graphicsFormat"];
        }

        // Sender restart request
        private void RequestRestart()
        {
            // Dirty trick: We only can restart senders by modifying the
            // spoutName property, so we modify it by an invalid name, then
            // revert it.
            foreach (SpoutSender send in targets)
            {
                send.spoutName = "";
                send.spoutName = _spoutName.stringValue;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.DelayedTextField(_spoutName, Labels.SpoutName);
            var restart = EditorGUI.EndChangeCheck();

            EditorGUILayout.PropertyField(_graphicsFormat);
            EditorGUILayout.PropertyField(_keepAlpha);
            EditorGUILayout.PropertyField(_captureMethod);

            EditorGUI.indentLevel++;

            if (_captureMethod.hasMultipleDifferentValues ||
                _captureMethod.enumValueIndex == (int)CaptureMethod.Camera)
            {
                EditorGUILayout.PropertyField(_sourceCamera);
            }

            if (_captureMethod.hasMultipleDifferentValues ||
                _captureMethod.enumValueIndex == (int)CaptureMethod.Texture)
            {
                EditorGUILayout.PropertyField(_sourceTexture);
            }

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();

            if (restart)
            {
                RequestRestart();
            }
        }

        private static class Labels
        {
            public static readonly Label SpoutName = "Spout Name";
        }
    }
} // namespace Klak.Spout.Editor