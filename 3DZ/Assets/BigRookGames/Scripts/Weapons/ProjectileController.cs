using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        // --- Config ---
        public float speed = 100;

        [Header("Explosion Settings")]
        public float explosionRadius = 5f;
        public float explosionForce = 800f;
        [Tooltip("Chỉ tác động lên các layer này (để trống = mọi layer).")]
        public LayerMask explosionAffectLayers = ~0;
        [Tooltip("Độ nâng theo trục Y khi nổ (tham số thứ 3 của AddExplosionForce).")]
        public float upwardsModifier = 1f;

        // --- Explosion VFX ---
        public GameObject rocketExplosion;

        // --- Projectile Mesh ---
        public MeshRenderer projectileMesh;

        // --- Script Variables ---
        private bool targetHit;

        // --- Audio ---
        public AudioSource inFlightAudioSource;

        // --- VFX ---
        public ParticleSystem disableOnHit;

        private void Update()
        {
            if (targetHit) return;
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!enabled) return;

            Explode();
            BlowObjects(); // <--- thêm gọi nổ đẩy Rigidbody

            projectileMesh.enabled = false;
            targetHit = true;

            if (inFlightAudioSource) inFlightAudioSource.Stop();

            foreach (Collider col in GetComponents<Collider>())
                col.enabled = false;

            if (disableOnHit) disableOnHit.Stop();

            Destroy(gameObject, 5f);
        }

        /// <summary>Spawn hiệu ứng nổ</summary>
        private void Explode()
        {
            if (rocketExplosion)
                Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation);
        }

        /// <summary>Đẩy các Rigidbody trong bán kính nổ</summary>
        private void BlowObjects()
        {
            // Lấy tất cả collider trong bán kính nổ theo layer chỉ định
            Collider[] affected = Physics.OverlapSphere(transform.position, explosionRadius, explosionAffectLayers);

            for (int i = 0; i < affected.Length; i++)
            {
                Rigidbody rb = affected[i].attachedRigidbody;
                if (rb == null) continue;
                if (rb.isKinematic) continue; // kinematic thì không chịu lực

                // Thêm lực nổ
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }

        // Vẽ bán kính nổ trong Scene view cho dễ căn chỉnh
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
