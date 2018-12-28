using UnityEngine;

namespace RPG.Characters {
    public class WeaponPickupPoint : MonoBehaviour {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickupSoundEffect;

        PlayerMovement player;
        AudioSource audioSource;

        // Start is called before the first frame update
        void Start() {
            player = FindObjectOfType<PlayerMovement>();
            audioSource = GetComponent<AudioSource>();

            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            var weaponGameObject = Instantiate(weaponPrefab,transform.position,transform.rotation);
            weaponGameObject.transform.parent = transform;
        }

        void OnTriggerEnter(Collider other) {
            player.PutWeaponInHand(weaponConfig);
            audioSource.PlayOneShot(pickupSoundEffect);
        }
    }
}
