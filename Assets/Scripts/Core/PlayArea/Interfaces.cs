// using System;
// using System.Collections.Generic;
// using Model.Mechanics;
// using UnityEngine;
//
// namespace Core.PlayArea{
//
//     public abstract class PlayableObjectViewBase: MonoBehaviour{
//         
//     }
//
//     public interface ISliceable{
//         public float Radius{ get; }
//         public void OnSliced();
//     }
//
//     public interface ICircleable{
//         public Collider2D Collider2D{ get; }
//         public void OnCharged();
//     }
//
//     public interface IMovable{
//         Vector2 Velocity{ get; set; }
//         public void UpdatePosition();
//     }
//     
//     
// }