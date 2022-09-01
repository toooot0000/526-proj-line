using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic{

    public Color lineColor = Color.red;
    public float width = 10f;
    
    public List<Vector2> points = null;
    public float tolerance = 1;
    
    protected override void OnPopulateMesh(VertexHelper vh){
        vh.Clear();
        LineUtility.Simplify(points, tolerance, points);
        if (points?.Count < 2) return;
        for (var i = 0; i < points?.Count - 1; i++){
            AddLine(points[i], points[i + 1], vh); 
        }
        
    }

    private void AddLine(Vector2 v1, Vector2 v2, VertexHelper vh){
        var vtx = UIVertex.simpleVert;
        vtx.color = lineColor;
        var verInd = vh.currentVertCount;
        var offset = (v2 - v1).Rotate(90).normalized * width / 2;
        var v11 = v1 - offset;
        var v12 = v1 + offset;
        var v21 = v2 - offset;
        var v22 = v2 + offset;
        vtx.position = v11;
        vh.AddVert(vtx);
        vtx.position = v12;
        vh.AddVert(vtx);
        vtx.position = v21;
        vh.AddVert(vtx);
        vtx.position = v22;
        vh.AddVert(vtx);
        vh.AddTriangle(verInd, verInd + 1, verInd + 2);
        vh.AddTriangle(verInd + 3, verInd + 2, verInd + 1);
    }
    
}
