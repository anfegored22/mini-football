using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreGoal : MonoBehaviour
{
    int goals = 0;
    int not_goals = 0;
    [SerializeField] GameObject environment;
    [SerializeField] GameObject field;
    [SerializeField] Material winMaterial;
    [SerializeField] Material loseMaterial;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            goals++;
            Debug.Log("goals: " + goals);
            field.GetComponent<MeshRenderer>().material = winMaterial;
            environment.GetComponent<FootballPlayer>().AddReward(1f);
            environment.GetComponent<FootballPlayer>().EndEpisode();
            // RestartBallPosition();
        }

        else if(other.gameObject.CompareTag("Not Goal"))
        {
            not_goals++;
            Debug.Log("not_goals: " + not_goals);
            field.GetComponent<MeshRenderer>().material = loseMaterial;
            environment.GetComponent<FootballPlayer>().AddReward(0.05f);
            environment.GetComponent<FootballPlayer>().EndEpisode();
            // RestartBallPosition();
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

}
