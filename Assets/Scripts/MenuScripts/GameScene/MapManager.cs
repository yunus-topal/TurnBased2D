using System;
using System.Collections.Generic;
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
        List<List<Node>> GenerateMap(int seed, int floors = 7, int minNodes = 2, int maxNodes = 4) {
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

    }
    
    
}
