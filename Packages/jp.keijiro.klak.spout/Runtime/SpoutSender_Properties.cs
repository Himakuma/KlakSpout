using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Klak.Spout
{
// Sender capture methods
    public enum CaptureMethod
    {
        GameView,
        Camera,
        Texture
    }

//
// Spout sender class (properties)
//
    partial class SpoutSender
    {
        #region Spout source

        [SerializeField] private string _spoutName = "Spout Sender";
        [SerializeField] private GraphicsFormat _graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;

        public GraphicsFormat senderGraphicsFormat
        {
            get => _graphicsFormat;
            set => _graphicsFormat = value;
        }

        public string spoutName
        {
            get => _spoutName;
            set => ChangeSpoutName(value);
        }

        private void ChangeSpoutName(string name)
        {
            // Sender refresh on renaming
            if (_spoutName == name)
            {
                return;
            }

            _spoutName = name;
            ReleaseSender();
        }

        #endregion

        #region Format option

        [SerializeField] private bool _keepAlpha;

        public bool keepAlpha
        {
            get => _keepAlpha;
            set => _keepAlpha = value;
        }

        #endregion

        #region Capture target

        [SerializeField] private CaptureMethod _captureMethod = CaptureMethod.GameView;

        public CaptureMethod captureMethod
        {
            get => _captureMethod;
            set => _captureMethod = value;
        }

        [SerializeField] private Camera _sourceCamera;

        public Camera sourceCamera
        {
            get => _sourceCamera;
            set => _sourceCamera = value;
        }

        [SerializeField] private Texture _sourceTexture;

        public Texture sourceTexture
        {
            get => _sourceTexture;
            set => _sourceTexture = value;
        }

        #endregion

        #region Resource asset reference

        [SerializeField] [HideInInspector] private SpoutResources _resources;

        public void SetResources(SpoutResources resources)
        {
            _resources = resources;
        }

        #endregion
    }
} // namespace Klak.Spout