using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    RaycastHit hit;

    public Dictionary<GameObject, Color> originalTreeColors = new Dictionary<GameObject, Color>();
    public Dictionary<GameObject, Color> oldTreeColors = new Dictionary<GameObject, Color>();
    //Dictionary<GameObject, Color> currentTreeColors = new Dictionary<GameObject, Color>();



    // Start is called before the first frame update
    void Start()
    {
        //Debug.unityLogger.logEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;

        foreach (KeyValuePair<GameObject, Color> pair in originalTreeColors)
        {
            Renderer renderer = pair.Key.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = 1f; // Set alpha to 1
                material.color = color;
            }
        }
        changeTreeBlocksScreen();
    }

    void changeTreeBlocksScreen()
    {

        // Calculate the difference between originalTreeColors and oldTreeColors
        Dictionary<GameObject, Color> missingColors = new Dictionary<GameObject, Color>();
        foreach (KeyValuePair<GameObject, Color> pair in oldTreeColors)
        {
            if (!originalTreeColors.ContainsKey(pair.Key))
            {
                missingColors.Add(pair.Key, pair.Value);
            }
        }

        Vector3 direction = (Player.playerPos - Camera.main.transform.position).normalized;
        float distance = Vector3.Distance(Camera.main.transform.position, Player.playerPos);

        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, direction, distance);

        // Update originalTreeColors
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Tree"))
            {
                //Debug.Log(" compare tag tree");
                Renderer renderer = hit.collider.GetComponent<MeshRenderer>();

                if (renderer != null)
                {
                    Material material = renderer.material;

                    if (!originalTreeColors.ContainsKey(hit.collider.gameObject))
                    {
                        originalTreeColors.Add(hit.collider.gameObject, material.color);
                    }

                    material.SetFloat("_Mode", 3); //set rendering mode to "Transparent"
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;

                    Color color = material.color;
                    color.a = 0.1f;
                    material.color = color;

                    // Remove the game object from oldTreeColors if it exists
                    if (oldTreeColors.ContainsKey(hit.collider.gameObject))
                    {
                        oldTreeColors.Remove(hit.collider.gameObject);
                    }
                }
            }
        }

        foreach (KeyValuePair<GameObject, Color> pair in missingColors)
        {
            Renderer renderer = pair.Key.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = 1f; // Set alpha to 1
                material.color = color;
            }
        }

        // Restore oldTreeColors that are not in originalTreeColors
        foreach (KeyValuePair<GameObject, Color> pair in oldTreeColors)
        {
            Renderer renderer = pair.Key.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = 1f; // Set alpha to 1
                material.color = color;
            }
        }

        // Create a copy of originalTreeColors for oldTreeColors
        if (originalTreeColors.Count > 0 && oldTreeColors.Count == 0)
        {
            foreach (KeyValuePair<GameObject, Color> pair in originalTreeColors)
            {
                oldTreeColors.Add(pair.Key, pair.Value);
            }
        }

    }
}
