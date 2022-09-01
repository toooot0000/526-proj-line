using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core{
    [RequireComponent(typeof(LineRenderer))]
    public class LineColliderGenerator: MonoBehaviour{
        private LineRenderer _lineRenderer;
        private PolygonCollider2D _polygonCollider;
        

        private void Start(){
            _lineRenderer = GetComponent<LineRenderer>();
            _polygonCollider = GetComponent<PolygonCollider2D>();
        }

        public void GenerateCollider(){
            // var mesh = new Mesh();
            // _lineRenderer.BakeMesh(mesh);
            // foreach (var vtx in mesh.vertices){
            //         
            // }
            // var points = new Vector3[_lineRenderer.positionCount];
            // _lineRenderer.GetPositions(points);
            //
        }
        
    }
}