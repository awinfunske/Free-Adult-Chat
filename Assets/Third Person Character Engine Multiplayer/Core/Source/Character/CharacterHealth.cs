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
using TPCEngine.UI;

namespace TPCEngine
{
    /// <summary>
    /// Fal Damage struct
    /// </summary>
    [System.Serializable]
    public struct FallDamageParam
    {
        public float minHeight;
        public float maxHeight;
        public int damage;
    }

    [System.Serializable]
    public struct RegenirationParam
    {
        public float interval;
        public int value;
        public float time;
    }

    /// <summary>
    /// Base Player Health class
    /// </summary>
    public class CharacterHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private int startHealth;
        [SerializeField] private FallDamageParam[] fallDamageParams;
        [SerializeField] private bool useRegeniration;
        [SerializeField] private RegenirationParam regenerationParam;

        private Animator animator;
        private TPCamera characterCamera;
        private TPCMotor characterMotor;
        private float lastHeightPosition;
        private bool onceReset;
        private bool regenerateProcessCoroutineIsRunning;

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
            animator = GetComponent<Animator>();
            characterMotor = GetComponent<TPCharacter>().GetCharacteMotor();
            characterCamera = GetComponent<TPCharacter>().GetCamera();
            transform.SetKinematic(true);
            health = startHealth;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            HealthHandler();
            HUDManager.Instance.UpdateHealth(health, maxHealth);
            if (IsAlive)
            {
                FallDamage(characterMotor.IsGrounded);
                HealthRegenerationHandler();
            }
        }

        /// <summary>
        /// Character Health handler
        /// </summary>
        public virtual void HealthHandler()
        {
            if (!IsAlive)
            {
                characterMotor.LockMovement = true;
                characterCamera.SetTarget(animator.GetBoneTransform(HumanBodyBones.Spine));
                Ragdoll(true);
                onceReset = true;
            }
            else if (onceReset)
            {
                characterMotor.LockMovement = false;
                characterCamera.SetTarget(transform);
                Ragdoll(false);
                onceReset = false;
            }
        }

        /// <summary>
        /// Health Regenegation System
        /// </summary>
        public virtual void HealthRegenerationHandler()
        {
            if (useRegeniration && health != maxHealth && !regenerateProcessCoroutineIsRunning)
                StartCoroutine(RegenerateProcess(regenerationParam));

        }

        public IEnumerator RegenerateProcess(RegenirationParam regenerationParam)
        {
            bool waitBeforeStart = true;
            bool playRegenerate = false;
            regenerateProcessCoroutineIsRunning = true;

            while (true)
            {
                while (waitBeforeStart)
                {
                    float lastHealth = health;
                    if (!IsAlive)
                        yield break;
                    yield return new WaitForSeconds(regenerationParam.time);
                    if (lastHealth == health)
                    {
                        waitBeforeStart = false;
                        playRegenerate = true;
                        break;
                    }
                }

                while (playRegenerate)
                {
                    if (!IsAlive)
                        yield break;
                    health += regenerationParam.value;
                    float lastHealth = health;
                    yield return new WaitForSeconds(regenerationParam.interval);
                    if (health >= maxHealth)
                    {
                        health = maxHealth;
                        regenerateProcessCoroutineIsRunning = false;
                        yield break;
                    }
                    else if (lastHealth != health)
                    {
                        waitBeforeStart = true;
                        playRegenerate = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Take damage
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            health -= amount;
        }

        public virtual void FallDamage(bool isGrounded)
        {
            if (!isGrounded)
            {
                if (lastHeightPosition < transform.position.y)
                {
                    lastHeightPosition = transform.position.y;
                }
            }
            else if (lastHeightPosition > transform.position.y)
            {
                float distance = lastHeightPosition - transform.position.y;
                for (int i = 0; i < fallDamageParams.Length; i++)
                {
                    if (distance > fallDamageParams[i].minHeight && distance < fallDamageParams[i].maxHeight)
                    {
                        TakeDamage(fallDamageParams[i].damage);
                        lastHeightPosition = transform.position.y;
                    }
                }
            }
        }

        public virtual void Ragdoll(bool isStart)
        {
            transform.SetKinematic(!isStart);
            animator.enabled = !isStart;
        }

        /// <summary>
        /// Player health
        /// </summary>
        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }

        /// <summary>
        /// Player is alive
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return health > 0;
            }
        }

        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

        public float HealthPercent { get { return ((float)health / maxHealth) * 100; } }

    }
}