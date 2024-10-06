// N-body Starter Code

// Fall 2024. IMDM 327

// Instructor. Myungin Lee

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Animations;

public class NewSolarSystem : MonoBehaviour

{

    private const float G = 100;

    public float[] speeds;

    GameObject[] body;

    BodyProperty[] bp;

    public int numberOfSphere = 50;

    public float scaleValue = 0.1f;

    TrailRenderer trailRenderer;

    struct BodyProperty // why struct?

    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct

        public float mass;

        public float distance;

        public float speed;

        public Vector3 velocity;

        public Vector3 acceleration;

        public Vector3 position;

    }



    void Start()

    {

        // Just like GO, computer should know how many room for struct is required:

        bp = new BodyProperty[numberOfSphere];

        body = new GameObject[numberOfSphere];
        // mass is measured in earth masses
        //distance in in millions of kilometers * 10


        //Sun
        bp[0].mass = 333000;
        bp[0].distance = 0;
        bp[0].speed = speeds[0];

        //mercury
        bp[1].mass = 0.055f;
        bp[1].distance = 63.81f;
        bp[1].speed = speeds[1];

        //venus
        bp[2].mass = 0.815f;
        bp[2].distance = 107.59f;
        bp[2].speed = speeds[2];

        
        //earth
        bp[3].mass = 1f;
        bp[3].distance = 151.48f;
        bp[3].speed = speeds[3];
        
        //mars
        bp[4].mass = 0.107f;
        bp[4].distance = 248.84f;
        bp[4].speed = speeds[4];

        //jupiter
        bp[5].mass = 317.8f;
        bp[5].distance = 755.91f;
        bp[5].speed = speeds[5];


        //saturn
        bp[6].mass = 95.160f;
        bp[6].distance = 1487.80f;
        bp[6].speed = speeds[6];

        //uranus
        bp[7].mass = 14.540f;
        bp[7].distance = 2954.60f;
        bp[7].speed = speeds[7];

        //neptune
        bp[8].mass = 17.150f;
        bp[8].distance = 4475.50f;
        bp[8].speed = speeds[8];
        //bp[1].speed = 47.9e3f;*/


        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)

        for (int i = 0; i < numberOfSphere; i++)

        {          

            // Our gameobjects are created here:

            body[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.

            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html


            // initial conditions

            float r = bp[i].distance;

            // position is (x,y,z). In this case, I want to plot them on the circle with r


            // ******** Fill in this part ********


            //body[i].transform.position = new Vector3( Random.Range(10,-10), Random.Range(10,-10), 180);
            float temp = Random.Range(0f,1f);
            temp *= Mathf.PI * 2;
            bp[i].position = new Vector3(r, 0, 180);
            body[i].transform.position = GetDisplayPos(bp[i].position);

            //bp[i].position = new Vector3(Mathf.Cos(temp) * r, Mathf.Sin(temp) * r, 180);

            // z = 180 to see this happen in front of me. Try something else (randomize) too.

            bp[i].velocity = new Vector3(0,bp[i].speed,0); // Try different initial condition

            //bp[i].mass = 1; // Simplified. Try different initial condition


            // Init Trail

            trailRenderer = body[i].AddComponent<TrailRenderer>();

            // Configure the TrailRenderer's properties

            trailRenderer.time = 30 * i;  // Duration of the trail

            trailRenderer.startWidth = 0.5f;  // Width of the trail at the start

            trailRenderer.endWidth = 0.1f;    // Width of the trail at the end

            // a material to the trail

            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));

            // Set the trail color over time

            Gradient gradient = new Gradient();

            gradient.SetKeys(

                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color (Mathf.Cos(Mathf.PI * 2 / numberOfSphere * i), Mathf.Sin(Mathf.PI * 2 / numberOfSphere * i), Mathf.Tan(Mathf.PI * 2 / numberOfSphere * i)), 0.80f) },

                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }

            );

            trailRenderer.colorGradient = gradient;


        }

        //InitialVelocitySetup();

    }


    void Update()

    {

        for (int i = 0; i < numberOfSphere; i++)

        {  

            // Important. Think about where this should be placed

            bp[i].acceleration = Vector3.zero; // what happens if you comment this out?

        }

        // Loop for N-body gravity

        // How should we design the loop?

        for (int i = 1; i < numberOfSphere; i++)

        {

                // Gravity = G * m1 * m2 / (distance^2). So we need G, m1, m2, distance, + direction

                float m1 = bp[i].mass;

                float m2 = bp[0].mass;


                // Let's say we get the vector from i to j body. Make sure which vector you are getting.

                // ******** Fill in this part ********

                Vector3 distance = bp[0].position - bp[i].position;
                //Debug.Log(distance);


                // Gravity. Finish the CalculateGravity function

                Vector3 gravity = CalculateGravity(distance, m1, m2);



                // Apply Gravity

                // F = ma -> a = F/m

                // Gravity is push and pull with same amount. Force: m1 <-> m2

                // ******** Fill in this part ********

                bp[i].acceleration += gravity/bp[i].mass; //

                //bp[j].acceleration -= gravity/bp[0].mass; // What decides the direction?

            

            // velocity is sigma(Acceleration*time)

            bp[i].velocity += bp[i].acceleration * Time.deltaTime;

            // position is sigma(velocity*time)
            bp[i].position += bp[i].velocity * Time.deltaTime;

            /*
            //float temp = Mathf.Sqrt(bp[i].position.magnitude / 1e8f);
            //Debug.Log(temp);
            float scaledDistance = Mathf.Sqrt(bp[i].position.magnitude / 1e8f);
            Vector3 direction = bp[i].position.normalized;
            //Debug.Log(direction + " is direction" + scaledDistance + " is dist");
            //Debug.Log(scaledDistance)
            body[i].transform.position = direction * scaledDistance;
            //body[i].transform.position = bp[i].position*0.0005f;
            */
            body[i].transform.position =  GetDisplayPos(bp[i].position);
        }


    }


    // Gravity Fuction to finish

    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)

    {

        Vector3 gravity; // note this is also Vector3

        float distance = distanceVector.magnitude;
        // Prevent division by zero
        if (distance == 0f){return new Vector3(0,0,0);}

        Vector3 direction = distanceVector.normalized;


       // **** Fill in the function below. Gravity = G * m1 * m2 / (distance^2). So we need G, m1, m2, distance, + direction

        float gravityForce = G * (m1 * m2) / (distance * distance);
        gravity = gravityForce * direction;

        return gravity;

    }
/*
    void InitialVelocitySetup(){
        for (int i = 1; i < numberOfSphere; i++)
        {

            float m2 = bp[0].mass;
            float r = Vector3.Distance(bp[i].position, bp[0].position);
            body[i].transform.LookAt(body[0].transform);

            bp[i].velocity += body[i].transform.right * Mathf.Sqrt((G * m2) / r);
            Debug.Log(bp[i].velocity);

        }
    }
    */

    Vector3 GetDisplayPos(Vector3 abstractPos){
        return new Vector3(abstractPos.x * scaleValue,abstractPos.y * scaleValue, 180);
    }

}

