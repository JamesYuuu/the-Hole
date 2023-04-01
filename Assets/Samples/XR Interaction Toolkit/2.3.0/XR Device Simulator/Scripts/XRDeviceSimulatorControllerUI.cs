using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.DeviceSimulator
{
    [RequireComponent(typeof(XRDeviceSimulatorUI))]
    class XRDeviceSimulatorControllerUI : MonoBehaviour
    {
        [FormerlySerializedAs("m_ControllerImage")]
        [Header("General")]

        [SerializeField]
        Image mControllerImage;

        [FormerlySerializedAs("m_ControllerOverlayImage")] [SerializeField]
        Image mControllerOverlayImage;

        [FormerlySerializedAs("m_PrimaryButtonImage")]
        [Header("Primary Button")]

        [SerializeField]
        Image mPrimaryButtonImage;

        [FormerlySerializedAs("m_PrimaryButtonText")] [SerializeField]
        Text mPrimaryButtonText;

        [FormerlySerializedAs("m_PrimaryButtonIcon")] [SerializeField]
        Image mPrimaryButtonIcon;

        [FormerlySerializedAs("m_SecondaryButtonImage")]
        [Header("Secondary Button")]

        [SerializeField]
        Image mSecondaryButtonImage;

        [FormerlySerializedAs("m_SecondaryButtonText")] [SerializeField]
        Text mSecondaryButtonText;

        [FormerlySerializedAs("m_SecondaryButtonIcon")] [SerializeField]
        Image mSecondaryButtonIcon;

        [FormerlySerializedAs("m_TriggerButtonImage")]
        [Header("Trigger")]

        [SerializeField]
        Image mTriggerButtonImage;

        [FormerlySerializedAs("m_TriggerButtonText")] [SerializeField]
        Text mTriggerButtonText;

        [FormerlySerializedAs("m_TriggerButtonIcon")] [SerializeField]
        Image mTriggerButtonIcon;

        [FormerlySerializedAs("m_GripButtonImage")]
        [Header("Grip")]

        [SerializeField]
        Image mGripButtonImage;

        [FormerlySerializedAs("m_GripButtonText")] [SerializeField]
        Text mGripButtonText;

        [FormerlySerializedAs("m_GripButtonIcon")] [SerializeField]
        Image mGripButtonIcon;

        [FormerlySerializedAs("m_ThumbstickButtonImage")]
        [Header("Thumbstick")]

        [SerializeField]
        Image mThumbstickButtonImage;

        [FormerlySerializedAs("m_ThumbstickButtonText")] [SerializeField]
        Text mThumbstickButtonText;

        [FormerlySerializedAs("m_ThumbstickButtonIcon")] [SerializeField]
        Image mThumbstickButtonIcon;

        [FormerlySerializedAs("m_MenuButtonImage")]
        [Header("Menu")]

        [SerializeField]
        Image mMenuButtonImage;

        [FormerlySerializedAs("m_MenuButtonText")] [SerializeField]
        Text mMenuButtonText;

        [FormerlySerializedAs("m_MenuButtonIcon")] [SerializeField]
        Image mMenuButtonIcon;

        XRDeviceSimulatorUI _mMainUIManager;

        bool _mPrimaryButtonActivated;
        bool _mSecondaryButtonActivated;
        bool _mTriggerActivated;
        bool _mGripActivated;
        bool _mMenuActivated;
        bool _mXAxisTranslateActivated;
        bool _mYAxisTranslateActivated;

        protected void Awake()
        {
            _mMainUIManager = GetComponent<XRDeviceSimulatorUI>();
        }

        internal void Initialize(XRDeviceSimulator simulator)
        {
            mPrimaryButtonText.text = simulator.primaryButtonAction.action.controls[0].displayName;
            mSecondaryButtonText.text = simulator.secondaryButtonAction.action.controls[0].displayName;
            mGripButtonText.text = simulator.gripAction.action.controls[0].displayName;
            mTriggerButtonText.text = simulator.triggerAction.action.controls[0].displayName;
            mMenuButtonText.text = simulator.menuAction.action.controls[0].displayName;

            var disabledImgColor = _mMainUIManager.DisabledColor;
            mThumbstickButtonImage.color = disabledImgColor;
            mControllerImage.color = _mMainUIManager.DisabledDeviceColor;
            mControllerOverlayImage.color = disabledImgColor;
        }

        internal void SetAsActiveController(bool active, XRDeviceSimulator simulator, bool isRestingHand = false)
        {
            var controls = isRestingHand ?
                simulator.restingHandAxis2DAction.action.controls :
                simulator.axis2DAction.action.controls;

            mThumbstickButtonText.text = $"{controls[0].displayName}, {controls[1].displayName}, {controls[2].displayName}, {controls[3].displayName}";

            UpdateButtonVisuals(active, mPrimaryButtonIcon, mPrimaryButtonText, simulator.primaryButtonAction.action.controls[0]);
            UpdateButtonVisuals(active, mSecondaryButtonIcon, mSecondaryButtonText, simulator.secondaryButtonAction.action.controls[0]);
            UpdateButtonVisuals(active, mTriggerButtonIcon, mTriggerButtonText, simulator.triggerAction.action.controls[0]);
            UpdateButtonVisuals(active, mGripButtonIcon, mGripButtonText, simulator.gripAction.action.controls[0]);
            UpdateButtonVisuals(active, mMenuButtonIcon, mMenuButtonText, simulator.menuAction.action.controls[0]);
            UpdateButtonVisuals(active || isRestingHand, mThumbstickButtonIcon, mThumbstickButtonText, simulator.axis2DAction.action.controls[0]);

            if (active)
            {
                UpdateButtonColor(mPrimaryButtonImage, _mPrimaryButtonActivated);
                UpdateButtonColor(mSecondaryButtonImage, _mSecondaryButtonActivated);
                UpdateButtonColor(mTriggerButtonImage, _mTriggerActivated);
                UpdateButtonColor(mGripButtonImage, _mGripActivated);
                UpdateButtonColor(mMenuButtonImage, _mMenuActivated);
                UpdateButtonColor(mThumbstickButtonImage, _mXAxisTranslateActivated || _mYAxisTranslateActivated);

                mControllerImage.color = _mMainUIManager.DeviceColor;
                mControllerOverlayImage.color = _mMainUIManager.EnabledColor;
            }
            else
            {
                UpdateDisableControllerButton(_mPrimaryButtonActivated, mPrimaryButtonImage, mPrimaryButtonIcon, mPrimaryButtonText);
                UpdateDisableControllerButton(_mSecondaryButtonActivated, mSecondaryButtonImage, mSecondaryButtonIcon, mSecondaryButtonText);
                UpdateDisableControllerButton(_mTriggerActivated, mTriggerButtonImage, mTriggerButtonIcon, mTriggerButtonText);
                UpdateDisableControllerButton(_mGripActivated, mGripButtonImage, mGripButtonIcon, mGripButtonText);
                UpdateDisableControllerButton(_mMenuActivated, mMenuButtonImage, mMenuButtonIcon, mMenuButtonText);

                if (!isRestingHand)
                    UpdateDisableControllerButton(_mXAxisTranslateActivated || _mYAxisTranslateActivated, mThumbstickButtonImage, mThumbstickButtonIcon, mThumbstickButtonText);
                else
                    mThumbstickButtonImage.color = _mMainUIManager.ButtonColor;

                mControllerImage.color = _mMainUIManager.DisabledDeviceColor;
                mControllerOverlayImage.color = _mMainUIManager.DisabledColor;
            }
        }

        // This function keeps the button selected color active if the key if hold when the controller is disabled.
        // Other buttons are disabled to avoid adding extra noise.
        void UpdateDisableControllerButton(bool active, Image button, Image buttonIcon, Text buttonText)
        {
            if(active)
            {
                var tmpColor = _mMainUIManager.SelectedColor;
                tmpColor.a = 0.5f;
                button.color = tmpColor;
                buttonText.gameObject.SetActive(true);
                buttonIcon.gameObject.SetActive(true);
            }
            else
            {
                button.color = _mMainUIManager.DisabledButtonColor;
                buttonText.gameObject.SetActive(false);
                buttonIcon.gameObject.SetActive(false);
            }
        }

        void UpdateButtonVisuals(bool active, Image buttonIcon, Text buttonText, InputControl control)
        {
            buttonText.gameObject.SetActive(active);
            buttonIcon.gameObject.SetActive(active);

            var color = active ? _mMainUIManager.EnabledColor : _mMainUIManager.DisabledColor;
            buttonText.color = color;
            buttonIcon.color = color;

            buttonIcon.transform.localScale = Vector3.one;
            buttonIcon.sprite = _mMainUIManager.GetInputIcon(control);
            switch (control.name)
            {
                case "leftButton":
                    buttonText.text = "L Mouse";
                    buttonIcon.color = Color.white;
                    buttonIcon.transform.localScale = new Vector3(-1f, 1f, 1f);
                    break;
                case "rightButton":
                    buttonText.text = "R Mouse";
                    buttonIcon.color = Color.white;
                    break;
                default:
                    buttonIcon.sprite = _mMainUIManager.KeyboardSprite;
                    break;
            }
        }

        void UpdateButtonColor(Image image, bool activated)
        {
            image.color = activated ? _mMainUIManager.SelectedColor : _mMainUIManager.ButtonColor;
        }

        internal void OnPrimaryButton(bool activated)
        {
            _mPrimaryButtonActivated = activated;
            UpdateButtonColor(mPrimaryButtonImage, activated);
        }

        internal void OnSecondaryButton(bool activated)
        {
            _mSecondaryButtonActivated = activated;
            UpdateButtonColor(mSecondaryButtonImage, activated);
        }

        internal void OnTrigger(bool activated)
        {
            _mTriggerActivated = activated;
            UpdateButtonColor(mTriggerButtonImage, activated);
        }

        internal void OnGrip(bool activated)
        {
            _mGripActivated = activated;
            UpdateButtonColor(mGripButtonImage, activated);
        }

        internal void OnMenu(bool activated)
        {
            _mMenuActivated = activated;
            UpdateButtonColor(mMenuButtonImage, activated);
        }

        internal void OnXAxisTranslatePerformed(bool activated)
        {
            _mXAxisTranslateActivated = activated;
            UpdateButtonColor(mThumbstickButtonImage, activated);
        }

        internal void OnZAxisTranslatePerformed(bool activated)
        {
            _mYAxisTranslateActivated = activated;
            UpdateButtonColor(mThumbstickButtonImage, activated);
        }
    }
}
