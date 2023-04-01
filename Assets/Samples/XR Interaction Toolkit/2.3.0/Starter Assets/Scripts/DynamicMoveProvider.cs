using Unity.XR.CoreUtils;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// A version of action-based continuous movement that automatically controls the frame of reference that
    /// determines the forward direction of movement based on user preference for each hand.
    /// For example, can configure to use head relative movement for the left hand and controller relative movement for the right hand.
    /// </summary>
    public class DynamicMoveProvider : ActionBasedContinuousMoveProvider
    {
        /// <summary>
        /// Defines which transform the XR Origin's movement direction is relative to.
        /// </summary>
        /// <seealso cref="DynamicMoveProvider.LeftHandMovementDirection"/>
        /// <seealso cref="DynamicMoveProvider.RightHandMovementDirection"/>
        public enum MovementDirection
        {
            /// <summary>
            /// Use the forward direction of the head (camera) as the forward direction of the XR Origin's movement.
            /// </summary>
            HeadRelative,

            /// <summary>
            /// Use the forward direction of the hand (controller) as the forward direction of the XR Origin's movement.
            /// </summary>
            HandRelative,
        }

        [FormerlySerializedAs("m_HeadTransform")]
        [Space, Header("Movement Direction")]
        [SerializeField]
        [Tooltip("Directs the XR Origin's movement when using the head-relative mode. If not set, will automatically find and use the XR Origin Camera.")]
        Transform mHeadTransform;

        /// <summary>
        /// Directs the XR Origin's movement when using the head-relative mode. If not set, will automatically find and use the XR Origin Camera.
        /// </summary>
        public Transform HeadTransform
        {
            get => mHeadTransform;
            set => mHeadTransform = value;
        }

        [FormerlySerializedAs("m_LeftControllerTransform")]
        [SerializeField]
        [Tooltip("Directs the XR Origin's movement when using the hand-relative mode with the left hand.")]
        Transform mLeftControllerTransform;

        /// <summary>
        /// Directs the XR Origin's movement when using the hand-relative mode with the left hand.
        /// </summary>
        public Transform LeftControllerTransform
        {
            get => mLeftControllerTransform;
            set => mLeftControllerTransform = value;
        }

        [FormerlySerializedAs("m_RightControllerTransform")]
        [SerializeField]
        [Tooltip("Directs the XR Origin's movement when using the hand-relative mode with the right hand.")]
        Transform mRightControllerTransform;

        public Transform RightControllerTransform
        {
            get => mRightControllerTransform;
            set => mRightControllerTransform = value;
        }

        [FormerlySerializedAs("m_LeftHandMovementDirection")]
        [SerializeField]
        [Tooltip("Whether to use the specified head transform or left controller transform to direct the XR Origin's movement for the left hand.")]
        MovementDirection mLeftHandMovementDirection;

        /// <summary>
        /// Whether to use the specified head transform or controller transform to direct the XR Origin's movement for the left hand.
        /// </summary>
        /// <seealso cref="MovementDirection"/>
        public MovementDirection LeftHandMovementDirection
        {
            get => mLeftHandMovementDirection;
            set => mLeftHandMovementDirection = value;
        }

        [FormerlySerializedAs("m_RightHandMovementDirection")]
        [SerializeField]
        [Tooltip("Whether to use the specified head transform or right controller transform to direct the XR Origin's movement for the right hand.")]
        MovementDirection mRightHandMovementDirection;

        /// <summary>
        /// Whether to use the specified head transform or controller transform to direct the XR Origin's movement for the right hand.
        /// </summary>
        /// <seealso cref="MovementDirection"/>
        public MovementDirection RightHandMovementDirection
        {
            get => mRightHandMovementDirection;
            set => mRightHandMovementDirection = value;
        }

        Transform _mCombinedTransform;
        Pose _mLeftMovementPose = Pose.identity;
        Pose _mRightMovementPose = Pose.identity;

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            _mCombinedTransform = new GameObject("[Dynamic Move Provider] Combined Forward Source").transform;
            _mCombinedTransform.SetParent(transform, false);
            _mCombinedTransform.localPosition = Vector3.zero;
            _mCombinedTransform.localRotation = Quaternion.identity;

            forwardSource = _mCombinedTransform;
        }

        /// <inheritdoc />
        protected override Vector3 ComputeDesiredMove(Vector2 input)
        {
            // Don't need to do anything if the total input is zero.
            // This is the same check as the base method.
            if (input == Vector2.zero)
                return Vector3.zero;

            // Initialize the Head Transform if necessary, getting the Camera from XR Origin
            if (mHeadTransform == null)
            {
                var xrOrigin = system.xrOrigin;
                if (xrOrigin != null)
                {
                    var xrCamera = xrOrigin.Camera;
                    if (xrCamera != null)
                        mHeadTransform = xrCamera.transform;
                }
            }

            // Get the forward source for the left hand input
            switch (mLeftHandMovementDirection)
            {
                case MovementDirection.HeadRelative:
                    if (mHeadTransform != null)
                        _mLeftMovementPose = mHeadTransform.GetWorldPose();

                    break;

                case MovementDirection.HandRelative:
                    if (mLeftControllerTransform != null)
                        _mLeftMovementPose = mLeftControllerTransform.GetWorldPose();

                    break;

                default:
                    Assert.IsTrue(false, $"Unhandled {nameof(MovementDirection)}={mLeftHandMovementDirection}");
                    break;
            }

            // Get the forward source for the right hand input
            switch (mRightHandMovementDirection)
            {
                case MovementDirection.HeadRelative:
                    if (mHeadTransform != null)
                        _mRightMovementPose = mHeadTransform.GetWorldPose();

                    break;

                case MovementDirection.HandRelative:
                    if (mRightControllerTransform != null)
                        _mRightMovementPose = mRightControllerTransform.GetWorldPose();

                    break;

                default:
                    Assert.IsTrue(false, $"Unhandled {nameof(MovementDirection)}={mRightHandMovementDirection}");
                    break;
            }

            // Combine the two poses into the forward source based on the magnitude of input
            var leftHandValue = leftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
            var rightHandValue = rightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

            var totalSqrMagnitude = leftHandValue.sqrMagnitude + rightHandValue.sqrMagnitude;
            var leftHandBlend = 0.5f;
            if (totalSqrMagnitude > Mathf.Epsilon)
                leftHandBlend = leftHandValue.sqrMagnitude / totalSqrMagnitude;

            var combinedPosition = Vector3.Lerp(_mRightMovementPose.position, _mLeftMovementPose.position, leftHandBlend);
            var combinedRotation = Quaternion.Slerp(_mRightMovementPose.rotation, _mLeftMovementPose.rotation, leftHandBlend);
            _mCombinedTransform.SetPositionAndRotation(combinedPosition, combinedRotation);

            return base.ComputeDesiredMove(input);
        }
    }
}
