using UnityEngine;

namespace Klak.Spout
{
//
// Spout receiver class (main implementation)
//
    [ExecuteInEditMode]
    [AddComponentMenu("Klak/Spout/Spout Receiver")]
    public sealed partial class SpoutReceiver : MonoBehaviour
    {
        #region Receiver plugin object

        private Receiver _receiver;

        private void Release()
        {
            _receiver?.Dispose();
            _receiver = null;

            Utility.Destroy(_buffer);
            _buffer = null;
        }

        #endregion

        #region Buffer texture object

        private RenderTexture _buffer;

        private RenderTexture PrepareBuffer()
        {
            // Receive-to-Texture mode:
            // Destroy the internal buffer and return the target texture.
            if (_targetTexture != null)
            {
                if (_buffer != null)
                {
                    Utility.Destroy(_buffer);
                    _buffer = null;
                }

                return _targetTexture;
            }

            var src = _receiver.Texture;

            // If the buffer exists but has wrong dimensions, destroy it first.
            if (_buffer != null &&
                (_buffer.width != src.width || _buffer.height != src.height))
            {
                Utility.Destroy(_buffer);
                _buffer = null;
            }

            // Create a buffer if it hasn't been allocated yet.
            if (_buffer == null)
            {
                _buffer = new RenderTexture(src.width, src.height, 0);
                _buffer.hideFlags = HideFlags.DontSave;
                _buffer.Create();
            }

            return _buffer;
        }

        #endregion

        #region MonoBehaviour implementation

        private void OnDisable()
        {
            Release();
        }

        private void OnDestroy()
        {
            Release();
        }

        private void Update()
        {
            // Receiver lazy initialization
            if (_receiver == null)
            {
                _receiver = new Receiver(_sourceName);
            }

            // Receiver plugin-side update
            _receiver.Update();

            // Do nothing further if no texture is ready yet.
            if (_receiver.Texture == null)
            {
                return;
            }

            // Received texture buffering
            var buffer = PrepareBuffer();
            Blitter.BlitFromSrgb(_resources, _receiver.Texture, buffer);

            // Renderer override
            if (_targetRenderer != null)
            {
                RendererOverride.SetTexture
                    (_targetRenderer, _targetMaterialProperty, buffer);
            }
        }

        #endregion
    }
} // namespace Klak.Spout