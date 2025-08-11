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
		public MapNodeHelper view;
        public double nodeSeed;
    }
    
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private Transform mapAnchor;
		[SerializeField] private GameObject linePrefab;
        [SerializeField] private ScrollViewHelper scrollView;
         
        [Tooltip("Distance between each row of nodes in the map.")]
        [SerializeField] private float floorDistance = 50f;
        [Tooltip("Distance between each column of nodes in a floor.")]
        [SerializeField] private float nodeDistance = 50f;
        
        private List<List<Node>> _gameMap = new List<List<Node>>();
        private void Start()
        {
            _gameMap = GenerateMap(SaveHelper.CurrentSaveFile.SeedNumber);
            for (var floorIndex = 0; floorIndex < _gameMap.Count; floorIndex++)
            {
                var floor = _gameMap[floorIndex];
                // place nodes in the map.
                var newFloor = Instantiate(floorPrefab, mapAnchor);

                for (var i = 0; i < floor.Count; i++)
                {
                    var node = floor[i];
                    var go = Instantiate(nodePrefab, newFloor.transform);
                    go.GetComponent<MapNodeHelper>().Initialize(node.type, floorIndex, i, node.nodeSeed);
                    node.view = go.GetComponent<MapNodeHelper>();
                }
                // TODO: implement this properly in the future.
                //CreateConnections(floor);
            }
            
            UpdateMapState();
            StartCoroutine(scrollView.SmoothScrollToBottom());
        }

        private List<List<Node>> GenerateMap(int seed, int floors = 7, int minNodes = 1, int maxNodes = 4)
        {
            var rng = Constants.Rng;
            var map = new List<List<Node>>();

            for (int floor = 0; floor < floors; floor++) {
                // 1 node for first and last floor.
                int nodeCount = (floor == 0 || floor == floors - 1) ? 1 : rng.Next(minNodes, maxNodes + 1);
                
                var layer = new List<Node>();

                for (int i = 0; i < nodeCount; i++) {
                    Node node = new();
                    node.nodeSeed = rng.NextDouble();
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

        void CreateConnections(List<Node> floor)
        {
            var container = new GameObject();
            container.transform.parent = mapAnchor;
            foreach (var node in floor)
            {
                foreach (var connection in node.connections)
                {
                    var line =  Instantiate(linePrefab, container.transform);
                }
            }
        }
        void DrawLine(RectTransform from, RectTransform to)
        {
            Vector3 startPos = from.position;
            Vector3 endPos = to.position;

            Vector3 dir = (endPos - startPos).normalized;
            float distance = Vector3.Distance(startPos, endPos);

            GameObject line = Instantiate(linePrefab, mapAnchor); // must be under the same Canvas
            RectTransform rt = line.GetComponent<RectTransform>();
            rt.position = startPos;
            rt.sizeDelta = new Vector2(distance, 4); // width, height
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

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

        public void UpdateMapState()
        {
            // check the current floor number. set interactable true for next floor node buttons.
            var floorNumber = SaveHelper.CurrentSaveFile.floorNumber;

            // player just started the game. only first node should be available.
            if (floorNumber == -1)
            {
                DisableAllNodes();
                _gameMap[0][0].view.SetNodeInteractable(true);
                return;
            }
            
            if (floorNumber < 0 || floorNumber >= _gameMap.Count)
            {
                Debug.LogError("floor number is wrong!");
                return;
            }
            
            var nodeNumber = SaveHelper.CurrentSaveFile.nodeNumber;
            if (nodeNumber < 0 || nodeNumber >= _gameMap[floorNumber].Count)
            {
                Debug.LogError("node number is wrong!");
                return;
            }
            
            int nextFloor = floorNumber + 1;
            if(nextFloor >= _gameMap[floorNumber].Count) return;

            // if there is a next floor availabe, make them interactable.
            DisableAllNodes();
            foreach (var node in _gameMap[nextFloor])
            {
                node.view.SetNodeInteractable(true);
            }
        }

        private void DisableAllNodes()
        {
            foreach (var floor in _gameMap)
            {
                foreach (var node in floor)
                {
                    node.view.SetNodeInteractable(false);
                }
            }
        }
    }
}
