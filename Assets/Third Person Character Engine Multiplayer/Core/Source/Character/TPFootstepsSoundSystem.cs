/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using TPCEngine.Utility;
using UnityEngine;

namespace TPCEngine
{
	/// <summary>
	/// Volume relative to speed
	/// </summary>
	[System.Serializable]
	public struct FootstepsVolume
	{
		public float volume;
		public float minSpeed;
		public float maxSpeed;
	}

	/// <summary>
	/// The sound of steps relative to the material of the object
	/// </summary>
	[System.Serializable]
	public struct FootstepsMaterial
	{
        public string name;
        public PhysicMaterial physicMaterial;
        public Texture2D texture;
        public AudioClip[] footstepsClips;
		public AudioClip[] landingClips;
	}

	/// <summary>
	/// Third person character footsteps sound system
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class TPFootstepsSoundSystem : MonoBehaviour
	{
		#region [Variables are editable in the inspector]
		[SerializeField] private Transform leftFoot;
		[SerializeField] private Transform rightFoot;
		[SerializeField] private float defaultVolume;
		[SerializeField] private FootstepsVolume[] footstepsVolumes;
		[SerializeField] private FootstepsMaterial[] footstepsMaterials;
		[SerializeField] private float rayRange;
		#endregion

		#region [Required variables]
		private AudioSource audioSource;
		private TPCMotor characterMotor;
		private bool leftFootIsPlayed;
		private bool rightFootIsPlayed;
		private bool isLanding;
        private Coroutine playLandSoundCoroutine;
        private bool playLandSoundCoroutineIsRunning;
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
			audioSource = GetComponent<AudioSource>();
			characterMotor = GetComponent<TPCharacter>().GetCharacteMotor();
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		protected virtual void Update()
		{
			ProcessLanding();
			if (characterMotor.MoveAmount > 0)
			{
				VolumeHandler();
				ProcessingSteps();
			}
			else
			{
				audioSource.volume = defaultVolume;
			}
		}

		/// <summary>
		/// Handling volume relative to the character speed
		/// </summary>
		protected virtual void VolumeHandler()
		{
			for (int i = 0; i < footstepsVolumes.Length; i++)
			{
				if (characterMotor.MoveAmount >= footstepsVolumes[i].minSpeed && characterMotor.MoveAmount < footstepsVolumes[i].maxSpeed)
				{
					audioSource.volume = footstepsVolumes[i].volume;
					break;
				}
			}
		}

		/// <summary>
		/// Handling character steps 
		/// </summary>
		protected virtual void ProcessingSteps()
		{
			RaycastHit leftFootRayHit;

			if (Physics.Raycast(leftFoot.position, -leftFoot.up, out leftFootRayHit, rayRange))
			{
				if (!leftFootIsPlayed)
                {
                    Object surfaceInfo = SurfaceHelper.GetSurfaceType(leftFootRayHit.collider, leftFootRayHit.point);
                    if (!surfaceInfo)
                        return;
                    PlayStepSound(surfaceInfo);
                }
                leftFootIsPlayed = true;
			}
			else
			{
				leftFootIsPlayed = false;
			}

			RaycastHit rigthFootRayHit;
			if (Physics.Raycast(rightFoot.position, -rightFoot.up, out rigthFootRayHit, rayRange))
			{
				if (!rightFootIsPlayed)
                {
                    Object surfaceInfo = SurfaceHelper.GetSurfaceType(rigthFootRayHit.collider, rigthFootRayHit.point);
                    if (!surfaceInfo)
                        return;
                    PlayStepSound(surfaceInfo);
                }
                rightFootIsPlayed = true;
			}
			else
			{
				rightFootIsPlayed = false;
			}
		}

		/// <summary>
		/// Processing character landing
		/// </summary>
		protected virtual void ProcessLanding()
		{
			if (!characterMotor.IsGrounded)
				isLanding = true;

			if (isLanding && characterMotor.IsGrounded)
			{
                if (playLandSoundCoroutineIsRunning)
                    StopCoroutine(playLandSoundCoroutine);
                playLandSoundCoroutine = StartCoroutine(PlayLandSound());
				isLanding = false;
			}
		}

		/// <summary>
		/// Play step sound
		/// </summary>
		/// <param name="material"></param>
		public virtual void PlayStepSound(Object surfaceInfo)
		{
			for (int i = 0; i < footstepsMaterials.Length; i++)
			{
				if (footstepsMaterials[i].physicMaterial == surfaceInfo || footstepsMaterials[i].texture == surfaceInfo)
				{
					int randomClip = Random.Range(0, footstepsMaterials[i].footstepsClips.Length);
					audioSource.PlayOneShot(footstepsMaterials[i].footstepsClips[randomClip]);
					break;
				}
			}
		}

		/// <summary>
		/// Play step sound
		/// </summary>
		/// <param name="material"></param>
		public virtual IEnumerator PlayLandSound()
		{
			RaycastHit landHit;
            playLandSoundCoroutineIsRunning = true;
            if (Physics.Raycast(characterMotor.CharacterTransform.position, -Vector3.up, out landHit, 100.0f))
			{
                Object surfaceInfo = SurfaceHelper.GetSurfaceType(landHit.collider, landHit.point);
                if (!surfaceInfo)
                {
                    playLandSoundCoroutineIsRunning = false;
                    yield break;
                }

                for (int i = 0; i < footstepsMaterials.Length; i++)
				{
					if (footstepsMaterials[i].physicMaterial == surfaceInfo || footstepsMaterials[i].texture == surfaceInfo)
					{
						int randomClip = Random.Range(0, footstepsMaterials[i].landingClips.Length);
						audioSource.PlayOneShot(footstepsMaterials[i].landingClips[randomClip]);
                        playLandSoundCoroutineIsRunning = false;
                        yield break;
                    }
                    yield return null;
                }
            }
            playLandSoundCoroutineIsRunning = false;
            yield break;
		}

		#endregion

		#region [Properties]
		/// <summary>
		/// Character Left Foot
		/// </summary>
		/// <value></value>
		public Transform LeftFoot { get { return leftFoot; } set { leftFoot = value; } }

		/// <summary>
		/// Character Right Foot
		/// </summary>
		/// <value></value>
		public Transform RightFoot { get { return rightFoot; } set { rightFoot = value; } }

		/// <summary>
		/// Volume relative to the character speed
		/// </summary>
		/// <value></value>
		public FootstepsVolume[] FootstepsVolumes { get { return footstepsVolumes; } set { footstepsVolumes = value; } }

		/// <summary>
		/// The sound of steps relative to the material of the object
		/// </summary>
		/// <value></value>
		public FootstepsMaterial[] FootstepsMaterials { get { return footstepsMaterials; } set { footstepsMaterials = value; } }

		public float RayRange { get { return rayRange; } set { rayRange = value; } }
		#endregion
	}
}