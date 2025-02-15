using System;
using UnityEngine;
using IntPtr = System.IntPtr;

namespace Klak.Spout
{
//
// Wrapper class for receiver instances on the native plugin side
//
    internal sealed class Receiver : IDisposable
    {
        #region Public property

        public Texture2D Texture { get; private set; }

        #endregion

        #region Frame update method

        public void Update()
        {
            if (_plugin == IntPtr.Zero)
            {
                return;
            }

            var data = Plugin.GetReceiverData(_plugin);

            // Texture refresh:
            // If we are referring to an old texture pointer, destroy it first.
            if (Texture != null &&
                Texture.GetNativeTexturePtr() != data.texturePointer)
            {
                Utility.Destroy(Texture);
                Texture = null;
            }

            // Lazy initialization:
            // We try creating a receiver texture every frame until getting a
            // correct one.
            if (Texture == null && data.texturePointer != IntPtr.Zero)
            {
                Texture = Texture2D.CreateExternalTexture
                ((int)data.width, (int)data.height, TextureFormat.RGBA32,
                    false, false, data.texturePointer);
            }

            // Update event for the render thread
            _event.IssuePluginEvent(EventID.UpdateReceiver);
        }

        #endregion

        #region Private objects

        private IntPtr _plugin;
        private readonly EventKicker _event;

        #endregion

        #region Object lifecycle

        public Receiver(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                return;
            }

            // Plugin object allocation
            _plugin = Plugin.CreateReceiver(sourceName);
            if (_plugin == IntPtr.Zero)
            {
                return;
            }

            // Event kicker (heap block for interop communication)
            _event = new EventKicker(new EventData(_plugin));

            // Initial update event
            _event.IssuePluginEvent(EventID.UpdateReceiver);
        }

        public void Dispose()
        {
            if (_plugin != IntPtr.Zero)
            {
                // Isssue the closer event to destroy the plugin object from the
                // render thread.
                _event.IssuePluginEvent(EventID.CloseReceiver);

                // Event kicker (interop memory) deallocation:
                // The close event above will refer to the block from the render
                // thread, so we actually can't free the memory here. To avoid this
                // problem, EventKicker uses MemoryPool to delay the memory
                // deallocation by the end of the frame.
                _event.Dispose();

                _plugin = IntPtr.Zero;
            }

            Utility.Destroy(Texture);
            Texture = null;
        }

        #endregion
    }
} // namespace Klak.Spout