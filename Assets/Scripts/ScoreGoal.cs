using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreGoal : MonoBehaviour
{
    int goals = 0;
    int not_goals = 0;
    [SerializeField] GameObject environment;
    [SerializeField] GameObject Goal;
    [SerializeField] Material winMaterial;
    [SerializeField] Material loseMaterial;
    GameObject bounds;
    GameObject line;
    GameObject field;

    private void Start()
    {
        bounds = GameObject.Find("Football Field/Glass");
        line = GameObject.Find("Football Field/Line");
        field = GameObject.Find("Football Field/Field");
    }


    private void Update()
    {

        MeshCollider goal_collider = Goal.GetComponent<MeshCollider>();
        if (goal_collider.bounds.Contains(transform.position))
        {
            goals++;
            AddRewardRestart(1f, winMaterial);
        }

        // If ball is out of range, restart game.
        MeshCollider m_collider = bounds.GetComponent<MeshCollider>();
        if (transform.position.z > line.transform.position.z)
        {
            not_goals++;
            AddRewardRestart(0.05f, loseMaterial);
        }
        if (!m_collider.bounds.Contains(transform.position))
        {
            AddRewardRestart(0f, loseMaterial);
        }


    }


    public void RestartBallPosition()
    {
        // Reset Ball Randomly
        float randomNumberX = (float)environment.GetComponent<FootballPlayer>().rnd.NextDouble()*3f; // Random.Range(0f, 3f);
        float randomNumberY = (float)environment.GetComponent<FootballPlayer>().rnd.NextDouble()*0.25f; // Random.Range(0f, 0.25f);
        float randomNumberZ = (float)environment.GetComponent<FootballPlayer>().rnd.NextDouble()*4f; // Random.Range(0f, 4f);

        GetComponent<Transform>().localPosition = new Vector3(
            -1.5f + randomNumberX, 
            0.5f + randomNumberY,
            -2.5f + randomNumberZ);

        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
    }

    void AddRewardRestart(float r, Material m)
    {
        field.GetComponent<MeshRenderer>().material = m;
        environment.GetComponent<FootballPlayer>().AddReward(r);
        environment.GetComponent<FootballPlayer>().EndEpisode();
        RestartBallPosition();
    } 

}
