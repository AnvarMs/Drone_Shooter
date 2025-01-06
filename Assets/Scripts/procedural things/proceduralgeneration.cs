using System.Collections.Generic;
using UnityEngine;


public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private float random;
    public static bool isMapCreated=false;
    private GameObject surfaceBottom, surfaceLeft, surfaceRight, surfaceUp, surfaceFriend, surfaceBack;
    private int[] possibleValues = { 30, 35, 40, 45, 50, 55, 60 };
    private float leftRightPosition;
    private List<GameObject> cubes;
    public Material wallMeterial, surfaceMaterial;
    public List<Vector3> PlayerSpownPoints;

    

    void Awake()
    {
        cubes = new List<GameObject>();
        PlayerSpownPoints = new List<Vector3>();
        surfaceBottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surfaceLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surfaceRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surfaceUp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surfaceFriend = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surfaceBack = GameObject.CreatePrimitive(PrimitiveType.Cube);

       
    }
    

        public void ClearMap(){
             foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
            cubes.Clear();
            PlayerSpownPoints.Clear();
            SpawnObjects();
        }
    public void GenerateTheMap()
    {
        isMapCreated=true;
        random = possibleValues[UnityEngine.Random.Range(0, possibleValues.Length)];
        leftRightPosition = random / 2;

        // Set surface positions
        surfaceBottom.transform.position = Vector3.zero;
        surfaceLeft.transform.position = new Vector3(-leftRightPosition, 10f, 0f);
        surfaceRight.transform.position = new Vector3(leftRightPosition, 10f, 0f);
        surfaceUp.transform.position = new Vector3(0f, 20f, 0f);
        surfaceFriend.transform.position = new Vector3(0f, 10f, -leftRightPosition);
        surfaceBack.transform.position = new Vector3(0f, 10f, leftRightPosition);
        // Reset surface rotations and scales
        surfaceBottom.transform.rotation = Quaternion.identity;
        surfaceLeft.transform.rotation = Quaternion.identity;
        surfaceRight.transform.rotation = Quaternion.identity;
        surfaceUp.transform.rotation = Quaternion.identity;
        surfaceFriend.transform.rotation = Quaternion.identity;
        surfaceBack.transform.rotation = Quaternion.identity;

        // Set surface scales
        surfaceBottom.transform.localScale = new Vector3(random, 1f, random);
        surfaceLeft.transform.localScale = new Vector3(1f, 20f, random);
        surfaceRight.transform.localScale = new Vector3(1f, 20f, random);
        surfaceFriend.transform.localScale = new Vector3(random, 20f, 1);
        surfaceBack.transform.localScale = new Vector3(random, 20f, 1);
        surfaceUp.transform.localScale = new Vector3(random, 1f, random);

        // Assign the material to each surface
        surfaceBottom.GetComponent<Renderer>().material = surfaceMaterial;
        surfaceLeft.GetComponent<Renderer>().material = wallMeterial;
        surfaceRight.GetComponent<Renderer>().material = wallMeterial;
        surfaceFriend.GetComponent<Renderer>().material = wallMeterial;
        surfaceBack.GetComponent<Renderer>().material = wallMeterial;
        surfaceUp.GetComponent<Renderer>().material = surfaceMaterial;

        SpawnObjects();
    }

    void SpawnObjects()
    {


        int gridSpacing = 5;

        for (int x = (int)-leftRightPosition; x < leftRightPosition; x += gridSpacing)
        {
            for (int z = (int)-leftRightPosition; z < leftRightPosition; z += gridSpacing)
            {
                for (int y = 1; y < 20; y += gridSpacing)
                {
                    int predict = UnityEngine.Random.Range(0, 4);
                    if (predict == 1)
                    {
                        PlayerSpownPoints.Add(new Vector3(x + gridSpacing / 2, y + gridSpacing / 2, z + gridSpacing / 2));

                    }
                    if (predict == 3)
                    {
                        CreateLight(new Vector3(x + gridSpacing / 2, y + gridSpacing / 2, z + gridSpacing / 2));
                    }
                    if (predict != 2) continue;

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x + gridSpacing / 2, y + gridSpacing / 2, z + gridSpacing / 2);
                    cube.transform.localScale = new Vector3(gridSpacing, gridSpacing, gridSpacing);
                    cube.GetComponent<Renderer>().material = wallMeterial;
                    cubes.Add(cube);
                }
            }
        }
    }

    void CreateLight(Vector3 Pos)
    {

        GameObject lightGameObject = new GameObject("Point Light");

        // Add a Light component to the GameObject
        Light lightComp = lightGameObject.AddComponent<Light>();

        // Set the light type to Point
        lightComp.type = UnityEngine.LightType.Point;

        lightGameObject.transform.position = Pos;

        // Optionally, you can customize the light's properties
        lightComp.color = Color.white;
        lightComp.range = 20f;
        lightComp.intensity = 1f;
        cubes.Add(lightComp.gameObject);
    }

     
}
