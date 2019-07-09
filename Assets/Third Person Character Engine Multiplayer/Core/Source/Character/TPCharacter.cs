/* ==================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================== */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCEngine
{
	/// <summary>
	/// 
	/// </summary>
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class TPCharacter : MonoBehaviour
	{
        #region [Variables are editable in the inspector]
        [SerializeField] private TPCamera characterCamera;
		[SerializeField] private TPCMotor characterMotor = new TPCMotor();
		[SerializeField] private TPCInverseKinematics inverseKinematics = new TPCInverseKinematics();
		#endregion

		#region [Required variables]
		private TPCAnimatorHandler animatorHandler = new TPCAnimatorHandler();
        #endregion

        #region [Functions]
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        /// 
        /// <remarks>
        /// Awake is called only once during the lifetime of the script instance.
        /// Awake is always called before any Start functions.
        /// This allows you to order initialization of scripts.
        /// </remarks>
        protected virtual void Awake()
		{
			characterMotor.Init(transform, GetComponent<Animator>(), GetComponent<Rigidbody>(), GetComponent<CapsuleCollider>());
			animatorHandler.Init(GetComponent<Animator>(), transform, characterMotor);
			inverseKinematics.Init(GetComponent<Animator>());
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void Update()
		{
			characterMotor.UpdateMotor();
			animatorHandler.UpdateAnimator();
		}

		/// <summary>
		/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void FixedUpdate()
		{
			characterMotor.AirControl();
		}

		/// <summary>
		/// Callback for processing animation movements for modifying root motion.
		/// </summary>
		private void OnAnimatorMove()
		{
			animatorHandler.AnimatorMove();
		}

		/// <summary>
		/// Callback for setting up animation IK (inverse kinematics).
		/// </summary>
		/// <param name="layerIndex">Index of the layer on which the IK solver is called.</param>
		private void OnAnimatorIK(int layerIndex)
		{
			inverseKinematics.HeadIK();
			inverseKinematics.HandIK();
			inverseKinematics.FootIK();
		}

		/// <summary>
		/// TPCMotor Instance
		/// </summary>
		/// <returns></returns>
		public TPCMotor GetCharacteMotor()
		{
			return characterMotor;
		}

		/// <summary>
		/// TPCAnimatorHandler Instance
		/// </summary>
		/// <returns></returns>
		public TPCAnimatorHandler GetAnimatorHandler()
		{
			return animatorHandler;
		}

		/// <summary>
		/// TPCInverseKinematics Instance
		/// </summary>
		/// <returns></returns>
		public TPCInverseKinematics GetInverseKinematics()
		{
			return inverseKinematics;
		}

        /// <summary>
        /// TPCamera Instance
        /// </summary>
        /// <returns></returns>
        public TPCamera GetCamera()
        {
            return characterCamera;
        }

        public void SetCamera(TPCamera characterCamera)
        {
            this.characterCamera = characterCamera;
        }
		#endregion
	}
}