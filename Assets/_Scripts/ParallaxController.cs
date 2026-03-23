using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayerConfig
{
    public GameObject layerPrefab;
    [Range(0f, 1f)] public float parallaxFactorX = 0.5f; public Vector3 verticalOffset;
}

public class ParallaxController : MonoBehaviour
{
    public List<ParallaxLayerConfig> layerConfigs;
    public Transform playerTransform;

    private class RuntimeLayer
    {
        public Transform container;
        public float factorX;
        public float tileWidth;
        public Vector3 startPos;
    }

    private List<RuntimeLayer> runtimeLayers = new List<RuntimeLayer>();

    void Start()
    {
        if (!playerTransform) playerTransform = Camera.main.transform;

        foreach (var config in layerConfigs)
        {
            if (!config.layerPrefab) continue;

            GameObject containerGO = new GameObject(config.layerPrefab.name + " Container");
            containerGO.transform.position = config.layerPrefab.transform.position + config.verticalOffset;
            containerGO.transform.SetParent(this.transform);

            SpriteRenderer sr = config.layerPrefab.GetComponent<SpriteRenderer>();
            float width = sr.bounds.size.x;

            for (int i = -1; i <= 1; i++)
            {
                GameObject clone = Instantiate(config.layerPrefab, containerGO.transform);
                clone.transform.localPosition = new Vector3(width * i, config.verticalOffset.y, 0f);
            }

            runtimeLayers.Add(new RuntimeLayer
            {
                container = containerGO.transform,
                factorX = config.parallaxFactorX,
                tileWidth = width,
                startPos = containerGO.transform.position
            });

            config.layerPrefab.SetActive(false);
        }
    }

    void LateUpdate()
    {
        foreach (var layer in runtimeLayers)
        {
            float newX = layer.startPos.x + (playerTransform.position.x - layer.startPos.x) * layer.factorX;

            layer.container.position = new Vector3(newX, playerTransform.position.y, layer.container.position.z);

            if (playerTransform.position.x - layer.container.position.x >= layer.tileWidth)
                layer.container.position += new Vector3(layer.tileWidth, 0, 0);
            else if (layer.container.position.x - playerTransform.position.x >= layer.tileWidth)
                layer.container.position -= new Vector3(layer.tileWidth, 0, 0);
        }
    }
}




#region parallaxcontroller #2
















#endregion










