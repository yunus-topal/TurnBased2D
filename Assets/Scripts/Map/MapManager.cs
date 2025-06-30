using System;
using System.Collections.Generic;
using Helpers;
using Models;
using UnityEngine;

namespace Map
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
        
        private List<List<Node>> _gameMap = new List<List<Node>>();
        private void Start()
        {
            _gameMap = GenerateMap(SaveHelper.CurrentSaveFile.SeedNumber);
            foreach (var floor in _gameMap)
            {
                // place nodes in the map.
                var newFloor = Instantiate(floorPrefab, mapAnchor);

                for (var i = 0; i < floor.Count; i++)
                {
                    var node = Instantiate(nodePrefab, newFloor.transform);
                    node.GetComponent<MapNodeHelper>().Initialize(floor[i].type);
                }
            }
            
        }

        private List<List<Node>> GenerateMap(int seed, int floors = 7, int minNodes = 1, int maxNodes = 4) {
            var rng = new System.Random(seed);
            var map = new List<List<Node>>();

            for (int floor = 0; floor < floors; floor++) {
                // 1 node for first and last floor.
                int nodeCount = (floor == 0 || floor == floors - 1) ? 1 : rng.Next(minNodes, maxNodes + 1);
                
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
    }
}
