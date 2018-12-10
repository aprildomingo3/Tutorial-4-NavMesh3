using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
    public class NPCConnectedPatrol : MonoBehaviour
    {

        [SerializeField]
        bool _patrolWaiting;

        [SerializeField]
        float _totalWaitTime = 3f;

        [SerializeField]
        float _switchProbability = 0.2f;

        NavMeshAgent _navMeshAgent;
        ConnectedWayPoint _currentWaypoint;
        ConnectedWayPoint _previousWaypoint;



        bool _traveling;
        bool _waiting;
        bool _patrolForward;
        float _waitTimer;
        int _waypointsVisited;

        public void Start()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();

            if (_navMeshAgent == null)
            {
                Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);

            }
            else
            {
                if (_currentWaypoint == null)
                {
                    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                    if (allWaypoints.Length > 0)
                    {
                        while (_currentWaypoint == null)
                        {
                            int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                            ConnectedWayPoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWayPoint>();

                            if (startingWaypoint != null)
                            {
                                _currentWaypoint = startingWaypoint;

                            }
                        }
                    }
                }
                    else
                    {
                        Debug.LogError("Failed to find any waypoints for use in the scene. ");
                    }

                SetDestination();

            }

        }

        void FixedUpdate()
        {
            if (Input.GetKey("escape"))
                Application.Quit();
        }

        public void Update()
        {

            if (_traveling && _navMeshAgent.remainingDistance <= 1.0f)
            {
                _traveling = false;
                _waypointsVisited++;

                if (_patrolWaiting)
                {
                    _waiting = true;
                    _waitTimer = 0f;
                }
                else
                {
                    SetDestination();
                }

            }

            if (_waiting)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= _totalWaitTime)
                {
                    _waiting = false;

                    SetDestination();

                }
            }
        }



        private void SetDestination()
        {
            if (_waypointsVisited > 0)
            {
                ConnectedWayPoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
                _previousWaypoint = _currentWaypoint;
                _currentWaypoint = nextWaypoint;

            }

            Vector3 targetVector = _currentWaypoint.transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _traveling = true;


        }
    }
}
