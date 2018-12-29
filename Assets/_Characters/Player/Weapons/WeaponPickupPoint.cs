using UnityEngine;

namespace RPG.Characters {
    public class WeaponPickupPoint : MonoBehaviour {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickupSoundEffect;

        WeaponSystem weaponSystem;
        AudioSource audioSource;

        // Start is called before the first frame update
        void Start() {
            weaponSystem = GameObject.FindWithTag("Player").GetComponent<WeaponSystem>(); 
            audioSource = GetComponent<AudioSource>();

            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            var weaponGameObject = Instantiate(weaponPrefab,transform.position,transform.rotation);
            weaponGameObject.transform.parent = transform;
        }

        void OnTriggerEnter(Collider other) {
            weaponSystem.PutWeaponInHand(weaponConfig);
            audioSource.PlayOneShot(pickupSoundEffect);
        }
    }
}
