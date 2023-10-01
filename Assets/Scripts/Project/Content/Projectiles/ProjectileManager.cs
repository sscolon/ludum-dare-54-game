using ProjectBubble.Core.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Content.Projectiles
{
    public class ProjectileManager : MonoBehaviour
    {
        private static ProjectileManager _instance;
        [SerializeField] private Projectile _projectilePrefab;
        private void OnEnable()
        {
            _instance = this;
        }

        public static void CreateForwardProjectiles(Vector3 startPosition, Vector3 direction, int projectileCount, float projectileSpread, float randomSpread = 0)
        {
            ProjectileManager projectileManager = _instance;
            for (int i = 0; i < projectileCount; i++)
            {
                float rand = UnityEngine.Random.Range(-randomSpread, randomSpread);
                float spread = projectileSpread + rand;
                float targetAngle = (spread / projectileCount);
                float angle = (i * targetAngle) - (targetAngle * (projectileCount / 2));
                if (projectileCount == 1)
                {
                    angle = 0 + rand;
                }

                Vector3 dir = direction + Quaternion.Euler(0, 0, angle) * direction;
                Projectile projectile = Instantiate(projectileManager._projectilePrefab, startPosition, projectileManager._projectilePrefab.transform.rotation);
                projectile.Right = dir;
            }
        }

        public static void CreateProjectileBurst(Vector3 startPosition, int projectileCount, float radius = 1, float randomSpread = 0)
        {
            ProjectileManager projectileManager = _instance;
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * (360f / projectileCount);
                float spread = UnityEngine.Random.Range(-randomSpread, randomSpread);
                Vector3 direction = Quaternion.Euler(0, 0, angle + spread) * Vector3.up;
                Projectile projectile = Instantiate(projectileManager._projectilePrefab, startPosition, projectileManager._projectilePrefab.transform.rotation);
                projectile.Right = direction;
            }
        }
    }
}
