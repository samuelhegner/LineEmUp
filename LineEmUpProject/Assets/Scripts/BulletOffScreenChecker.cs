using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class BulletOffScreenChecker : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Vector2 widthThresold;
        private Vector2 heightThresold;

        public event Action bulletOutOfBounds;

        void Start()
        {
            mainCamera = Camera.main;
            widthThresold.x = Screen.width + 0.05f;
        }

        void Update()
        {
            if (!BoundaryChecker.withinBulletEdges(transform.position)) 
            {
                bulletOutOfBounds?.Invoke();
                enabled = false;
                Destroy(gameObject);
            }
        }
    }
}