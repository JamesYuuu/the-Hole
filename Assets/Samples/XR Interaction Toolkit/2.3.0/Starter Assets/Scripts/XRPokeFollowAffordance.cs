using Unity.Mathematics;
using Unity.XR.CoreUtils.Bindings;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Follow animation affordance for <see cref="IPokeStateDataProvider"/>, such as <see cref="XRPokeFilter"/>.
    /// Used to animate a pressed transform, such as a button to follow the poke position.
    /// </summary>
    [AddComponentMenu("XR/XR Poke Follow Affordance", 22)]
    public class XRPokeFollowAffordance : MonoBehaviour
    {
        [FormerlySerializedAs("m_PokeFollowTransform")]
        [SerializeField]
        [Tooltip("Transform that will move in the poke direction when this or a parent GameObject is poked." +
                 "\nNote: Should be a direct child GameObject.")]
        Transform mPokeFollowTransform;

        /// <summary>
        /// Transform that will animate along the axis of interaction when this interactable is poked.
        /// Note: Must be a direct child GameObject as it moves in local space relative to the poke target's transform.
        /// </summary>
        public Transform PokeFollowTransform
        {
            get => mPokeFollowTransform;
            set => mPokeFollowTransform = value;
        }

        [FormerlySerializedAs("m_SmoothingSpeed")]
        [SerializeField]
        [Range(0f, 20f)]
        [Tooltip("Multiplies transform position interpolation as a factor of Time.deltaTime. If 0, no smoothing will be applied.")]
        float mSmoothingSpeed = 8f;

        /// <summary>
        /// Multiplies transform position interpolation as a factor of <see cref="Time.deltaTime"/>. If <c>0</c>, no smoothing will be applied.
        /// </summary>
        public float SmoothingSpeed
        {
            get => mSmoothingSpeed;
            set => mSmoothingSpeed = value;
        }

        [FormerlySerializedAs("m_ReturnToInitialPosition")]
        [SerializeField]
        [Tooltip("When this component is no longer the target of the poke, the Poke Follow Transform returns to the original position.")]
        bool mReturnToInitialPosition = true;

        /// <summary>
        /// When this component is no longer the target of the poke, the <see cref="PokeFollowTransform"/> returns to the original position.
        /// </summary>
        public bool ReturnToInitialPosition
        {
            get => mReturnToInitialPosition;
            set => mReturnToInitialPosition = value;
        }

        [FormerlySerializedAs("m_ApplyIfChildIsTarget")]
        [SerializeField]
        [Tooltip("Whether to apply the follow animation if the target of the poke is a child of this transform. " +
                 "This is useful for UI objects that may have child graphics.")]
        bool mApplyIfChildIsTarget = true;

        /// <summary>
        /// Whether to apply the follow animation if the target of the poke is a child of this transform.
        /// This is useful for UI objects that may have child graphics.
        /// </summary>
        public bool ApplyIfChildIsTarget
        {
            get => mApplyIfChildIsTarget;
            set => mApplyIfChildIsTarget = value;
        }

        [FormerlySerializedAs("m_ClampToMaxDistance")]
        [SerializeField]
        [Tooltip("Whether to keep the Poke Follow Transform from moving past a maximum distance from the poke target.")]
        bool mClampToMaxDistance;

        /// <summary>
        /// Whether to keep the <see cref="PokeFollowTransform"/> from moving past <see cref="MaxDistance"/> from the poke target.
        /// </summary>
        public bool ClampToMaxDistance
        {
            get => mClampToMaxDistance;
            set => mClampToMaxDistance = value;
        }

        [FormerlySerializedAs("m_MaxDistance")]
        [SerializeField]
        [Tooltip("The maximum distance from this transform that the Poke Follow Transform can move.")]
        float mMaxDistance;

        /// <summary>
        /// The maximum distance from this transform that the <see cref="PokeFollowTransform"/> can move when
        /// <see cref="ClampToMaxDistance"/> is <see langword="true"/>.
        /// </summary>
        public float MaxDistance
        {
            get => mMaxDistance;
            set => mMaxDistance = value;
        }

        IPokeStateDataProvider _mPokeDataProvider;

        readonly Vector3TweenableVariable _mTransformTweenableVariable = new Vector3TweenableVariable();
        readonly BindingsGroup _mBindingsGroup = new BindingsGroup();
        Vector3 _mInitialPosition;
        bool _mIsFirstFrame;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Awake()
        {
            _mPokeDataProvider = GetComponentInParent<IPokeStateDataProvider>();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Start()
        {
            if (mPokeFollowTransform != null)
            {
                _mInitialPosition = mPokeFollowTransform.localPosition;
                _mBindingsGroup.AddBinding(_mTransformTweenableVariable.Subscribe(OnTransformTweenableVariableUpdated));
                _mBindingsGroup.AddBinding(_mPokeDataProvider.pokeStateData.SubscribeAndUpdate(OnPokeStateDataUpdated));
            }
            else
            {
                enabled = false;
                Debug.LogWarning($"Missing Poke Follow Transform assignment on {this}. Disabling component.", this);
            }
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDestroy()
        {
            _mBindingsGroup.Clear();
            _mTransformTweenableVariable?.Dispose();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void LateUpdate()
        {
            if (_mIsFirstFrame)
            {
                _mTransformTweenableVariable.HandleTween(1f);
                _mIsFirstFrame = false;
                return;
            }
            _mTransformTweenableVariable.HandleTween(mSmoothingSpeed > 0f ? Time.deltaTime * mSmoothingSpeed : 1f);
        }

        void OnTransformTweenableVariableUpdated(float3 position)
        {
            mPokeFollowTransform.localPosition = position;
        }

        void OnPokeStateDataUpdated(PokeStateData data)
        {
            var pokeTarget = data.target;
            var applyFollow = mApplyIfChildIsTarget
                ? pokeTarget != null && pokeTarget.IsChildOf(transform)
                : pokeTarget == transform;

            if (applyFollow)
            {
                var targetPosition = pokeTarget.InverseTransformPoint(data.axisAlignedPokeInteractionPoint);
                if (mClampToMaxDistance && targetPosition.sqrMagnitude > mMaxDistance * mMaxDistance)
                    targetPosition = Vector3.ClampMagnitude(targetPosition, mMaxDistance);

                _mTransformTweenableVariable.target = targetPosition;
            }
            else if (mReturnToInitialPosition)
            {
                _mTransformTweenableVariable.target = _mInitialPosition;
            }
        }
    }
}