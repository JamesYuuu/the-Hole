using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Add this component to a GameObject and call the <see cref="IncrementText"/> method
    /// in response to a Unity Event to update a text display to count up with each event.
    /// </summary>
    public class IncrementUIText : MonoBehaviour
    {
        [FormerlySerializedAs("m_Text")]
        [SerializeField]
        [Tooltip("The Text component this behavior uses to display the incremented value.")]
        Text mText;

        /// <summary>
        /// The Text component this behavior uses to display the incremented value.
        /// </summary>
        public Text Text
        {
            get => mText;
            set => mText = value;
        }

        int _mCount;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Awake()
        {
            if (mText == null)
                Debug.LogWarning("Missing required Text component reference. Use the Inspector window to assign which Text component to increment.", this);
        }

        /// <summary>
        /// Increment the string message of the Text component.
        /// </summary>
        public void IncrementText()
        {
            _mCount += 1;
            if (mText != null)
                mText.text = _mCount.ToString();
        }
    }
}
