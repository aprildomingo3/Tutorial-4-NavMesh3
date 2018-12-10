﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{

    public class ConnectedWayPoint : Waypoint
    {

        [SerializeField]
        protected float _connectivityRadius = 50f;

        List<ConnectedWayPoint> _connections;   

        public void Start()
        {
            GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

            _connections = new List<ConnectedWayPoint>();

            for(int i = 0; i < allWaypoints.Length; i++)
            {
                ConnectedWayPoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWayPoint>();

                if(nextWaypoint != null)
                {
                    if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectivityRadius && nextWaypoint != this)
                    {
                        _connections.Add(nextWaypoint);
                    }

                }

            }

        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _connectivityRadius);

        }

        public ConnectedWayPoint NextWaypoint(ConnectedWayPoint previousWaypoint)
        {
            if(_connections.Count == 0)
            {

                Debug.LogError("Insufficient waypoint count. ");
                return null;

            }
            else if(_connections.Count == 1 && _connections.Contains(previousWaypoint))
            {

                return previousWaypoint;

            }
            else
            {
                ConnectedWayPoint nextWaypoint;
                int nextIndex = 0;

                do
                {
                    nextIndex = UnityEngine.Random.Range(0, _connections.Count);
                    nextWaypoint = _connections[nextIndex];

                } while (nextWaypoint == previousWaypoint);

                return nextWaypoint;

            }
        } 
    }
}
