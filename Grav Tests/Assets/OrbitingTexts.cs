using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrbitingTexts : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float radius = 5f;
    [SerializeField] private int numberOfObjects = 10;
    [SerializeField] private List<string> textList;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float baseFontSize = 10f;
    [SerializeField] private float scaleFactor = 0.5f;
    [SerializeField] private float minFadeDistance = 2f;
    [SerializeField] private float maxFadeDistance = 10f;
    [SerializeField] private float exponentFactor = 2f; // Added exponent factor as a serialized field


    private List<TextMeshPro> textObjects;

    private void Start()
    {
        if (textPrefab.GetComponent<TextMeshPro>() == null)
        {
            Debug.LogError("The provided prefab does not have a TextMeshPro component.");
            return;
        }

        if (textList.Count != numberOfObjects)
        {
            Debug.LogError("The text list size must be equal to the number of objects.");
            return;
        }

        textObjects = new List<TextMeshPro>();
        PositionTextsSpherically();
    }

    private void Update()
    {
        UpdateFontSizesBasedOnDistance();
        UpdateTextFacing();
        UpdateTextTransparency();
    }

    private void PositionTextsSpherically()
    {
        float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f)); // Golden angle
        float offset = 2f / numberOfObjects;
        float increment = Mathf.PI * (3f - Mathf.Sqrt(5f));

        for (int i = 0; i < numberOfObjects; i++)
        {
            float y = ((i * offset) - 1) + (offset / 2);
            float r = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
            float phi = ((i + 1) % numberOfObjects) * increment;

            Vector3 pos = new Vector3(Mathf.Cos(phi) * r, y, Mathf.Sin(phi) * r) * radius;
            GameObject newText = Instantiate(textPrefab, orbitCenter.position + pos, Quaternion.identity, orbitCenter);
            newText.GetComponent<TextMeshPro>().text = textList[i];
            textObjects.Add(newText.GetComponent<TextMeshPro>());
        }
    }

    private void UpdateFontSizesBasedOnDistance()
    {
        foreach (TextMeshPro textObject in textObjects)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, textObject.transform.position);
            textObject.fontSize = baseFontSize + (distance * scaleFactor);
        }
    }

    private void UpdateTextFacing()
    {
        foreach (TextMeshPro textObject in textObjects)
        {
            textObject.transform.rotation = Quaternion.LookRotation(textObject.transform.position - mainCamera.transform.position);
        }
    }

    private void UpdateTextTransparency()
    {
        foreach (TextMeshPro textObject in textObjects)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, textObject.transform.position);
            float normalizedDistance = Mathf.InverseLerp(minFadeDistance, maxFadeDistance, distance);
            float alpha = Mathf.Clamp01(Mathf.Pow(normalizedDistance, exponentFactor));

            Color textColor = textObject.color;
            textColor.a = alpha;
            textObject.color = textColor;
        }
    }
}
