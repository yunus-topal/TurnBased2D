using System;
using System.Collections.Generic;
using Helpers;
using Models;
using UnityEngine;

namespace MenuScripts.GameScene
{
    class Node {
        public Encounter type;
        public List<Node> connections = new();
    }
    
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private Transform mapAnchor;
        
        [Tooltip("Distance between each row of nodes in the map.")]
        [SerializeField] private float floorDistance = 50f;
        [Tooltip("Distance between each column of nodes in a floor.")]
        [SerializeField] private float nodeDistance = 50f;
        
        private List<List<Node>> map = new List<List<Node>>();
        private void Start()
        {
            map = GenerateMap(SaveHelper.CurrentSaveFile.SeedNumber);
            foreach (var floor in map)
            {
                // place nodes in the map.
                var newFloor = Instantiate(floorPrefab, mapAnchor);

                for (var i = 0; i < floor.Count; i++)
                {
                    var node = floor[i];
                    Instantiate(nodePrefab, newFloor.transform);
                }
            }
            
        }

        private List<List<Node>> GenerateMap(int seed, int floors = 7, int minNodes = 1, int maxNodes = 4) {
            var rng = new System.Random(seed);
            var map = new List<List<Node>>();

            for (int floor = 0; floor < floors; floor++) {
                int nodeCount = rng.Next(minNodes, maxNodes + 1);
                var layer = new List<Node>();

                for (int i = 0; i < nodeCount; i++) {
                    Node node = new();
                    if (floor == 0)
                        node.type = Encounter.Combat;
                    else if (floor == floors - 1)
                        node.type = Encounter.Boss;
                    else
                        node.type = GetRandomEncounter(rng);
                    layer.Add(node);
                }

                map.Add(layer);
            }

            // Generate connections between floors
            for (int floor = 0; floor < floors - 1; floor++) {
                var currentLayer = map[floor];
                var nextLayer = map[floor + 1];

                foreach (var node in currentLayer) {
                    int connections = rng.Next(1, Math.Min(4, nextLayer.Count + 1)); // Up to 3
                    for (int j = 0; j < connections; j++) {
                        var target = nextLayer[rng.Next(nextLayer.Count)];
                        if (!node.connections.Contains(target))
                            node.connections.Add(target);
                    }
                }
            }

            return map;
        }
        
        Encounter GetRandomEncounter(System.Random rng) {
            int roll = rng.Next(100);
            return roll switch {
                < 50 => Encounter.Combat,
                < 70 => Encounter.Rest,
                < 85 => Encounter.Merchant,
                _ => Encounter.MiniBoss,
            };
        }

        // calculate x positions of nodes.
        private List<float> CalculateNodePositions(int count)
        {
            if (count < 1)
            {
                Debug.LogError("node count < 1");
                return new List<float>(count);
            }
            
            if(count == 1) return new List<float>() { 0 };

            var firstNodePosition = -(count / 2) * nodeDistance;
            if (count % 2 == 0) firstNodePosition += nodeDistance / 2;
            
            var positions  = new List<float>();
            for(int i = 0; i < count; i++)
            {
                positions.Add(firstNodePosition);
                firstNodePosition += nodeDistance;
            }
            return positions;
        }
    }
}
