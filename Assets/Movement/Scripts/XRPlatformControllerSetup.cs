using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.Management;
#else
using UnityEngine.XR.Management;
#endif

namespace Unity.Template.VR
{
    internal class XRPlatformControllerSetup : MonoBehaviour
    {
        [FormerlySerializedAs("m_LeftController")] [SerializeField]
        GameObject mLeftController;

        [FormerlySerializedAs("m_RightController")] [SerializeField]
        GameObject mRightController;
        
        [FormerlySerializedAs("m_LeftControllerOculusPackage")] [SerializeField]
        GameObject mLeftControllerOculusPackage;

        [FormerlySerializedAs("m_RightControllerOculusPackage")] [SerializeField]
        GameObject mRightControllerOculusPackage;

        void Start()
        {
#if UNITY_EDITOR
            var loaders = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone).Manager.activeLoaders;
#else
            var loaders = XRGeneralSettings.Instance.Manager.activeLoaders;
#endif
            
            foreach (var loader in loaders)
            {
                if (loader.name.Equals("Oculus Loader"))
                {
                    mRightController.SetActive(false);
                    mLeftController.SetActive(false);
                    mRightControllerOculusPackage.SetActive(true);
                    mLeftControllerOculusPackage.SetActive(true);
                }
            }
        }
    }
}
