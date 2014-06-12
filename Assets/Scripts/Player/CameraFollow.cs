using UnityEngine;

namespace XRay.Player {
    public class CameraFollow : MonoBehaviour {
        public Transform Target;
        public float XMargin = 4f; // Distance in the x axis the player can move before the camera follows.
        public float YMargin = 2f; // Distance in the y axis the player can move before the camera follows.
        public float XSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
        public float YSmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
        public Vector2 MaxXAndY; // The maximum x and y coordinates the camera can have.
        public Vector2 MinXAndY; // The minimum x and y coordinates the camera can have.


        private Transform _player; // Reference to the player's transform.
        private Vector3 _velocity;


        public void Awake() {
            // Setting up the reference.
            _player = GameObject.Find("Player").transform;
        }


        private bool CheckXMargin() {
            // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
            return Mathf.Abs(transform.position.x - _player.position.x) > XMargin;
        }


        private bool CheckYMargin() {
            // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
            return Mathf.Abs(transform.position.y - _player.position.y) > YMargin;
        }


        public void FixedUpdate() {
            //TrackPlayer();
        }

        private void Update() {
            if (!Target || (!CheckXMargin() && !CheckYMargin())) return;
            var point = camera.WorldToViewportPoint(Target.position);
            var delta = Target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                //(new Vector3(0.5, 0.5, point.z));
            var destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, 0.15f);
        }

        private void TrackPlayer() {
            // By default the target x and y coordinates of the camera are it's current x and y coordinates.
            var targetX = transform.position.x;
            var targetY = transform.position.y;

            // If the player has moved beyond the x margin...
            if (CheckXMargin())
                // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
                targetX = Mathf.Lerp(transform.position.x, _player.position.x, XSmooth*Time.deltaTime);

            // If the player has moved beyond the y margin...
            if (CheckYMargin())
                // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
                targetY = Mathf.Lerp(transform.position.y, _player.position.y, YSmooth*Time.deltaTime);

            // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
            //targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
            //targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

            // Set the camera's position to the target position with the same z component.
            transform.position = new Vector3(targetX, targetY, transform.position.z);
        }
    }
}