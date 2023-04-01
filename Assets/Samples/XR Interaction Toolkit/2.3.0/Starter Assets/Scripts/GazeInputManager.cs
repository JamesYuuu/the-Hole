using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Manages input fallback for <see cref="XRGazeInteractor"/> when eye tracking is not available.
    /// </summary>
    public class GazeInputManager : MonoBehaviour
    {
        // This is the name of the layout that is registered by EyeGazeInteraction in the OpenXR Plugin package
        const string KEyeGazeLayoutName = "EyeGaze";

        [FormerlySerializedAs("m_FallbackIfEyeTrackingUnavailable")]
        [SerializeField]
        [Tooltip("Enable fallback to head tracking if eye tracking is unavailable.")]
        bool mFallbackIfEyeTrackingUnavailable = true;

        /// <summary>
        /// Enable fallback to head tracking if eye tracking is unavailable.
        /// </summary>
        public bool FallbackIfEyeTrackingUnavailable
        {
            get => mFallbackIfEyeTrackingUnavailable;
            set => mFallbackIfEyeTrackingUnavailable = value;
        }


        bool _mEyeTrackingDeviceFound;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Awake()
        {
            // Check if we have eye tracking support
            var inputDeviceList = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, inputDeviceList);
            if (inputDeviceList.Count > 0)
            {
                Debug.Log("Eye tracking device found!", this);
                _mEyeTrackingDeviceFound = true;
                return;
            }

            foreach (var device in InputSystem.InputSystem.devices)
            {
                if (device.layout == KEyeGazeLayoutName)
                {
                    Debug.Log("Eye gaze device found!", this);
                    _mEyeTrackingDeviceFound = true;
                    return;
                }
            }

            Debug.LogWarning($"Could not find a device that supports eye tracking on Awake. {this} has subscribed to device connected events and will activate the GameObject when an eye tracking device is connected.", this);

            InputDevices.deviceConnected += OnDeviceConnected;
            InputSystem.InputSystem.onDeviceChange += OnDeviceChange;

            gameObject.SetActive(mFallbackIfEyeTrackingUnavailable);
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDestroy()
        {
            InputDevices.deviceConnected -= OnDeviceConnected;
            InputSystem.InputSystem.onDeviceChange -= OnDeviceChange;
        }

        void OnDeviceConnected(InputDevice inputDevice)
        {
            if (_mEyeTrackingDeviceFound || !inputDevice.characteristics.HasFlag(InputDeviceCharacteristics.EyeTracking))
                return;

            Debug.Log("Eye tracking device found!", this);
            _mEyeTrackingDeviceFound = true;
            gameObject.SetActive(true);
        }

        void OnDeviceChange(InputSystem.InputDevice device, InputDeviceChange change)
        {
            if (_mEyeTrackingDeviceFound || change != InputDeviceChange.Added)
                return;

            if (device.layout == KEyeGazeLayoutName)
            {
                Debug.Log("Eye gaze device found!", this);
                _mEyeTrackingDeviceFound = true;
                gameObject.SetActive(true);
            }
        }
    }
}
