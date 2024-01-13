﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class FieldOfView : MonoBehaviour {

	[SerializeField] private float viewRadius;
	[SerializeField, Range(0,360)] private float viewAngle;

	[SerializeField] private LayerMask targetMask;
	[SerializeField] private LayerMask obstacleMask;

	[SerializeField] private float meshResolution;
	[SerializeField] private  int edgeResolveIterations;
	[SerializeField] private  float edgeDstThreshold;

	[SerializeField] private  float maskCutawayDst = .1f;

	[SerializeField] private MeshFilter viewMeshFilter;
	
	private List<Transform> _visibleTargets = new List<Transform>();
	private Mesh _viewMesh;

	public float ViewRadius
	{
		get => viewRadius;
		set
		{
			if (value < 0) 
				throw new ArgumentException("View Radius cannot be below zero.");
			viewRadius = value;
		}
	}

	public float ViewAngle
	{
		get => viewAngle;
		set
		{
			if (value < 0 || 360 < value) 
				throw new ArgumentException("View Angle have to be in 0 to 360");
			viewAngle = value;
		}
	}

	public IReadOnlyList<Transform> VisibleTargets => _visibleTargets;

	private void Start() 
	{
		_viewMesh = new Mesh ();
		_viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = _viewMesh;

		StartCoroutine (FindTargetsWithDelay(.2f));
	}

	private IEnumerator FindTargetsWithDelay(float delay) 
	{
		while (true) 
		{
			yield return new WaitForSeconds (delay);
			FindVisibleTargets();
		}
	}

	private void LateUpdate() 
	{
		DrawFieldOfView ();
	}

	private void FindVisibleTargets() 
	{
		_visibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) 
		{
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) 
			{
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) 
				{
					_visibleTargets.Add (target);
				}
			}
		}
	}

	private void DrawFieldOfView() 
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3> ();
		ViewCastInfo oldViewCast = new ViewCastInfo ();
		
		for (int i = 0; i <= stepCount; i++) 
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast (angle);

			if (i > 0) 
			{
				bool edgeDstThresholdExceeded = Mathf.Abs (oldViewCast.Dst - newViewCast.Dst) > edgeDstThreshold;
				if (oldViewCast.Hit != newViewCast.Hit || (oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded)) 
				{
					EdgeInfo edge = FindEdge (oldViewCast, newViewCast);
					
					if (edge.PointA != Vector3.zero) 
					{
						viewPoints.Add (edge.PointA);
					}
					if (edge.PointB != Vector3.zero) 
					{
						viewPoints.Add (edge.PointB);
					}
				}

			}

			viewPoints.Add (newViewCast.Point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount-2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) 
		{
			vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]) + Vector3.forward * maskCutawayDst;

			if (i < vertexCount - 2) 
			{
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		_viewMesh.Clear ();

		_viewMesh.vertices = vertices;
		_viewMesh.triangles = triangles;
		_viewMesh.RecalculateNormals ();
	}


	private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) 
	{
		float minAngle = minViewCast.Angle;
		float maxAngle = maxViewCast.Angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++) 
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast (angle);

			bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.Dst - newViewCast.Dst) > edgeDstThreshold;
			if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded) 
			{
				minAngle = angle;
				minPoint = newViewCast.Point;
			} 
			else 
			{
				maxAngle = angle;
				maxPoint = newViewCast.Point;
			}
		}
		return new EdgeInfo (minPoint, maxPoint);
	}


	private ViewCastInfo ViewCast(float globalAngle) 
	{
		Vector3 dir = DirFromAngle (globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast (transform.position, dir, out hit, viewRadius, obstacleMask)) 
		{
			return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
		}
		return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) 
	{
		if (!angleIsGlobal) 
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo 
	{
		public bool Hit;
		public Vector3 Point;
		public float Dst;
		public float Angle;

		public ViewCastInfo(bool hit, Vector3 point, float dst, float angle) 
		{
			Hit = hit;
			Point = point;
			Dst = dst;
			Angle = angle;
		}
	}

	public struct EdgeInfo 
	{
		public Vector3 PointA;
		public Vector3 PointB;

		public EdgeInfo(Vector3 pointA, Vector3 pointB) {
			PointA = pointA;
			PointB = pointB;
		}
	}

}