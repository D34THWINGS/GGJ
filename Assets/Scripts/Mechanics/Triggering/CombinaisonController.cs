using UnityEngine;
using System.Collections.Generic;
using System.Timers;
using XRay.Objects;

namespace XRay.Mechanics.Triggering {

	public class CombinaisonController : TriggeringMechanism {
		
		public Transform Spawn;
		public GameObject SpawnedObject;
		public float SpawnInterval = 5000f;
		public List<int> Combinaison;
		public bool Enabled {
			get {
				return enabled;
			}
			set {
				if (value)
					timer.Start();
				else
					timer.Stop();
				enabled = value;
			}
		}
		
		private int nbOfValid = 0;
		private Timer timer;
		private bool pop = false;
		
		void Start() {
			if (Spawn == null)
				throw new UnityException("This component needs the spawn to be setted");
			if (SpawnedObject == null)
				throw new UnityException("This component needs the spawned object to be setted");
			
			timer = new Timer(SpawnInterval);
			timer.Elapsed += (sender, e) => {
				pop = true;
			};
			timer.AutoReset = true;
			timer.Start();
		}
		
		void Update() {
			// Spawn a random shape
			if (pop) {
				GameObject spawned = (GameObject) Instantiate(SpawnedObject);
				spawned.name = "Digit" + nbOfValid + 1;
				spawned.transform.position = Spawn.position;
				spawned.AddComponent<Rigidbody2D>();
				spawned.GetComponent<Reshape>().CurrentShape = Random.Range(0, 1);
				spawned.transform.parent = transform.parent;
				pop = false;
			}
		}
		
		void OnTriggerEnter2D(Collider2D collider) {
			if (!Enabled) return;
			
			var resh = collider.gameObject.GetComponent<Reshape>();
			if (resh == null) return;
			
			if (nbOfValid == Combinaison.Count || resh.CurrentShape != Combinaison[nbOfValid]){ 
				Destroy(collider.gameObject);
				particleSystem.Play();
			} else {
				nbOfValid++;
				Destroy(collider.gameObject.GetComponent<InteractibleObject>());
			}
			
			if (nbOfValid == Combinaison.Count) {
				Enabled = false;
				timer.Stop();
				Trigger(EventNames.ENABLE);
			}
		}
	}
}
