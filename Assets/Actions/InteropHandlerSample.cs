using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace ActionUnity
{

    public class InteropHandlerSample : InteropHandler
    {
        // the id which will be use in registration of action vertex buffer
        private const string _ActionVertexBufferName = "sampleVertexBuffer";

        [SerializeField] private ParticlesDrawer _particlesDrawer;

        [SerializeField] private int _sizeBuffer = 256;

        private ComputeBuffer _computeBuffer;

        /// <summary>
        /// Create a compute buffer of float4 of size _sizeBuffer
        /// </summary>
        private void CreateBuffer()
        {
            int stride = Marshal.SizeOf(typeof(float4));
            //allocate memory for compute buffer
            _computeBuffer = new ComputeBuffer(_sizeBuffer, stride);
            _particlesDrawer.InitParticlesBuffer(_computeBuffer, _sizeBuffer, 0.1f);
        }


        private void Start()
        {
            InitializeInteropHandler();

        }

        /// <summary>
        /// Create the texture and the buffer. Construct action from them. Register these action in InteropUnityCUDA and
        /// call start function on it
        /// </summary>
        protected override void InitializeActions()
        {
            base.InitializeActions();

            if (_particlesDrawer == null)
            {
                Debug.LogError("Set particles drawer in inspector !");
                return;
            }

            InitSampleVertexBuffer();
        }

        private void InitSampleVertexBuffer()
        {
            CreateBuffer();
            ActionUnitySampleVertexBuffer actionUnitySampleVertexBuffer = new ActionUnitySampleVertexBuffer(_computeBuffer, _sizeBuffer);
            RegisterActionUnity(actionUnitySampleVertexBuffer, _ActionVertexBufferName);
            CallFunctionStartInAction(_ActionVertexBufferName);

        }

        public void Update()
        {
            UpdateInteropHandler();
        }

        /// <summary>
        /// call update function of the two registered actions
        /// </summary>
        protected override void UpdateActions()
        {
            base.UpdateActions();
            CallFunctionUpdateInAction(_ActionVertexBufferName);
        }

        public void OnDestroy()
        {
            OnDestroyInteropHandler();
        }

        /// <summary>
        /// call onDestroy function of the two registered actions
        /// </summary>
        protected override void OnDestroyActions()
        {
            base.OnDestroyActions();
            // CallFunctionOnDestroyInAction(_ActionVertexBufferName);
        }
    }
}
