using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Octree
{
    public class OctreeNode
    {
        private Bounds _nodeBounds;
    
        private float _minSize;
        private Bounds[] _childBounds;
        private OctreeNode[] _children;
        public List<Boid> boidsInRegion;
        private bool _isIntersected = false;
        public OctreeNode(Bounds bounds, float minSize, List<Boid> boidsInRegion)
        {
            _nodeBounds = bounds;
            _minSize = minSize;

            float quarter = _nodeBounds.size.x / 4.0f;
            float childLength = _nodeBounds.size.y / 2.0f;

            Vector3 childSize = new Vector3(childLength, childLength, childLength);
            _childBounds = new Bounds[8];
        
            _childBounds[0] = new Bounds(_nodeBounds.center + new Vector3(-quarter, quarter, -quarter), childSize);
            _childBounds[1] = new Bounds(_nodeBounds.center + new Vector3(quarter, quarter, -quarter), childSize);
            _childBounds[2] = new Bounds(_nodeBounds.center + new Vector3(-quarter, quarter, quarter), childSize);
            _childBounds[3] = new Bounds(_nodeBounds.center + new Vector3(quarter, quarter, quarter), childSize);
            _childBounds[4] = new Bounds(_nodeBounds.center + new Vector3(-quarter, -quarter, -quarter), childSize);
            _childBounds[5] = new Bounds(_nodeBounds.center + new Vector3(quarter, -quarter, -quarter), childSize);
            _childBounds[6] = new Bounds(_nodeBounds.center + new Vector3(-quarter, -quarter, quarter), childSize);
            _childBounds[7] = new Bounds(_nodeBounds.center + new Vector3(quarter, -quarter, quarter), childSize);

            this.boidsInRegion = boidsInRegion;
        }

        public void ClearTree()
        {
            _children = null;
            boidsInRegion.Clear();
            // _isIntersected = false;
        }
        public void UpdateTree(Boid boid)
        {
            if (_nodeBounds.size.y <= _minSize)
            {
                return;
            }
            if (_children is null)
            {
                _children = new OctreeNode[8];
            }
        
            bool dividing = false;
            for (int i = 0; i < 8; i++)
            {
                if (_children[i] == null)
                {
                    _children[i] = new OctreeNode(_childBounds[i], _minSize, new List<Boid>());
                }
            
                if (_childBounds[i].Contains(boid.transform.position))
                {
                    dividing = true;
                    boidsInRegion.Remove(boid);
                    _children[i].boidsInRegion.Add(boid);
                    _children[i].UpdateTree(boid);
                    break;
                }
            }

            if (!dividing)
            {
                _children = null;
            }
        }
    

        public void FindBoundsIntersectBounds(Bounds bounds, List<Boid> intersectionBounds)
        {
            _isIntersected = false;
            if (!_nodeBounds.Intersects(bounds))
            {
                return;
            }
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    if (child is null)
                    {
                        continue;
                    }
                    child.FindBoundsIntersectBounds(bounds, intersectionBounds);
                }
            }
            else
            {
                intersectionBounds.AddRange(boidsInRegion);
                _isIntersected = true;
                return;
            }
        }
        public void FindBoundsIntersectSphere(Vector3 sphereCenter, float radius, ref HashSet<Boid> neighbours)
        {
            if (!IntersectSphere(sphereCenter, radius))
            {
                return;
            }
        
            if (_children != null)
            {
                _isIntersected = false;
                foreach (var child in _children)
                {
                    if (child is null)
                    {
                        continue;
                    }
                    child.FindBoundsIntersectSphere(sphereCenter, radius, ref neighbours);
                }
            }
            else
            {
                neighbours.AddRange(boidsInRegion);

                _isIntersected = true;
                return;
            }
        }

        private bool IntersectSphere(Vector3 sphereCenter, float radius)
        {
            return _nodeBounds.Contains(sphereCenter) 
                   || SqrDistanceTo(sphereCenter) <= (radius * radius);
        }
    
        private float SqrDistanceTo(Vector3 point)
        {
            return (_nodeBounds.ClosestPoint(point) - point).sqrMagnitude;
        }
        public void Draw()
        {
            // if (boidsInRegion.Count > 0)
            // {
            //     Gizmos.color = Color.red;
            // }
            if (_isIntersected)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = new Color(0, 1, 0);
            }
        
            Gizmos.DrawWireCube(_nodeBounds.center, _nodeBounds.size);
        
            if (_children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (_children[i] != null)
                    {
                        _children[i].Draw();
                    }
                }
            }
        }
    
    }
}
