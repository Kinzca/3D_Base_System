using System;
using C_.PlayerController;
using UnityEngine;

namespace C_.EnemyController
{
    public class ShieldTrigger : MonoBehaviour
    {
        public PlayerFsm manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.TransitionState(StateType.Hit);
            }
        }
    }
}
