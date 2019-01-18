using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters {
    public class WeaponRangeSystem : WeaponSystem {
    
        [SerializeField] GameObject projectilePrefab;

        protected override void DoDamage() {
            //Spawn a projectile, the collider of the projectile will do the damage
            print("New Projectile");
        }

    }
}