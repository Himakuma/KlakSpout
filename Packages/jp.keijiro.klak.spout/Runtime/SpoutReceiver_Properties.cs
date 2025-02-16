using UnityEngine;

namespace Klak.Spout
{
//
// Spout receiver class (properties)
//
    partial class SpoutReceiver
    {
        #region Runtime property

        public RenderTexture receivedTexture
            => _buffer != null ? _buffer : _targetTexture;

        #endregion

        #region Spout source

        [SerializeField] private string _sourceName;

        public string sourceName
        {
            get => _sourceName;
            set => ChangeSourceName(value);
        }

        private void ChangeSourceName(string name)
        {
            // Receiver refresh on source changes
            if (_sourceName == name)
            {
                return;
            }

            _sourceName = name;
            Release();
        }

        #endregion

        #region Destination settings

        [SerializeField] private RenderTexture _targetTexture;

        public RenderTexture targetTexture
        {
            get => _targetTexture;
            set => _targetTexture = value;
        }

        [SerializeField] private Renderer _targetRenderer;

        public Renderer targetRenderer
        {
            get => _targetRenderer;
            set => _targetRenderer = value;
        }

        [SerializeField] private string _targetMaterialProperty;

        public string targetMaterialProperty
        {
            get => _targetMaterialProperty;
            set => _targetMaterialProperty = value;
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