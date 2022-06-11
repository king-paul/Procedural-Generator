using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProceduralGeneration;

public class CaveUIManager : MonoBehaviour
{
    public MeshGenerator meshGenerator;
    public Camera camera2d;
    public InputManager inputManager;

    [Header("Sliders")]
    public Slider widthSlider;
    public Slider heightSlider;
    public Slider fillPercentSlider;
    public Slider smoothingSlider;
    public Slider caveBorderSizeSlider;
    public Slider minWallSizeSlider;
    public Slider minRooomSizeSlider;
    public Slider passageWidthSlider;

    [Header("Slider value Labels")]
    public TextMeshProUGUI widthValue;
    public TextMeshProUGUI heightValue;
    public TextMeshProUGUI fillPercentValue;
    public TextMeshProUGUI smoothingValue;
    public TextMeshProUGUI caveBordersValue;
    public TextMeshProUGUI wallSizeValue;
    public TextMeshProUGUI roomSizeValue;
    public TextMeshProUGUI passageWidthValue;

    [Header("Other Componenets")]
    public TMP_InputField seedInput;
    public Toggle useRandomToggle;

    private int width = 128;
    private int height = 72;
    private int seed = 0;
    private bool useRandomSeed = false;
    private int randomFillPercent = 50;
    private int smoothingIterations = 5;
    private int borderSize = 3;
    private int minimumWallSize = 50;
    private int minimumRoomSize = 50;
    private int passageWidth = 1;
    private int tileSize = 1;
    private bool is2D;

    private int wallHeight = 5;

    private bool generated = false;

    public void SetWidth(float value)
    {
        widthValue.text = width.ToString();
        width = (int) value;
    }

    public void SetHeight(float value)
    {
        heightValue.text = height.ToString();
        height = (int) value;
    }

    public void SetFillPercent(float value)
    {
        fillPercentValue.text = value.ToString();
        randomFillPercent = (int)value;
    }

    public void SetSmoothing(float value)
    {
        smoothingValue.text = value.ToString();
        smoothingIterations = (int) value;
    }

    public void SetCaveBorder(float value)
    {
        caveBordersValue.text = value.ToString();
        borderSize = (int) value;
    }

    public void SetMinWallSize(float value)
    {
        wallSizeValue.text = value.ToString();
        minimumWallSize = (int) value;
    }

    public void SetMinRoomSize(float value)
    {
        roomSizeValue.text = value.ToString();
        minimumRoomSize = (int) value;
    }

    public void SetPassageWidth(float value)
    {
        passageWidthValue.text = value.ToString();
        passageWidth = (int) value;
    }

    public void SetSeed(int value)
    {
        seedInput.text = value.ToString();
        seed = value;
    }

    public void SetUseRandom(bool random)
    {
        useRandomSeed = random;

        if (random)
        {
            seedInput.enabled = false;
            seedInput.interactable = false;
        }
        else
        {
            seedInput.interactable = true;
            seedInput.enabled = true;
        }
    }

    public void SwitchViewMode(bool mode2d)
    {
        is2D = mode2d;
        inputManager.Is2D = is2D;

        if (mode2d)
        {
            camera2d.gameObject.SetActive(true);
            meshGenerator.transform.rotation = Quaternion.Euler(270, 0, 0);            
        }
        else
        {
            camera2d.gameObject.SetActive(false);
            meshGenerator.transform.rotation = Quaternion.identity;

        }
    }

    public void GenerateCave()
    {
        var cave = new CaveGenerator(width, height, randomFillPercent, smoothingIterations, borderSize,
            minimumWallSize, minimumRoomSize, passageWidth, false, useRandomSeed, seed, false);

        if (useRandomSeed)
        {
            seed = cave.Seed;
            seedInput.text = seed.ToString();
        }

        meshGenerator.GenerateMesh(cave.MarchingSquares, tileSize, is2D);

        generated = true;
    }

    public void Regenerate()
    {
        if (!generated)
            return;

        var cave = new CaveGenerator(width, height, randomFillPercent, smoothingIterations, borderSize,
            minimumWallSize, minimumRoomSize, passageWidth, false, false, seed, false);

        meshGenerator.GenerateMesh(cave.MarchingSquares, tileSize, is2D);
    }

    private void Start()
    {        
        // Set slider values
        widthSlider.value = width;
        heightSlider.value = height;
        fillPercentSlider.value = randomFillPercent;
        smoothingSlider.value = smoothingIterations;
        caveBorderSizeSlider.value = borderSize;
        minWallSizeSlider.value = minimumWallSize;
        minRooomSizeSlider.value = minimumRoomSize;
        passageWidthSlider.value = passageWidth;

        
        // set value labels text
        widthValue.text = widthSlider.value.ToString();
        heightValue.text = heightSlider.value.ToString();
        fillPercentValue.text = fillPercentSlider.value.ToString();
        smoothingValue.text = smoothingSlider.value.ToString();
        caveBordersValue.text = caveBorderSizeSlider.value.ToString();
        wallSizeValue.text = minWallSizeSlider.value.ToString();
        roomSizeValue.text = minRooomSizeSlider.value.ToString();
        passageWidthValue.text = passageWidthSlider.value.ToString();

        // set seed input and toggle
        seedInput.text = seed.ToString();
        useRandomToggle.isOn = useRandomSeed;

        //GenerateCave();
    }

}