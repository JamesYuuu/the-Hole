using System;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.XR.Interaction.Toolkit.Samples.DeviceSimulator
{
    class XRDeviceSimulatorUI : MonoBehaviour
    {
        XRDeviceSimulator _mSimulator;

        const string KMouseDeviceType = "Mouse";
        const string KTranslateLookText = "Move";
        const string KRotateLookText = "Look";

#if UNITY_EDITOR
        const string KMenuOpenStateKey = "XRI." + nameof(XRDeviceSimulatorUI) + ".MenuOpenState";
#endif
        [FormerlySerializedAs("m_IsMenuOpen")]
        [SerializeField]
        [HideInInspector]
        bool mIsMenuOpen = true;

        bool IsMenuOpen
        {
            get
            {
#if UNITY_EDITOR
                mIsMenuOpen = EditorPrefs.GetBool(KMenuOpenStateKey, mIsMenuOpen);
#endif
                return mIsMenuOpen;
            }

            set
            {
                mIsMenuOpen = value;
#if UNITY_EDITOR
                EditorPrefs.SetBool(KMenuOpenStateKey, mIsMenuOpen);
#endif
            }
        }

        [FormerlySerializedAs("m_XRDeviceSimulatorMainPanel")]
        [Header("Open/Close Panels")]

        [SerializeField]
        GameObject mXRDeviceSimulatorMainPanel;
        [FormerlySerializedAs("m_XRDeviceSimulatorCollapsedPanel")] [SerializeField]
        GameObject mXRDeviceSimulatorCollapsedPanel;

        [FormerlySerializedAs("m_HmdSpriteDark")]
        [Header("Sprites")]

        [SerializeField]
        Sprite mHmdSpriteDark;
        [FormerlySerializedAs("m_HmdSpriteLight")] [SerializeField]
        Sprite mHmdSpriteLight;

        [FormerlySerializedAs("m_KeyboardSprite")] [SerializeField]
        Sprite mKeyboardSprite;
        internal Sprite KeyboardSprite => mKeyboardSprite;

        [FormerlySerializedAs("m_MouseSprite")] [SerializeField]
        Sprite mMouseSprite;
        internal Sprite MouseSprite => mMouseSprite;

        [FormerlySerializedAs("m_RMouseSpriteDark")] [SerializeField]
        Sprite mRMouseSpriteDark;
        internal Sprite RMouseSpriteDark => mRMouseSpriteDark;

        [FormerlySerializedAs("m_RMouseSpriteLight")] [SerializeField]
        Sprite mRMouseSpriteLight;
        internal Sprite RMouseSpriteLight => mRMouseSpriteLight;

        [FormerlySerializedAs("m_RMouseSprite")]
        [HideInInspector]
        [SerializeField]
        Sprite mRMouseSprite;
        internal Sprite RMouseSprite
        {
            get
            {
#if !UNITY_EDITOR
                if (m_RMouseSprite == null)
                    m_RMouseSprite = m_RMouseSpriteDark;
#endif
                return mRMouseSprite;
            }
        }

        [FormerlySerializedAs("m_CycleDevicesText")]
        [Header("General")]

        [SerializeField]
        Text mCycleDevicesText;

        [FormerlySerializedAs("m_CurrentSelectedDeviceText")] [SerializeField]
        Text mCurrentSelectedDeviceText;

        [FormerlySerializedAs("m_HeadsetImage")]
        [Header("Headset Device")]

        [SerializeField]
        Image mHeadsetImage;

        [FormerlySerializedAs("m_HeadsetMoveButton")]
        [Space]

        [SerializeField]
        Image mHeadsetMoveButton;

        [FormerlySerializedAs("m_HeadsetMoveButtonIcon")] [SerializeField]
        Image mHeadsetMoveButtonIcon;

        [FormerlySerializedAs("m_HeadsetMoveButtonText")] [SerializeField]
        Text mHeadsetMoveButtonText;

        [FormerlySerializedAs("m_HeadsetMoveValueIcon")] [SerializeField]
        Image mHeadsetMoveValueIcon;

        [FormerlySerializedAs("m_HeadsetMoveValueText")] [SerializeField]
        Text mHeadsetMoveValueText;

        [FormerlySerializedAs("m_HeadsetLookButton")]
        [Space]

        [SerializeField]
        Image mHeadsetLookButton;

        [FormerlySerializedAs("m_HeadsetLookButtonText")] [SerializeField]
        Text mHeadsetLookButtonText;

        [FormerlySerializedAs("m_HeadsetLookValueIcon")] [SerializeField]
        Image mHeadsetLookValueIcon;

        [FormerlySerializedAs("m_HeadsetLookValueText")] [SerializeField]
        Text mHeadsetLookValueText;

        [FormerlySerializedAs("m_CursorLockButton")]
        [Space]

        [SerializeField]
        [FormerlySerializedAs("m_ShowCursorButton")]
        Image mCursorLockButton;

        [FormerlySerializedAs("m_CursorLockValueText")]
        [SerializeField]
        [FormerlySerializedAs("m_ShowCursorValueText")]
        Text mCursorLockValueText;

        [FormerlySerializedAs("m_MouseModeButtonText")]
        [Space]

        [SerializeField]
        Text mMouseModeButtonText;

        [FormerlySerializedAs("m_MouseModeValueText")] [SerializeField]
        Text mMouseModeValueText;

        [FormerlySerializedAs("m_HeadsetSelectedButton")]
        [Space]

        [SerializeField]
        Image mHeadsetSelectedButton;

        [FormerlySerializedAs("m_HeadsetSelectedValueText")] [SerializeField]
        Text mHeadsetSelectedValueText;

        [FormerlySerializedAs("m_ControllerSelectedButton")]
        [Header("Controllers")]

        [SerializeField]
        Image mControllerSelectedButton;

        [FormerlySerializedAs("m_ControllersSelectedValueText")] [SerializeField]
        Text mControllersSelectedValueText;

        [FormerlySerializedAs("m_LeftController")]
        [Header("Left Controller")]

        [SerializeField]
        XRDeviceSimulatorControllerUI mLeftController;

        [FormerlySerializedAs("m_LeftControllerButtonText")] [SerializeField]
        Text mLeftControllerButtonText;

        [FormerlySerializedAs("m_RightController")]
        [Header("Right Controller")]

        [SerializeField]
        XRDeviceSimulatorControllerUI mRightController;

        [FormerlySerializedAs("m_RightControllerButtonText")] [SerializeField]
        Text mRightControllerButtonText;

        static readonly Color KEnabledColorDark = new Color(0xC4 / 255f, 0xC4 / 255f, 0xC4 / 255f);
        static readonly Color KEnabledColorLight = new Color(0x55/255f, 0x55/255f, 0x55/255f);
        [FormerlySerializedAs("m_EnabledColor")]
        [HideInInspector]
        [SerializeField]
        Color mEnabledColor = Color.clear;
        internal Color EnabledColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_EnabledColor == Color.clear)
                    m_EnabledColor = k_EnabledColorDark;
#endif
                return mEnabledColor;
            }
        }

        static readonly Color KDisabledColorDark = new Color(0xC4 / 255f, 0xC4 / 255f, 0xC4 / 255f, 0.5f);
        static readonly Color KDisabledColorLight = new Color(0x55/255f, 0x55/255f, 0x55/255f, 0.5f);
        [FormerlySerializedAs("m_DisabledColor")]
        [HideInInspector]
        [SerializeField]
        Color mDisabledColor = Color.clear;
        internal Color DisabledColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_DisabledColor == Color.clear)
                    m_DisabledColor = k_DisabledColorDark;
#endif
                return mDisabledColor;
            }
        }

        static readonly Color KButtonColorDark = new Color(0x55 / 255f, 0x55 / 255f, 0x55 / 255f);
        static readonly Color KButtonColorLight = new Color(0xE4/255f, 0xE4/255f, 0xE4/255f);
        [FormerlySerializedAs("m_ButtonColor")]
        [HideInInspector]
        [SerializeField]
        Color mButtonColor = Color.clear;
        internal Color ButtonColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_ButtonColor == Color.clear)
                    m_ButtonColor = k_ButtonColorDark;
#endif
                return mButtonColor;
            }
        }

        static readonly Color KDisabledButtonColorDark = new Color(0x55 / 255f, 0x55 / 255f, 0x55 / 255f, 0.5f);
        static readonly Color KDisabledButtonColorLight = new Color(0xE4 / 255f, 0xE4 / 255f, 0xE4 / 255f, 0.5f);
        [FormerlySerializedAs("m_DisabledButtonColor")]
        [HideInInspector]
        [SerializeField]
        Color mDisabledButtonColor = Color.clear;
        internal Color DisabledButtonColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_DisabledButtonColor == Color.clear)
                    m_DisabledButtonColor = k_DisabledButtonColorDark;
#endif
                return mDisabledButtonColor;
            }
        }

        static readonly Color KSelectedColorDark = new Color(0x4F / 255f, 0x65 / 255f, 0x7F / 255f);
        static readonly Color KSelectedColorLight = new Color(0x96/255f, 0xC3/255f, 0xFB/255f);
        [FormerlySerializedAs("m_SelectedColor")]
        [HideInInspector]
        [SerializeField]
        Color mSelectedColor = Color.clear;
        internal Color SelectedColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_SelectedColor == Color.clear)
                    m_SelectedColor = k_SelectedColorDark;
#endif
                return mSelectedColor;
            }
        }

        static readonly Color KBackgroundColorDark = Color.black;
        static readonly Color KBackgroundColorLight = new Color(0xB6/255f, 0xB6/255f, 0xB6/255f);
        [FormerlySerializedAs("m_BackgroundColor")]
        [HideInInspector]
        [SerializeField]
        Color mBackgroundColor = Color.clear;
        internal Color BackgroundColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_BackgroundColor == Color.clear)
                    m_BackgroundColor = k_BackgroundColorDark;
#endif
                return mBackgroundColor;
            }
        }

        static readonly Color KDeviceColorDark = new Color(0x6E / 255f, 0x6E / 255f, 0x6E / 255f);
        static readonly Color KDeviceColorLight = new Color(0xE4 / 255f, 0xE4 / 255f, 0xE4 / 255f);
        [FormerlySerializedAs("m_DeviceColor")]
        [HideInInspector]
        [SerializeField]
        Color mDeviceColor = Color.clear;
        internal Color DeviceColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_DeviceColor == Color.clear)
                    m_DeviceColor = k_DeviceColorDark;
#endif
                return mDeviceColor;
            }
        }

        static readonly Color KDisabledDeviceColorDark = new Color(0x58 / 255f, 0x58 / 255f, 0x58 / 255f);
        static readonly Color KDisabledDeviceColorLight = new Color(0xA2 / 255f, 0xA2 / 255f, 0xA2 / 255f, 0.5f);
        [FormerlySerializedAs("m_DisabledDeviceColor")]
        [HideInInspector]
        [SerializeField]
        Color mDisabledDeviceColor = Color.clear;
        internal Color DisabledDeviceColor
        {
            get
            {
#if !UNITY_EDITOR
                if (m_DisabledDeviceColor == Color.clear)
                    m_DisabledDeviceColor = k_DisabledDeviceColorDark;
#endif
                return mDisabledDeviceColor;
            }
        }


        // Handles 2 axis activation for 1 UI Button
        bool _mXAxisActivated;
        bool _mZAxisActivated;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Start()
        {
            var simulator = GetComponentInParent<XRDeviceSimulator>();
            if (simulator != null)
                Initialize(simulator);
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDestroy()
        {
            if (_mSimulator != null)
            {
                Unsubscribe(_mSimulator.manipulateLeftAction, OnManipulateLeftAction);
                Unsubscribe(_mSimulator.manipulateRightAction, OnManipulateRightAction);
                Unsubscribe(_mSimulator.toggleManipulateLeftAction, OnToggleManipulateLeftAction);
                Unsubscribe(_mSimulator.toggleManipulateRightAction, OnToggleManipulateRightAction);
                Unsubscribe(_mSimulator.toggleManipulateBodyAction, OnToggleManipulateBodyAction);
                Unsubscribe(_mSimulator.manipulateHeadAction, OnManipulateHeadAction);
                Unsubscribe(_mSimulator.cycleDevicesAction, OnCycleDevicesAction);
                Unsubscribe(_mSimulator.stopManipulationAction, OnStopManipulationAction);
                Unsubscribe(_mSimulator.toggleMouseTransformationModeAction, OnToggleMouseTransformationModeAction);
                Unsubscribe(_mSimulator.negateModeAction, OnNegateModeAction);
                Unsubscribe(_mSimulator.toggleCursorLockAction, OnToggleCursorLockAction);
                Unsubscribe(_mSimulator.keyboardXTranslateAction, OnKeyboardXTranslateAction);
                Unsubscribe(_mSimulator.keyboardYTranslateAction, OnKeyboardYTranslateAction);
                Unsubscribe(_mSimulator.keyboardZTranslateAction, OnKeyboardZTranslateAction);
                Unsubscribe(_mSimulator.restingHandAxis2DAction, OnRestingHandAxis2DAction);
                Unsubscribe(_mSimulator.gripAction, OnGripAction);
                Unsubscribe(_mSimulator.triggerAction, OnTriggerAction);
                Unsubscribe(_mSimulator.menuAction, OnMenuAction);
                Unsubscribe(_mSimulator.primaryButtonAction, OnPrimaryButtonAction);
                Unsubscribe(_mSimulator.secondaryButtonAction, OnSecondaryButtonAction);
            }
        }

        void Initialize(XRDeviceSimulator simulator)
        {
            _mSimulator = simulator;
            InitColorTheme();
            Initialize();
            // Start with the headset enabled
            OnSetMouseMode();
            OnActivateHeadsetDevice();
        }

        void InitColorTheme()
        {
#if UNITY_EDITOR
            var isEditorPro = EditorGUIUtility.isProSkin;
            mEnabledColor = isEditorPro ? KEnabledColorDark : KEnabledColorLight;
            mDisabledColor = isEditorPro ? KDisabledColorDark : KDisabledColorLight;
            mButtonColor = isEditorPro ? KButtonColorDark : KButtonColorLight;
            mDisabledButtonColor = isEditorPro ? KDisabledButtonColorDark : KDisabledButtonColorLight;
            mSelectedColor = isEditorPro ? KSelectedColorDark : KSelectedColorLight;
            mBackgroundColor = isEditorPro ? KBackgroundColorDark : KBackgroundColorLight;
            mDeviceColor = isEditorPro ? KDeviceColorDark : KDeviceColorLight;
            mDisabledDeviceColor = isEditorPro ? KDisabledDeviceColorDark : KDisabledDeviceColorLight;
            mHeadsetImage.sprite = isEditorPro ? mHmdSpriteDark : mHmdSpriteLight;
            mRMouseSprite = isEditorPro ? mRMouseSpriteDark : mRMouseSpriteLight;
#endif
        }

        void Initialize()
        {
            var bckgrdAlpha = mXRDeviceSimulatorMainPanel.GetComponent<Image>().color.a;

            foreach (var image in GetComponentsInChildren<Image>(true))
                image.color = image.sprite == null ? ButtonColor : EnabledColor;

            foreach (var text in GetComponentsInChildren<Text>(true))
                text.color = EnabledColor;

            mHeadsetImage.color = Color.white;

            var bckgrdColor = BackgroundColor;
            bckgrdColor.a = bckgrdAlpha;
            mXRDeviceSimulatorMainPanel.GetComponent<Image>().color = bckgrdColor;
            mXRDeviceSimulatorCollapsedPanel.GetComponent<Image>().color = bckgrdColor;

            mCycleDevicesText.text = _mSimulator.cycleDevicesAction.action.controls[0].displayName;

            // Headset
            var toggleManipulateBodyActionControl = _mSimulator.toggleManipulateBodyAction.action.controls[0];
            mHeadsetSelectedValueText.text = $"{toggleManipulateBodyActionControl.displayName}";

            var ctrlsBinding1 = _mSimulator.axis2DAction.action.controls;
            var ctrlsBinding2 = _mSimulator.keyboardYTranslateAction.action.controls;
            mHeadsetMoveValueText.text = $"{ctrlsBinding1[0].displayName},{ctrlsBinding1[1].displayName},{ctrlsBinding1[2].displayName},{ctrlsBinding1[3].displayName} + " +
                $"{ctrlsBinding2[0].displayName},{ctrlsBinding2[1].displayName}";

            mCursorLockValueText.text = _mSimulator.toggleCursorLockAction.action.controls[0].displayName;
            mCursorLockButton.color = Cursor.lockState == CursorLockMode.Locked ? SelectedColor : ButtonColor;

            mHeadsetLookButtonText.text = _mSimulator.mouseTransformationMode == XRDeviceSimulator.TransformationMode.Translate ? KTranslateLookText : KRotateLookText;
            mMouseModeValueText.text = _mSimulator.toggleMouseTransformationModeAction.action.controls[0].displayName;

            var manipulateHeadActionControl = _mSimulator.manipulateHeadAction.action.controls[0];
            mHeadsetLookValueIcon.sprite = GetInputIcon(manipulateHeadActionControl);
            if (manipulateHeadActionControl.name.Equals("leftButton") ||  manipulateHeadActionControl.name.Equals("rightButton"))
            {
                mHeadsetLookValueIcon.color = Color.white;

                // If the binding is using the left button, mirror the MouseR image
                if (manipulateHeadActionControl.name.Equals("leftButton"))
                    mHeadsetLookValueIcon.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            mHeadsetLookValueText.text = manipulateHeadActionControl.device.name == KMouseDeviceType ? KMouseDeviceType : manipulateHeadActionControl.displayName;

            mLeftController.Initialize(_mSimulator);
            mLeftControllerButtonText.text = $"{_mSimulator.toggleManipulateLeftAction.action.controls[0].displayName} / {_mSimulator.manipulateLeftAction.action.controls[0].displayName} [Hold]";
            mRightController.Initialize(_mSimulator);
            mRightControllerButtonText.text = $"{_mSimulator.toggleManipulateRightAction.action.controls[0].displayName} / {_mSimulator.manipulateRightAction.action.controls[0].displayName} [Hold]";

            mControllersSelectedValueText.text =
                $"{_mSimulator.toggleManipulateLeftAction.action.controls[0].displayName}, {_mSimulator.toggleManipulateRightAction.action.controls[0].displayName} [Toggle]";

            mHeadsetMoveButtonIcon.color = EnabledColor;

            // Update OnDestroy with corresponding Unsubscribe call when adding here
            Subscribe(_mSimulator.manipulateLeftAction, OnManipulateLeftAction);
            Subscribe(_mSimulator.manipulateRightAction, OnManipulateRightAction);
            Subscribe(_mSimulator.toggleManipulateLeftAction, OnToggleManipulateLeftAction);
            Subscribe(_mSimulator.toggleManipulateRightAction, OnToggleManipulateRightAction);
            Subscribe(_mSimulator.toggleManipulateBodyAction, OnToggleManipulateBodyAction);
            Subscribe(_mSimulator.manipulateHeadAction, OnManipulateHeadAction);
            Subscribe(_mSimulator.cycleDevicesAction, OnCycleDevicesAction);
            Subscribe(_mSimulator.stopManipulationAction, OnStopManipulationAction);
            Subscribe(_mSimulator.toggleMouseTransformationModeAction, OnToggleMouseTransformationModeAction);
            Subscribe(_mSimulator.negateModeAction, OnNegateModeAction);
            Subscribe(_mSimulator.toggleCursorLockAction, OnToggleCursorLockAction);
            Subscribe(_mSimulator.keyboardXTranslateAction, OnKeyboardXTranslateAction);
            Subscribe(_mSimulator.keyboardYTranslateAction, OnKeyboardYTranslateAction);
            Subscribe(_mSimulator.keyboardZTranslateAction, OnKeyboardZTranslateAction);
            Subscribe(_mSimulator.restingHandAxis2DAction, OnRestingHandAxis2DAction);
            Subscribe(_mSimulator.gripAction, OnGripAction);
            Subscribe(_mSimulator.triggerAction, OnTriggerAction);
            Subscribe(_mSimulator.menuAction, OnMenuAction);
            Subscribe(_mSimulator.primaryButtonAction, OnPrimaryButtonAction);
            Subscribe(_mSimulator.secondaryButtonAction, OnSecondaryButtonAction);

            mXRDeviceSimulatorMainPanel.SetActive(IsMenuOpen);
            mXRDeviceSimulatorCollapsedPanel.SetActive(!IsMenuOpen);
        }

        internal Sprite GetInputIcon(InputControl control)
        {
            if (control == null)
                return null;

            var icon = KeyboardSprite;
            if (control.device.name == KMouseDeviceType)
            {
                switch (control.name)
                {
                    case "leftButton":
                    case "rightButton":
                        icon = RMouseSprite;
                        break;
                    default:
                        icon = MouseSprite;
                        break;
                }
            }

            return icon;
        }

        /// <summary>
        /// Hides the simulator UI panel when called while displaying the simulator button.
        /// </summary>
        public void OnClickCloseSimulatorUIPanel()
        {
            IsMenuOpen = false;
            mXRDeviceSimulatorMainPanel.SetActive(false);
            mXRDeviceSimulatorCollapsedPanel.SetActive(true);
        }

        /// <summary>
        /// Displays the simulator UI panel when called while hiding the simulator button.
        /// </summary>
        public void OnClickOpenSimulatorUIPanel()
        {
            IsMenuOpen = true;
            mXRDeviceSimulatorMainPanel.SetActive(true);
            mXRDeviceSimulatorCollapsedPanel.SetActive(false);
        }

        /// <summary>
        /// Sets the Left Controller device as active to receive input.
        /// </summary>
        void OnActivateLeftController()
        {
            mCurrentSelectedDeviceText.text = "Left Controller";
            OnActivateController(mLeftController);
        }

        /// <summary>
        /// Sets the Right Controller device as active to receive input.
        /// </summary>
        void OnActivateRightController()
        {
            mCurrentSelectedDeviceText.text = "Right Controller";
            OnActivateController(mRightController);
        }

        void OnActivateController(XRDeviceSimulatorControllerUI controller)
        {
            PushCurrentButtonState(controller);
            controller.SetAsActiveController(true, _mSimulator);
            var other = controller == mLeftController ? mRightController : mLeftController;
            other.SetAsActiveController(false, _mSimulator, true);

            mHeadsetImage.gameObject.SetActive(false);

            HeadDeviceSetActive(false);
            mControllerSelectedButton.color = SelectedColor;
        }

        /// <summary>
        /// Sets both Left & Right Controller devices as active to receive input.
        /// </summary>
        void OnActivateBothControllers()
        {
            mCurrentSelectedDeviceText.text = "Left & Right Controllers";
            PushCurrentButtonState(mLeftController);
            PushCurrentButtonState(mRightController);
            mLeftController.SetAsActiveController(true, _mSimulator);
            mRightController.SetAsActiveController(true, _mSimulator);

            mHeadsetImage.gameObject.SetActive(false);

            HeadDeviceSetActive(false);
            mControllerSelectedButton.color = SelectedColor;
        }

        void PushCurrentButtonState(XRDeviceSimulatorControllerUI controller)
        {
            controller.OnGrip(_mSimulator.gripAction.action.inProgress);
            controller.OnTrigger(_mSimulator.triggerAction.action.inProgress);
            controller.OnMenu(_mSimulator.menuAction.action.inProgress);
            controller.OnPrimaryButton(_mSimulator.primaryButtonAction.action.inProgress);
            controller.OnSecondaryButton(_mSimulator.secondaryButtonAction.action.inProgress);
            controller.OnXAxisTranslatePerformed(_mSimulator.keyboardXTranslateAction.action.inProgress);
            controller.OnZAxisTranslatePerformed(_mSimulator.keyboardZTranslateAction.action.inProgress);
        }

        /// <summary>
        /// Sets the headset device as active to receive input.
        /// </summary>
        void OnActivateHeadsetDevice(bool activated = true)
        {
            mLeftController.SetAsActiveController(false, _mSimulator);
            mRightController.SetAsActiveController(false, _mSimulator);

            mCurrentSelectedDeviceText.text = activated ? "Head Mounted Display (HMD)" : "None";
            mHeadsetImage.gameObject.SetActive(activated);

            HeadDeviceSetActive(activated);
            mControllerSelectedButton.color = ButtonColor;
        }

        /// <summary>
        /// Updates all the UI associated the the Headset.
        /// </summary>
        /// <param name="active">Whether the headset is the active device or not.</param>
        void HeadDeviceSetActive(bool active)
        {
            mHeadsetSelectedButton.color = active ? SelectedColor : ButtonColor;

            var currentColor = active ? EnabledColor : DisabledColor;
            mHeadsetMoveButtonIcon.color = currentColor;
            mHeadsetMoveButtonText.color = currentColor;
            mHeadsetMoveValueIcon.color = currentColor;
            mHeadsetMoveValueText.color = currentColor;

            mHeadsetMoveButton.color = active ? ButtonColor : DisabledButtonColor;
        }

        static void Subscribe(InputActionReference reference, Action<InputAction.CallbackContext> performedOrCanceled)
        {
            var action = reference != null ? reference.action : null;
            if (action != null && performedOrCanceled != null)
            {
                action.performed += performedOrCanceled;
                action.canceled += performedOrCanceled;
            }
        }

        static void Unsubscribe(InputActionReference reference, Action<InputAction.CallbackContext> performedOrCanceled)
        {
            var action = reference != null ? reference.action : null;
            if (action != null && performedOrCanceled != null)
            {
                action.performed -= performedOrCanceled;
                action.canceled -= performedOrCanceled;
            }
        }

        void OnManipulateLeftAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
            {
                if (_mSimulator.manipulatingLeftController && _mSimulator.manipulatingRightController)
                    OnActivateBothControllers();
                else if (_mSimulator.manipulatingLeftController)
                    OnActivateLeftController();
            }
            else
            {
                if (_mSimulator.manipulatingRightController)
                    OnActivateRightController();
                else
                    OnActivateHeadsetDevice(_mSimulator.manipulatingFPS);
            }
        }

        void OnManipulateRightAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
            {
                if (_mSimulator.manipulatingLeftController && _mSimulator.manipulatingRightController)
                    OnActivateBothControllers();
                else if (_mSimulator.manipulatingRightController)
                    OnActivateRightController();
            }
            else
            {
                if (_mSimulator.manipulatingLeftController)
                    OnActivateLeftController();
                else
                    OnActivateHeadsetDevice(_mSimulator.manipulatingFPS);
            }
        }

        void OnToggleManipulateLeftAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
            {
                if (_mSimulator.manipulatingLeftController)
                    OnActivateLeftController();
                else
                    OnActivateHeadsetDevice();
            }
        }

        void OnToggleManipulateRightAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
            {
                if (_mSimulator.manipulatingRightController)
                    OnActivateRightController();
                else
                    OnActivateHeadsetDevice();
            }
        }

        void OnToggleManipulateBodyAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
            {
                OnActivateHeadsetDevice();
            }
        }

        void OnManipulateHeadAction(InputAction.CallbackContext context)
        {
            var isInProgress = context.phase.IsInProgress();
            var noControllers = !_mSimulator.manipulatingLeftController && !_mSimulator.manipulatingRightController;
            if (isInProgress)
            {
                if (_mSimulator.manipulatingFPS || noControllers)
                    OnActivateHeadsetDevice();
            }
            else if (noControllers)
            {
                OnActivateHeadsetDevice(_mSimulator.manipulatingFPS);
            }

            mHeadsetImage.gameObject.SetActive(isInProgress || noControllers);
            mHeadsetLookButton.color = isInProgress ? SelectedColor : ButtonColor;
        }

        void OnCycleDevicesAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
            {
                if (_mSimulator.manipulatingFPS)
                    OnActivateHeadsetDevice();
                if (_mSimulator.manipulatingLeftController)
                    OnActivateLeftController();
                if (_mSimulator.manipulatingRightController)
                    OnActivateRightController();
            }
        }

        void OnStopManipulationAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
                OnActivateHeadsetDevice(_mSimulator.manipulatingFPS);
        }

        void OnToggleMouseTransformationModeAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
                OnSetMouseMode();
        }

        void OnNegateModeAction(InputAction.CallbackContext context)
        {
            OnSetMouseMode();
        }

        void OnToggleCursorLockAction(InputAction.CallbackContext context)
        {
            if (context.phase.IsInProgress())
                OnCursorLockChanged();
        }

        void OnKeyboardXTranslateAction(InputAction.CallbackContext context)
        {
            OnXAxisTranslatePerformed(context.phase.IsInProgress(), false);
        }

        void OnKeyboardYTranslateAction(InputAction.CallbackContext context)
        {
            OnYAxisTranslatePerformed(context.phase.IsInProgress());
        }

        void OnKeyboardZTranslateAction(InputAction.CallbackContext context)
        {
            OnZAxisTranslatePerformed(context.phase.IsInProgress(), false);
        }

        void OnRestingHandAxis2DAction(InputAction.CallbackContext context)
        {
            var restingHandAxis2DInput = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1f);
            if (context.phase.IsInProgress())
            {
                if (restingHandAxis2DInput.x != 0f)
                    OnXAxisTranslatePerformed(true, true);
                if (restingHandAxis2DInput.y != 0f)
                    OnZAxisTranslatePerformed(true, true);
            }
            else
            {
                if (restingHandAxis2DInput.x == 0f)
                    OnXAxisTranslatePerformed(false, true);
                if (restingHandAxis2DInput.y == 0f)
                    OnZAxisTranslatePerformed(false, true);
            }
        }

        void OnGripAction(InputAction.CallbackContext context)
        {
            OnGripPerformed(context.phase.IsInProgress());
        }

        void OnTriggerAction(InputAction.CallbackContext context)
        {
            OnTriggerPerformed(context.phase.IsInProgress());
        }

        void OnMenuAction(InputAction.CallbackContext context)
        {
            OnMenuPerformed(context.phase.IsInProgress());
        }

        void OnPrimaryButtonAction(InputAction.CallbackContext context)
        {
            OnPrimaryButtonPerformed(context.phase.IsInProgress());
        }

        void OnSecondaryButtonAction(InputAction.CallbackContext context)
        {
            OnSecondaryButtonPerformed(context.phase.IsInProgress());
        }

        void OnSetMouseMode()
        {
            // Translate/Rotate
            mMouseModeButtonText.text = _mSimulator.negateMode
                ? XRDeviceSimulator.Negate(_mSimulator.mouseTransformationMode).ToString()
                : _mSimulator.mouseTransformationMode.ToString();
            // Move/Look
            mHeadsetLookButtonText.text =
                _mSimulator.mouseTransformationMode == XRDeviceSimulator.TransformationMode.Translate
                    ? KTranslateLookText
                    : KRotateLookText;
        }

        void OnCursorLockChanged()
        {
            mCursorLockButton.color = Cursor.lockState == CursorLockMode.Locked ? SelectedColor : ButtonColor;
        }

        // x-axis [A-D]
        void OnXAxisTranslatePerformed(bool activated, bool restingHand)
        {
            var active = activated;
            if (!restingHand)
            {
                _mXAxisActivated = activated;
                active |= _mZAxisActivated;
            }

            if (_mSimulator.manipulatingLeftController)
            {
                var lController = restingHand ? mRightController : mLeftController;
                lController.OnXAxisTranslatePerformed(active);
            }

            if (_mSimulator.manipulatingRightController)
            {
                var rController = restingHand ? mLeftController : mRightController;
                rController.OnXAxisTranslatePerformed(active);
            }

            if (_mSimulator.manipulatingFPS)
                mHeadsetMoveButton.color = active ? SelectedColor : ButtonColor;
        }

        // y-axis [Q-E]
        void OnYAxisTranslatePerformed(bool activated)
        {
            if (_mSimulator.manipulatingFPS)
                mHeadsetMoveButton.color = activated ? SelectedColor : ButtonColor;
        }

        // z-axis [W-S]
        void OnZAxisTranslatePerformed(bool activated, bool restingHand)
        {
            var active = activated;
            if (!restingHand)
            {
                _mZAxisActivated = activated;
                active |= _mXAxisActivated;
            }

            if (_mSimulator.manipulatingLeftController)
            {
                var lController = restingHand ? mRightController : mLeftController;
                lController.OnZAxisTranslatePerformed(active);
            }

            if (_mSimulator.manipulatingRightController)
            {
                var rController = restingHand ? mLeftController : mRightController;
                rController.OnZAxisTranslatePerformed(active);
            }

            if (_mSimulator.manipulatingFPS)
                mHeadsetMoveButton.color = active ? SelectedColor : ButtonColor;
        }

        void OnMenuPerformed(bool activated)
        {
            if (_mSimulator.manipulatingLeftController)
                mLeftController.OnMenu(activated);

            if (_mSimulator.manipulatingRightController)
                mRightController.OnMenu(activated);
        }

        void OnGripPerformed(bool activated)
        {
            if (_mSimulator.manipulatingLeftController)
                mLeftController.OnGrip(activated);

            if (_mSimulator.manipulatingRightController)
                mRightController.OnGrip(activated);
        }

        void OnTriggerPerformed(bool activated)
        {
            if (_mSimulator.manipulatingLeftController)
                mLeftController.OnTrigger(activated);

            if (_mSimulator.manipulatingRightController)
                mRightController.OnTrigger(activated);
        }

        void OnPrimaryButtonPerformed(bool activated)
        {
            if (_mSimulator.manipulatingLeftController)
                mLeftController.OnPrimaryButton(activated);

            if (_mSimulator.manipulatingRightController)
                mRightController.OnPrimaryButton(activated);
        }

        void OnSecondaryButtonPerformed(bool activated)
        {
            if (_mSimulator.manipulatingLeftController)
                mLeftController.OnSecondaryButton(activated);

            if (_mSimulator.manipulatingRightController)
                mRightController.OnSecondaryButton(activated);
        }
    }
}
