using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {

	public float waterLevel = 0;
	public float floatHeight = 2;
	public float bounceDamp;
	public Vector3 bouyancyForce;
	public Vector3[] points = new Vector3[15];

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 liftForce;
	private Vector3[] previousForces = new Vector3[15];
	private int realWaterLevelDiff;
	private int pointsUnderWater;

	void Start() {
		for (int i = 0; i < 15; i++) {
			//System.Random rand = new System.Random(gameObject.name.GetHashCode());
			//gameObject.transform.position = new Vector3(0f, rand.Next(-10, 11), 0f);
			previousForces[i].x = 0f;
			previousForces[i].y = 0f;
			previousForces[i].z = 0f;
		}
	}

	void FixedUpdate() {
		//System.Random rand1 = new System.Random(gameObject.name.GetHashCode() * Time.deltaTime);
		for (int i = 0; i < 15; i++) {
			System.Random rand = new System.Random(i * gameObject.name.GetHashCode() * System.Convert.ToInt32(Time.deltaTime));
			//realWaterLevelDiff = rand.Next(-10, 11);
			//int dampen = rand.Next (1, 10);
			//Random.seed = i * Random.seed * gameObject.name.GetHashCode();
			actionPoint = transform.position + transform.TransformDirection(points[i]);
			forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
			
			if (forceFactor > 0f) {

				if (forceFactor < 1f) {
					forceFactor *= forceFactor;
				}
				liftForce = -(Physics.gravity / 15) * (forceFactor - rigidbody.velocity.y * bounceDamp);

				liftForce *= Random.Range(-.2f, 1f); //System.Convert.ToSingle(rand.NextDouble());
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
