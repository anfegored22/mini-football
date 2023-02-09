using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FootballPlayer : Agent
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject goalSensor;
    [SerializeField] GameObject player;
    [SerializeField] GameObject field;
    [SerializeField] Material maxStepsMaterial;
    Rigidbody rb_player;
    Rigidbody rb_ball;
    int steps = 0;
    public int maxSteps = 1000;
    public System.Random rnd = new System.Random(0); // Environment Random

    // Start is called before the first frame update
    void Start()
    {
        // Game Settings
        Application.targetFrameRate = 60;

        // Environment Settings
        rb_player = player.GetComponent<Rigidbody>();
        rb_ball = ball.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Here we set up the start of each episode
        player.GetComponent<PlayerMovement>().RestartPlayerPosition();
        ball.GetComponent<ScoreGoal>().RestartBallPosition();
        steps = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Environment State
        sensor.AddObservation(player.gameObject.transform.localPosition);
        sensor.AddObservation(ball.gameObject.transform.localPosition);
        sensor.AddObservation(goalSensor.gameObject.transform.localPosition);
        sensor.AddObservation(rb_ball.velocity);
        sensor.AddObservation(rb_ball.angularVelocity);

        //sensor.AddObservation(ball.gameObject.transform.localPosition - goalSensor.gameObject.transform.localPosition);
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get Agent Actions
        int jump = actions.DiscreteActions[0];
        float moveHorizontal = actions.ContinuousActions[0];
        float moveVertical = actions.ContinuousActions[1];

        // Act
        if (jump==1 && player.GetComponent<PlayerMovement>().IsGrounded())
        {
            float jumpForce = player.GetComponent<PlayerMovement>().jumpForce;
            rb_player.velocity = new Vector3(rb_player.velocity.x, jumpForce, rb_player.velocity.z);
        }

        float movementSpeed = player.GetComponent<PlayerMovement>().movementSpeed;
        rb_player.velocity = new Vector3(moveHorizontal * movementSpeed, rb_player.velocity.y, moveVertical * movementSpeed);

        // Each actions cost the agent 
        steps += 1;
        AddReward(-0.0001f);

        if (steps > maxSteps)
        {
            steps = 0;
            field.GetComponent<MeshRenderer>().material = maxStepsMaterial;
            EpisodeInterrupted();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousAction[0] = Input.GetAxis("Horizontal");
        continuousAction[1] = Input.GetAxis("Vertical");
        discreteActions[0] = Input.GetButtonDown("Jump") ? 1 : 0;
    }


}
