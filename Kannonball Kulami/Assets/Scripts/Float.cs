using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {

	public static float test;
	public float waterLevel = 0;
	public float floatHeight = 2;
	public float bounceDamp;
	public float forceDampening = 1;
	public Vector3 bouyancyForce;
	public int pointNum = 15;
	public Vector3[] points = new Vector3[15];
	public bool autoFill = true;

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 liftForce;
	private Vector3[] previousForces = new Vector3[15];
	private int realWaterLevelDiff;
	private int pointsUnderWater;

	private Vector3 size;

	void Start() {

		for (int i = 0; i < pointNum; i++) {
			//System.Random rand = new System.Random(gameObject.name.GetHashCode());
			//gameObject.transform.position = new Vector3(0f, rand.Next(-10, 11), 0f);
			previousForces[i].x = 0f;
			previousForces[i].y = 0f;
			previousForces[i].z = 0f;

			if (autoFill) {
				FloatShared fl = gameObject.GetComponentInParent<FloatShared>();
				if (fl != null) {
					floatHeight = fl.floatHeight;
					bouyancyForce = fl.bouyancyForce;
					waterLevel = fl.waterLevel;
					bounceDamp = fl.bounceDamp;
					gameObject.rigidbody.drag = fl.drag;
					gameObject.rigidbody.angularDrag = fl.angularDrag;
				}
				gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
				size = gameObject.collider.bounds.size;
				float x = size.x / 2 / 4 * 3;
				float y = size.y / 2 / 4 * 3;
				float z = size.z / 2 / 4 * 3;
				points[0] = new Vector3(x, y, z);
				points[1] = new Vector3(x, y, -z);
				points[2] = new Vector3(-x, y, z);
				points[3] = new Vector3(-x, y, -z);
				points[4] = new Vector3(0, y, 0);
				points[5] = new Vector3(x, 0, z);
				points[6] = new Vector3(x, 0, -z);
				points[7] = new Vector3(-x, 0, z);
				points[8] = new Vector3(-x, 0, -z);
				points[9] = new Vector3(0, 0, 0);
				points[10] = new Vector3(x, -y, z);
				points[11] = new Vector3(x, -y, -z);
				points[12] = new Vector3(-x, -y, z);
				points[13] = new Vector3(-x, -y, -z);
				points[14] = new Vector3(0, -y, 0);
			}


		}
	}

	void FixedUpdate() {
		if (gameObject.name == "TEST") {
			int x = 5;	
		}
		//System.Random rand1 = new System.Random(gameObject.name.GetHashCode() * Time.deltaTime);
		for (int i = 0; i < pointNum; i++) {
			System.Random rand = new System.Random(i * gameObject.name.GetHashCode() * System.Convert.ToInt32(Time.deltaTime));
			//realWaterLevelDiff = rand.Next(-10, 11);
			//int dampen = rand.Next (1, 10);
			//Random.seed = i * Random.seed * gameObject.name.GetHashCode();
			actionPoint = rigidbody.worldCenterOfMass + transform.TransformDirection(points[i]);
			forceFactor = (waterLevel + floatHeight - actionPoint.y);
			
			if (forceFactor > 0f) {

				if (forceFactor < 1f) {
					forceFactor *= forceFactor;
				}
				liftForce = bouyancyForce * (forceFactor - rigidbody.velocity.y * bounceDamp);

				liftForce *= Random.Range(.1f, 1f); //System.Convert.ToSingle(rand.NextDouble());
				//if (liftForce.y > previousForces[i].y * 2) {
				//	Debug.Log(liftForce);
				//	liftForce.y = liftForce.y / 2;
				//	Debug.Log(liftForce);
				//}
				//if (liftForce.y > previousForces[i].y) {
				//	liftForce.y = (liftForce.y + previousForces[i].y) / 2;
				//}
				//previousForces[i] = liftForce;
				//Debug.Log(liftForce);
				rigidbody.AddForceAtPosition(liftForce, actionPoint);
			}
		}
	}

}
