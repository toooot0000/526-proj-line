using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.TouchTracking{
    [RequireComponent(typeof(LineRenderer))]
    public class LineColliderGenerator : MonoBehaviour{
        public float widthFactor = 1;
        private bool _isDirty;

        private LineRenderer _lineRenderer;
        private LinkedList<Vector2> _list;
        private PolygonCollider2D _polygonCollider;
        private Transform _transform;


        private void Start(){
            _lineRenderer = GetComponent<LineRenderer>();
            _polygonCollider = GetComponent<PolygonCollider2D>();
            _list = new LinkedList<Vector2>();
            _transform = GetComponent<Transform>();
        }

        public void FixedUpdate(){
            if (_isDirty){
                _polygonCollider.points = _list.ToArray();
                _isDirty = false;
            }
        }

        public void AddPoint(Vector2 worldPosition){
            _list.AddLast(_transform.InverseTransformPoint(worldPosition));
            _isDirty = true;
        }

        public void AddPoint(Vector2 worldPosition, Vector2 dir, float lineWidth){
            if (_list.Count == 0){
                AddPoint(worldPosition);
                return;
            }

            var localPosition = (Vector2)_transform.InverseTransformPoint(worldPosition);
            dir = dir.normalized.Rotated(90);
            var p1 = localPosition + dir * lineWidth / 2 * widthFactor;
            var p2 = localPosition - dir * lineWidth / 2 * widthFactor;
            _list.AddFirst(p1);
            _list.AddLast(p2);
            _isDirty = true;
        }

        public void ClearPoints(){
            _list.Clear();
            _isDirty = true;
        }
    }
}