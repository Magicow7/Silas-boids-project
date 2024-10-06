// N-body Starter Code

// Fall 2024. IMDM 327

// Instructor. Myungin Lee

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Animations;

public class ThreeBody : MonoBehaviour

{

    private const float G = 500f;

    GameObject[] body;

    BodyProperty[] bp;

    public int numberOfSphere = 50;

    TrailRenderer trailRenderer;

    struct BodyProperty // why struct?

    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct

        public float mass;

        public Vector3 velocity;

        public Vector3 acceleration;
        

    }



    void Start()

    {

        // Just like GO, computer should know how many room for struct is required:

        bp = new BodyProperty[numberOfSphere];

        body = new GameObject[numberOfSphere];


        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)

        for (int i = 0; i < numberOfSphere; i++)

        {          

            // Our gameobjects are created here:

            body[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.

            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html


            // initial conditions

            float r = 50f;

            // position is (x,y,z). In this case, I want to plot them on the circle with r


            // ******** Fill in this part ********


            //body[i].transform.position = new Vector3( Random.Range(10,-10), Random.Range(10,-10), 180);
            float temp = Random.Range(0f,1f);
            temp *= Mathf.PI * 2;
            body[i].transform.position = new Vector3(Mathf.Cos(temp) * r, Mathf.Sin(temp) * r, 180);

            // z = 180 to see this happen in front of me. Try something else (randomize) too.


            bp[i].velocity = new Vector3(Random.Range(-5,5),Random.Range(-5,5),0); // Try different initial condition

            bp[i].mass = 1; // Simplified. Try different initial condition


            // Init Trail

            trailRenderer = body[i].AddComponent<TrailRenderer>();

            // Configure the TrailRenderer's properties

            trailRenderer.time = 100.0f;  // Duration of the trail

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

        for (int i = 0; i < numberOfSphere; i++)

        {

            for (int j = i + 1; j < numberOfSphere; j++)

            {

                // Gravity = G * m1 * m2 / (distance^2). So we need G, m1, m2, distance, + direction

                float m1 = bp[i].mass;

                float m2 = bp[j].mass;


                // Let's say we get the vector from i to j body. Make sure which vector you are getting.

                // ******** Fill in this part ********

                Vector3 distance = body[j].transform.position - body[i].transform.position;
                //Debug.Log(distance);


                // Gravity. Finish the CalculateGravity function

                Vector3 gravity = CalculateGravity(distance, m1, m2);



                // Apply Gravity

                // F = ma -> a = F/m

                // Gravity is push and pull with same amount. Force: m1 <-> m2

                // ******** Fill in this part ********

                bp[i].acceleration += gravity/bp[i].mass; //

                bp[j].acceleration -= gravity/bp[j].mass; // What decides the direction?

            }

            // velocity is sigma(Acceleration*time)

            bp[i].velocity += bp[i].acceleration * Time.deltaTime;
            //if(bp[i].velocity.magnitude > 50){
                //bp[i].velocity -= bp[i].acceleration * Time.deltaTime;
            //}

            // position is sigma(velocity*time)

            body[i].transform.position += bp[i].velocity * Time.deltaTime;

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

}

/*
public class ThreeBody : MonoBehaviour

{

    private const float G = 500f;

    GameObject[] body;

    BodyProperty[] bp;

    private int numberOfSphere = 50;

    TrailRenderer trailRenderer;

    struct BodyProperty // why struct?

    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct

        public float mass;

        public Vector3 velocity;
        public Vector3 orbitalVelocity;
        public float radius;

        public Vector3 acceleration;

    }



    void Start()

    {

        // Just like GO, computer should know how many room for struct is required:

        bp = new BodyProperty[numberOfSphere];

        body = new GameObject[numberOfSphere];
        //Sun
        bp[0].mass = 1989100000E+21f;
        bp[0].radius = 0;
        bp[0].velocity = Vector3.zero;

        //mercury
        bp[1].mass = 0.33E24f;
        bp[1].radius = 57.9E6f;
        bp[1].orbitalVelocity = 47.4f;

        //venus
        bp[2].mass = 0.33E24f;
        bp[2].radius = 57.9E6f;
        bp[2].orbitalVelocity = 47.4f;

        //earth
        bp[3].mass = 0.33E24f;
        bp[3].radius = 57.9E6f;
        bp[3].orbitalVelocity = 47.4f;

        //mars
        bp[4].mass = 0.33E24f;
        bp[4].radius = 57.9E6f;
        bp[4].orbitalVelocity = 47.4f;

        //jupiter
        bp[5].mass = 0.33E24f;
        bp[5].radius = 57.9E6f;
        bp[5].orbitalVelocity = 47.4f;

        //saturn
        bp[6].mass = 0.33E24f;
        bp[6].radius = 57.9E6f;
        bp[6].orbitalVelocity = 47.4f;

        //uranus
        bp[7].mass = 0.33E24f;
        bp[7].radius = 57.9E6f;
        bp[7].orbitalVelocity = 47.4f;

        //neptunre
        bp[8].mass = 0.33E24f;
        bp[8].radius = 57.9E6f;
        bp[8].orbitalVelocity = 47.4f;

        


        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)

        for (int i = 0; i < numberOfSphere; i++)

        {          

            // Our gameobjects are created here:

            body[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); // why sphere? try different options.

            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html


            // initial conditions

            float r = 100f;

            // position is (x,y,z). In this case, I want to plot them on the circle with r


            // ******** Fill in this part ********

            body[i].transform.position = new Vector3( r*Mathf.Cos(Mathf.PI*2/numberOfSphere), r*Mathf.Sin(Mathf.PI*2/numberOfSphere) , 180);

            // z = 180 to see this happen in front of me. Try something else (randomize) too.


            //float theta = Mathf.Pi.....
            bp[i].velocity = new Vector3(0,0,0); // Try different initial condition

            bp[i].mass = 1; // Simplified. Try different initial condition


            // Init Trail

            trailRenderer = body[i].AddComponent<TrailRenderer>();

            // Configure the TrailRenderer's properties

            trailRenderer.time = 100.0f;  // Duration of the trail

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

        for (int i = 0; i < numberOfSphere; i++)

        {

            for (int j = i + 1; j < numberOfSphere; j++)

            {

                // Gravity = G * m1 * m2 / (distance^2). So we need G, m1, m2, distance, + direction

                float m1 = bp[i].mass;

                float m2 = bp[j].mass;


                // Let's say we get the vector from i to j body. Make sure which vector you are getting.

                // ******** Fill in this part ********

                // Vector3 distance = **** - ****;


                // Gravity. Finish the CalculateGravity function

                // Vector3 gravity = CalculateGravity(distance, m1, m2);



                // Apply Gravity

                // F = ma -> a = F/m

                // Gravity is push and pull with same amount. Force: m1 <-> m2

                // ******** Fill in this part ********

                //bp[i].acceleration ** ****; //

                //bp[j].acceleration ** ****; // What decides the direction?

            }

            // velocity is sigma(Acceleration*time)

            bp[i].velocity += bp[i].acceleration * Time.deltaTime;

            // position is sigma(velocity*time)

            body[i].transform.position += bp[i].velocity * Time.deltaTime;

        }

        /*
        for(int i = 0; i < body.Length; ++i){

        }
        /
    }


    // Gravity Fuction to finish

    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)

    {

        Vector3 gravity; // note this is also Vector3

       // **** Fill in the function below. Gravity = G * m1 * m2 / (distance^2). So we need G, m1, m2, distance, + direction

        // gravity = ****;

        return gravity;

    }
    

}
*/
