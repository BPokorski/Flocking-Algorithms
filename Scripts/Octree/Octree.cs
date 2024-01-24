using System.Collections.Generic;
using UnityEngine;

namespace Octree
{
    public class Octree
    {
        public OctreeNode rootNode;
        public Bounds octreeBounds;
        private List<Boid> _allBoids = new List<Boid>();
        public Octree(float rootNodeSize, float minNodeSize)
        {
            Bounds bounds = new Bounds();
        
            bounds.SetMinMax(Vector3.zero, new Vector3(rootNodeSize, rootNodeSize, rootNodeSize));
            octreeBounds = bounds;
            rootNode = new OctreeNode(bounds, minNodeSize, new List<Boid>());
        }

        public void AddObjects(List<Boid> boids)
        {
            foreach (Boid boid in boids)
            {
                _allBoids.Add(boid);
            }

            UpdateTree();
        }
        public void UpdateTree()
        {
            rootNode.ClearTree();
            foreach (Boid boid in _allBoids)
            {
                rootNode.UpdateTree(boid);
            }
        }

        public Bounds OctreeBounds
        {
            get => octreeBounds;
        }
        public HashSet<Boid> FindNodesIntersectsSphere(Vector3 sphereCenter, float radius)
        {
            HashSet<Boid> foundNeighbours = new HashSet<Boid>();
        
            rootNode.FindBoundsIntersectSphere(sphereCenter, radius, ref foundNeighbours);
            return foundNeighbours;
        }
    }
}
