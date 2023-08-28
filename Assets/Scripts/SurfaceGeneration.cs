using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SurfaceGeneration : MonoBehaviour
{
    public GameObject island;
    public GameObject bridge;
    public GameObject edge;
    public GameMaster gm;
    public GameObject skeleton;
    public GameObject bird;
    public GameObject largeStage;
    public GameObject wall;
    public float originalSize;
    public float border = 88.0f;
    public float oldBorder = 88.0f;
    public float viewEdge = 0.0f;
    // public float scanBounds = 880f;
    public float scanBounds = 180f;
    public float xOfNextLargeStage = 250f;
    public GameObject mostRecentWall;
    public Vector3 priviousPos = new Vector3(0,3.72f,0);
    public bool buildBridge = false;
    private GameObject priviousIsland = null;
    private Camera cam;

    void Awake(){
        cam = Camera.main;
    }

    void Start(){
        viewEdge = cam.orthographicSize * Screen.width / Screen.height;
        originalSize = island.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.player != null){
            if(gm.player.transform.position.x >= oldBorder){
                
                //setting values for newIsland
                bool spawingLarge = false;
                Quaternion rotation = new Quaternion(0,0,0,0);
                GameObject newIsland;
                float xValue = 0f;
                float yValue = 0f;
                if (gm.player.transform.position.x <= xOfNextLargeStage){
                    newIsland = island;
                    float ogScale = island.transform.localScale.x;
                    newIsland.transform.localScale = new Vector3(0.1f*(UnityEngine.Random.value + 1.5f), 0.1f*(UnityEngine.Random.value + 0.75f), 0.1f);
                    xValue = border + viewEdge + (newIsland.GetComponent<SpriteRenderer>().bounds.size.x/2);
                    yValue = priviousPos.y + (10 * (UnityEngine.Random.value - 0.5f));
                    while(yValue <= -4f || yValue >= 15f) {
                        yValue = priviousPos.y + (10 * (UnityEngine.Random.value - 0.5f));
                    }    
                }
                else {
                    spawingLarge = true;
                    xOfNextLargeStage += (((UnityEngine.Random.value + 1) * 50) + 150);
                    newIsland = largeStage;
                    xValue = xValue = border + viewEdge + 15f;
                    yValue = -3.4f;
                }
                Vector3 location = new Vector3(xValue, yValue, 0);
                GameObject myIsland = Instantiate(newIsland, location, rotation);
                AIRedrawing(myIsland);
                //spawn enemies
                if (!spawingLarge){    
                    if(newIsland.GetComponent<SpriteRenderer>().bounds.size.x >= 22f){
                        //Debug.Log("trying to spawn skeleton...");
                        Instantiate(skeleton, location + new Vector3(0,newIsland.GetComponent<SpriteRenderer>().bounds.size.y,0), rotation);
                        //if(!buildBridge){
                        Vector3 nearPos = new Vector3(-0.9f * (newIsland.GetComponent<SpriteRenderer>().bounds.size.x/2),newIsland.GetComponent<SpriteRenderer>().bounds.size.y/2,0);
                        Instantiate(edge, location + nearPos, rotation);
                        //}
                        Vector3 farPos = new Vector3(0.9f * (newIsland.GetComponent<SpriteRenderer>().bounds.size.x/2),newIsland.GetComponent<SpriteRenderer>().bounds.size.y/2,0);
                        Instantiate(edge, location + farPos, rotation);
                    }
                    float ranNum = UnityEngine.Random.value;
                    if(newIsland.GetComponent<SpriteRenderer>().bounds.size.x >= 17f && ranNum >= 0.66667f){
                        Instantiate(bird, location + new Vector3(0,16.2f,0), rotation);
                    }
                }
                //building a bridge if nessesary
                if (buildBridge && !spawingLarge && priviousIsland.GetComponent<SpriteRenderer>() != null){
                    bridgeConstruction(
                        priviousPos,
                        priviousIsland.GetComponent<SpriteRenderer>().bounds.size.x,
                        priviousIsland.GetComponent<SpriteRenderer>().bounds.size.y,
                        location,
                        newIsland.GetComponent<SpriteRenderer>().bounds.size.x,
                        newIsland.GetComponent<SpriteRenderer>().bounds.size.y);
                }

                //building a wall if nessesary
                if (priviousIsland == largeStage){
                    Vector3 nearPos = new Vector3(-0.9f * (newIsland.GetComponent<SpriteRenderer>().bounds.size.x/2),newIsland.GetComponent<SpriteRenderer>().bounds.size.y/2,0);
                    mostRecentWall = Instantiate(wall, location + nearPos, rotation);
                }
                //adding to score / difficulty
                gm.player.stats.curScore += 10;
                gm.enemyDifficultyModifier += 0.1f;
                Debug.Log("Why is this always 1: " + gm.enemyDifficultyModifier);

                //reset the values 
                priviousIsland = newIsland;
                priviousPos = location;
                float distanceBetween = UnityEngine.Random.value*15;
                if (distanceBetween >= 6f){
                    buildBridge = true;
                }
                oldBorder = border;
                if (!spawingLarge){
                    border += (newIsland.GetComponent<SpriteRenderer>().bounds.size.x + distanceBetween);
                }
                else {
                    border += (34 + distanceBetween);
                }
            }
        }
    }
    public void bridgeConstruction(Vector3 oldPos, float oldWidth, float oldHight, Vector3 newPos, float newWidth, float newHight) {

        //position math 
        float yDif = ((newPos.y + (newWidth/2)) - (oldPos.y + (oldWidth/2)));
        float distBetween = (newPos.x - (newWidth/2)) - (oldPos.x + (oldWidth/2));
        newPos = newPos - new Vector3((newWidth/2) + (distBetween/2),yDif/2,0);
        //angle math
        Quaternion angle = Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan(yDif/distBetween));
        //generate bridge
        GameObject newBridge = Instantiate(bridge, newPos, angle);
        //scale math 
        float diagDist = Mathf.Sqrt(Mathf.Pow(yDif, 2) + Mathf.Pow(distBetween, 2)) * 1.1f;
        int repetes = 0;
        while(newBridge.GetComponent<SpriteRenderer>().bounds.size.x <= (diagDist - 0.1) || newBridge.GetComponent<SpriteRenderer>().bounds.size.x >= (diagDist + 0.1)){
            if (newBridge.GetComponent<SpriteRenderer>().bounds.size.x <= (diagDist - 0.1)){
                newBridge.transform.localScale = new Vector3(newBridge.transform.localScale.x + 0.001f, newBridge.transform.localScale.y + 0.001f, 0);
            }
            else{
                newBridge.transform.localScale = new Vector3(newBridge.transform.localScale.x - 0.001f, newBridge.transform.localScale.y - 0.001f, 0);
            }
            repetes ++;
            if (repetes >= 3000) {
                Debug.Log("would have crashed...");
                break;
            }
        }
        buildBridge = false;
    }

    public void AIRedrawing(GameObject aIsland){
        
        //Debug.Log("trying to redraw" + aIsland.GetComponent<PolygonCollider2D>().bounds);
        if (gm.player.transform.position.x >= scanBounds){
            scanBounds += 500f;
            AstarData data = AstarPath.active.data;
            //GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
            GridGraph gg = AstarPath.active.data.gridGraph;
            int width = 700;
            int depth = 60;
            int nodeSize = 1;
            gg.center = new Vector3(gm.player.transform.position.x + 200, 0, 0);
            Debug.Log("Graph center should be " + gm.player.transform.position.x);
            gg.SetDimensions(width, depth, nodeSize);
            //gg.rotation = new Vector3(90, 0, 0);
            //gg.Use2DPhysics = true;
            //gg.2D = true;
            AstarPath.active.Scan();
            // AstarData data = AstarPath.active.data;
            // GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
            // gg.RelocateNodes(new Vector3(scanBounds * 2, 0, 0), new Quaternion(0,0,0,0), 1, 1, 0);
            // AstarPath.active.Scan();
            // Debug.Log("has it relocated?");
        }
        else{
            if (aIsland.GetComponent<PolygonCollider2D>() != null){
                AstarPath.active.UpdateGraphs(aIsland.GetComponent<PolygonCollider2D>().bounds);
            }
            else {
                foreach (Transform child in aIsland.transform){
                    if(child.gameObject.layer == 10){
                        AstarPath.active.UpdateGraphs(child.gameObject.GetComponent<PolygonCollider2D>().bounds);
                    }
                }
            }
        }
    }

    public void DestroyWall(){
        Destroy(mostRecentWall.gameObject);
        gm.player.stats.curScore += 50;
    }

    public void SetWallCounter(float keysCollected, float keysRequired){
        RectTransform progressBar = mostRecentWall.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        progressBar.localScale = new Vector3(progressBar.localScale.x, (keysRequired - keysCollected)/keysRequired, progressBar.localScale.z);
    }
}