/* ==================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================== */

using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace TPCEngine
{
    public class FPCameraPostProcessing : MonoBehaviour
    {
        [SerializeField] private PostProcessingProfile profile;
        [SerializeField] private CharacterHealth characterHealth;
        [SerializeField] private int startPoint = 50;
        [SerializeField] private float chromaticAberrationSpeed = 2;
        [SerializeField] private float vignetteSmooth = 10;
        [Range(0, 1)] [SerializeField] private float vignetteMinValue = 0.3f;
        [Range(0, 1)] [SerializeField] private float vignetteMaxValue = 0.5f;
        [SerializeField] private float resetSmooth = 10;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        /// 
        /// <remarks>
        /// Like the Awake function, Start is called exactly once in the lifetime of the script. 
        /// However, Awake is called when the script object is initialised, regardless of whether or not the script is enabled. 
        /// Start may not be called on the same frame as Awake if the script is not enabled at initialisation time.
        /// </remarks>
        protected virtual void Start()
        {
            if (characterHealth != null)
                StartCoroutine(CameraPostProcessing());
        }
        

        /// <summary>
        /// First person camera post processing.
        /// </summary>
        /// <returns>IEnumerator</returns>
        public virtual IEnumerator CameraPostProcessing()
        {
            VignetteModel.Settings vignetteSettings = profile.vignette.settings;
            ChromaticAberrationModel.Settings chromaticAberrationSettings = profile.chromaticAberration.settings;
            bool resetSettings = false;
            while (true)
            {
                if (characterHealth.Health <= startPoint)
                {
                    float characterHealthInverseLerp = Mathf.InverseLerp(startPoint, 0, characterHealth.Health);
                    float intensityPingPong = Mathf.PingPong(Time.time * (characterHealthInverseLerp * chromaticAberrationSpeed), characterHealthInverseLerp);
                    float intensityLerp = Mathf.Lerp(vignetteSettings.intensity, characterHealthInverseLerp, Time.deltaTime * vignetteSmooth);
                    chromaticAberrationSettings.intensity = intensityPingPong;
                    vignetteSettings.intensity = Mathf.Clamp(intensityLerp, vignetteMinValue, vignetteMaxValue);
                    profile.chromaticAberration.settings = chromaticAberrationSettings;
                    profile.vignette.settings = vignetteSettings;

                    resetSettings = false;
                }
                else if (!resetSettings)
                {
                    if (profile.vignette.settings.intensity > 0 || profile.chromaticAberration.settings.intensity > 0)
                    {
                        chromaticAberrationSettings.intensity = Mathf.Lerp(chromaticAberrationSettings.intensity, 0, Time.deltaTime * resetSmooth);
                        vignetteSettings.intensity = Mathf.Lerp(vignetteSettings.intensity, 0, Time.deltaTime * resetSmooth);
                        profile.chromaticAberration.settings = chromaticAberrationSettings;
                        profile.vignette.settings = vignetteSettings;
                    }
                    else
                    {
                        resetSettings = true;
                    }
                }
                yield return null;
            }
        }

        public PostProcessingProfile Profile
        {
            get
            {
                return profile;
            }

            set
            {
                profile = value;
            }
        }

        public int StartPoint
        {
            get
            {
                return startPoint;
            }

            set
            {
                startPoint = value;
            }
        }

        public float ChromaticAberrationSpeed
        {
            get
            {
                return chromaticAberrationSpeed;
            }

            set
            {
                chromaticAberrationSpeed = value;
            }
        }

        public float VignetteSmooth
        {
            get
            {
                return vignetteSmooth;
            }

            set
            {
                vignetteSmooth = value;
            }
        }

        public float VignetteMinValue
        {
            get
            {
                return vignetteMinValue;
            }

            set
            {
                vignetteMinValue = value;
            }
        }

        public float VignetteMaxValue
        {
            get
            {
                return vignetteMaxValue;
            }

            set
            {
                vignetteMaxValue = value;
            }
        }

        public float ResetSmooth
        {
            get
            {
                return resetSmooth;
            }

            set
            {
                resetSmooth = value;
            }
        }

        public CharacterHealth _CharacterHealth
        {
            get
            {
                return characterHealth;
            }

            set
            {
                characterHealth = value;
            }
        }
    }
}