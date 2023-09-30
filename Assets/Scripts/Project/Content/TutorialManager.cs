using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ProjectBubble.Core;
using ProjectBubble.Core.Combat;

namespace ProjectBubble.Content
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private GameObject _bubbleHereVisual;
        [SerializeField] private GameObject _attackHereVisual;
        [SerializeField] private GameObject _tutorialEnemyPrefab;

        private void Start()
        {
            StartCoroutine(DoTutorialSequence());
        }

        private IEnumerator DoTutorialSequence()
        {
            //Alright boys it's time for a lil ol tutorials.
            //How will the tutorial be structured??? Hmmm

            //Arrow that appearso ver a dummy enemy telling you to click, mouse visual to show what button is to attack
            //After the enemy is bubbled (check by checking it's active state, move to next tutorial)
            //Another enemy spawns and tells the player to attack. Once the enemy is dead the real game can begin...


            //And that's the tutorial! We can add the VFX/Animations later.
            GameObject instance = _waveManager.SpawnPrefab(_tutorialEnemyPrefab);
            _bubbleHereVisual.transform.position = instance.transform.position;
            _bubbleHereVisual.gameObject.SetActive(true);

            yield return new WaitUntil(() => instance == null || !instance.gameObject.activeSelf);
            GameObject target = _waveManager.SpawnPrefab(_tutorialEnemyPrefab);
            _attackHereVisual.transform.position = target.transform.position;
            _attackHereVisual.gameObject.SetActive(true);

            bool isDead = false;
            void Clear()
            {
                isDead = true;
            }

            Entity entity = target.GetComponent<Entity>();
            entity.OnClear += Clear;
            yield return new WaitUntil(() => isDead);

            //Begin Game
            _waveManager?.NextWave();
        }
    }
}
