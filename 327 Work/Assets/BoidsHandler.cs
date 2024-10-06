// N-body Starter Code

// Fall 2024. IMDM 327

// Instructor. Myungin Lee

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Animations;

public class BoidsHandler : MonoBehaviour

{

    private const float G = 500f;

    GameObject[] body;

    BodyProperty[] bp;

    public int numberOfSphere = 50;

    public float speed;

    public float cohesionRadius;
    public float cohesionStrength;
    public float alignmentRadius;
    public float alignmentStrength;
    public float avoidanceRadius;
    public float avoidanceStrength;

    public float centerizeDistThreshold;
    public Vector3 centerPoint;

    public GameObject squawkObject;

    public float minSquawkTime;
    public float maxSquawkTime;

    public bool drawDebugLines;

    TrailRenderer trailRenderer;

    struct BodyProperty // why struct?

    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct

        public Vector3 velocity;

        public Vector3 acceleration;

        public float squawkTimer;
        

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

            if(i == 0){
                body[i].GetComponent<MeshRenderer>().enabled = false;
            }

            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html


            // initial conditions

            float r = 50f;

            // position is (x,y,z). In this case, I want to plot them on the circle with r


            // ******** Fill in this part ********


            //body[i].transform.position = new Vector3( Random.Range(10,-10), Random.Range(10,-10), 180);
            float temp = Random.Range(0f,1f);
            temp *= Mathf.PI * 2;
            body[i].transform.position = new Vector3(Random.Range(-5,5),Random.Range(-5,5), Random.Range(-5,5) + 90);

            // z = 180 to see this happen in front of me. Try something else (randomize) too.


            bp[i].velocity = Random.insideUnitSphere * speed;

            bp[i].squawkTimer = Random.Range((float)minSquawkTime,(float)maxSquawkTime);



            // Init Trail

            if(i != 0){
                 trailRenderer = body[i].AddComponent<TrailRenderer>();

            // Configure the TrailRenderer's properties

            trailRenderer.time = 5.0f;  // Duration of the trail

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

    }


    void FixedUpdate()

    {

        for (int i = 0; i < numberOfSphere; i++)

        {  

            bp[i].acceleration = Vector3.zero;
        }


        for (int i = 0; i < numberOfSphere; i++)
        {
            bp[i].acceleration += Cohesion(body[i]) * cohesionStrength;
            bp[i].acceleration += Alignment(body[i], bp[i].velocity) * alignmentStrength;
            bp[i].acceleration += Avoidance(body[i]) * avoidanceStrength;
            bp[i].acceleration += Recenter(body[i]);

            if(bp[i].squawkTimer <= 0){
                Squawk(body[i].transform.position);
                bp[i].squawkTimer = Random.Range((float)minSquawkTime,(float)maxSquawkTime);
            }else{
                bp[i].squawkTimer -= Time.deltaTime;
            }


            if (drawDebugLines) {
                Debug.DrawLine(body[i].transform.position, body[i].transform.position + bp[i].acceleration, Color.blue);
            }
            

            bp[i].velocity += bp[i].acceleration * Time.deltaTime;

            body[i].transform.position += bp[i].velocity * Time.deltaTime;

        }

        Camera.main.transform.position = body[0].transform.position;
        Camera.main.transform.LookAt(body[0].transform.position + bp[0].velocity);


    }


    // Gravity Fuction to finish

    private Vector3 Cohesion (GameObject a)
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        for(int i = 0; i < numberOfSphere; i++){
            if (body[i] != a && Vector3.Distance(body[i].transform.position,a.transform.position) < cohesionRadius)
            {
                center += body[i].transform.position;
                count++;
            }

        }
         
        if (count > 0)
        {
            center /= count;
            return (center - a.transform.position).normalized * speed;
        }

        return Vector3.zero;

    }

    private Vector3 Alignment(GameObject a, Vector3 v){
        Vector3 alignmentVector = Vector3.zero;
        int count = 0;

        for(int i = 0; i < numberOfSphere; i++)
        {
            if (body[i] != a && Vector3.Distance(a.transform.position, body[i].transform.position) < alignmentRadius)
            {
                alignmentVector += bp[i].velocity;
                count++;
            }
        }

        if (count > 0)
        {
            alignmentVector /= count;
            return (alignmentVector.normalized * speed - v);
        }

        return Vector3.zero;
    }

    private Vector3 Avoidance(GameObject a)
    {
        Vector3 avoidanceVector = Vector3.zero;

        for(int i = 0; i < numberOfSphere; i++)
        {
            
            if (body[i] != a && Vector3.Distance(a.transform.position, body[i].transform.position) < avoidanceRadius)
            {
                avoidanceVector += (a.transform.position - body[i].transform.position);
            }
        }

        return avoidanceVector.normalized * speed;
    }

    private Vector3 Recenter(GameObject a)
    {
        Vector3 centerDirection =  centerPoint - a.transform.position;
        float dist = Vector3.Distance(a.transform.position, centerPoint);

        if(dist > centerizeDistThreshold){
            return centerDirection.normalized * speed;
        }
        return Vector3.zero;
    }

    private void Squawk(Vector3 squawkPosition){
        AudioSource a = Instantiate(squawkObject, squawkPosition, Quaternion.identity).GetComponent<AudioSource>();
        a.pitch = Random.Range(0.8f,1.2f);
        a.volume = Random.Range(0.75f,1f);
    }

}
