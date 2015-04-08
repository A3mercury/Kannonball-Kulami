using UnityEngine;
using System.Collections;

public class Gerstner : MonoBehaviour {
	
	public float waterHeight;
	public Vector4 amplitude;
	public Vector4 frequency;
	public Vector4 steepness;
	public Vector4 speed;
	public Vector4 directionAB;
	public Vector4 directionCD;
	private float timeOffset = 0;
	private Vector3[] oldVert;
	private Vector3[] newVert;
	private Vector3[] vertsToAccess;
	private MeshFilter meshFilter;
	private Mesh newMesh;
	private Mesh mesh;
	private MeshCollider meshCollider;
	private bool first = true;
	
	private Vector3 orgPos;
	
	void Start()
	{
		Debug.Log ("start");
		orgPos = transform.position;
		//BuoyancyShared bs = GameObject.Find("BuoyancyShared").GetComponent<BuoyancyShared>();
		//waterHeight = bs.waterHeight;
		//timeOffset = bs.timeOffset;
		//amplitude = bs.amplitude;
		//frequency = bs.frequency;
		//steepness = bs.steepness;
		//speed = bs.speed;
		//directionAB = bs.directionAB;
		//directionCD = bs.directionCD;
		meshCollider = GetComponent<MeshCollider>();
		meshFilter = GetComponent<MeshFilter>();
		mesh = meshFilter.mesh;
		newMesh = new Mesh();
		//newMesh.triangles = mesh.triangles;
		//newMesh.normals = mesh.normals;
		//newMesh.uv = mesh.uv;
		newMesh.MarkDynamic();
		//mesh.MarkDynamic();
		oldVert = mesh.vertices;
		vertsToAccess = mesh.vertices;
		newVert = new Vector3[oldVert.Length];

	}

	public float getHeightAtVertex(Vector3 v)
	{
		float height = 0;
		float previous = float.MaxValue;
		//for(int i = 0; i < vertsToAccess.Length; i++)
		//{
		//	vertsToAccess[i] = transform.TransformPoint(vertsToAccess[i]);
		//}
		for(int i = 0; i < vertsToAccess.Length; i++)
		{
			float distance = (vertsToAccess[i] - v).sqrMagnitude;
			if(distance < previous)
			{
				previous = distance;
				height = vertsToAccess[i].y;
			}
		}
		return height;
	}

	private float dot(Vector4 A, Vector4 B)
	{
		float one;
		float two;
		float three;
		float four;
		
		one = A.x * B.x;
		two = A.y * B.y;
		three = A.z * B.z;
		four = A.w * B.w;
		
		return one + two + three + four;
	}
	
	private Vector4 VectorMul(Vector4 A, Vector4 B)
	{
		return new Vector4(A.x * B.x, A.y * B.y, A.z * B.z, A.w * B.w);
	}
	
	private float dot(Vector2 A, Vector2 B)
	{
		return (A.x * B.x) + (A.y * B.y);
	}
	
	//private Vector3 Gerst()
	//{
	//Vector3 offsets;
	
	//offsets.x =
	//	steepness * amplitude * dir.x *
	//		cos( freq * dot( dir, xzVtx ) + speed * _Time.x); 
	//
	//offsets.z =
	//	steepness * amp * dir.y *
	//		cos( freq * dot( dir, xzVtx ) + speed * _Time.x); 
	//
	//offsets.y = 
	//	amp * sin ( freq * dot( dir, xzVtx ) + speed * _Time.x);
	//
	//return offsets;	
	//}
	
	private Vector3 Gerst(Vector3 point)
	{
		Vector3 offset;
		
		Vector4 AB = new Vector4(steepness.x * amplitude.x * directionAB.x, steepness.x * amplitude.x * directionAB.y, steepness.y * amplitude.y * directionAB.z, steepness.y * amplitude.y * directionAB.w);
		Vector4 CD = new Vector4(steepness.z * amplitude.z * directionCD.x, steepness.z * amplitude.z * directionCD.y, steepness.w * amplitude.w * directionCD.z, steepness.w * amplitude.w * directionCD.w);
		
		Vector4 ABCD = VectorMul(frequency, new Vector4(dot (new Vector2(directionAB.x, directionAB.y), new Vector2(point.x, point.z)), dot (new Vector2(directionAB.z, directionAB.w), new Vector2(point.x, point.z)),
		                                                dot (new Vector2(directionCD.x, directionCD.y), new Vector2(point.x, point.z)), dot (new Vector2(directionCD.z, directionCD.w), new Vector2(point.x, point.z))));
		Vector4 time = VectorMul(new Vector4(Time.time + timeOffset, Time.time + timeOffset, Time.time + timeOffset, Time.time + timeOffset), speed);
		Vector4 temp = ABCD + time;
		Vector4 cos = new Vector4(Mathf.Cos(temp.x), Mathf.Cos(temp.y), Mathf.Cos(temp.z), Mathf.Cos(temp.w));
		Vector4 sin = new Vector4(Mathf.Sin(temp.x), Mathf.Sin(temp.y), Mathf.Sin(temp.z), Mathf.Sin(temp.w));
		
		offset.x = dot (cos, new Vector4(AB.x, AB.z, CD.x, CD.z));
		offset.z = dot (cos, new Vector4(AB.y, AB.w, CD.y, CD.w));
		offset.y = dot (sin, amplitude);

		return offset;
	}
	
	void FixedUpdate()
	//void GenerateWaves()
	{
		//transform.position = orgPos + Gerst(orgPos);//orgPos + new Vector3(0f, Gerst(orgPos).y, 0f);
		for(int i = 0; i < oldVert.Length; i++) 
		{
			Vector3 offset = Gerst(transform.TransformPoint(oldVert[i]));
			newVert[i] = oldVert[i] + new Vector3(offset.x / transform.localScale.x, offset.y / transform.localScale.y, offset.z / transform.localScale.z);
		}
		newMesh.vertices = newVert;
		if(first)
		{
			newMesh.triangles = mesh.triangles;
			first = false;
		}
		//newMesh.normals = mesh.normals;
		//newMesh.uv = mesh.uv;
		//newMesh.uv1 = mesh.uv1;
		//newMesh.uv2 = mesh.uv2;
		//newMesh.colors = mesh.colors;
		//newMesh.colors32 = mesh.colors32;
		newMesh.RecalculateBounds();
		//newMesh.RecalculateNormals();
		newMesh.name = "test";
		meshFilter.mesh = newMesh;
		for(int i = 0; i < newVert.Length; i++)
		{
			vertsToAccess[i] = transform.TransformPoint(newVert[i]);
		}
		//mesh.RecalculateNormals();
		//meshCollider.sharedMesh = newMesh;
		//meshCollider.sharedMesh.RecalculateBounds();
		//Debug.Log(getHeightAtVertex(Vector3.zero));
	}
}
