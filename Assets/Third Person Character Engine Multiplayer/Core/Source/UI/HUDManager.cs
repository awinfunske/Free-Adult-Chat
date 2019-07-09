/* ==================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================== */

using UnityEngine;
using UnityEngine.UI;
using TPCEngine.Utility;

namespace TPCEngine.UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        [SerializeField] private Text healthPoint;
        [SerializeField] private Scrollbar healthBar;

        private int lastHealth;
      
        /// <summary>
        /// Update the health UI value.
        /// </summary>
        /// <param name="health"></param>
        public virtual void UpdateHealth(int health, int maxHealth)
        {
            if (lastHealth == health)
                return;

            if (healthPoint != null)
                healthPoint.text = health.ToString();
            if (healthBar != null)
                healthBar.size = Mathf.InverseLerp(0, maxHealth, health);
            lastHealth = health;
        }

        public Text HealthPoint
        {
            get
            {
                return healthPoint;
            }

            set
            {
                healthPoint = value;
            }
        }

        public Scrollbar HealthBar
        {
            get
            {
                return healthBar;
            }

            set
            {
                healthBar = value;
            }
        }

    }
}