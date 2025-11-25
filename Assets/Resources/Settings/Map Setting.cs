using UnityEngine;
[CreateAssetMenu(fileName = "Map Setting", menuName = "Game Setting/Map Setting")]
public class MapSetting : ScriptableObject
{
    public int defaultWidth;
    public int width = 50;
    public int defaultHeight;
    public int height = 50;

    public string seed;

    [Range(0, 100)]
    public int randomFillPercent = 45;

    [Range(0, 10)]
    public int mapSmooth = 5;

    [Range(0, 10)]
    public int passageRadius = 5;

    [Range(0, 10)]
    public int squareSize = 1;

    [Range(0, 10)]
    public int borderSize = 5;

    [Range(0, 100)]
    public int wallThresHoldSize = 50; // Less = more smaller walls
    [Range(0, 100)]
    public int roomThresHoldSize = 50; // Less = more smaller rooms

    public void Save(ref MapData data)
    {
        data.width = width;
        data.height = height;
        data.seed = seed;
    }

    public void Load(MapData data)
    {
        width = data.width;
        height = data.height;
        seed = data.seed;
    }

    public void ResetData()
    {
        width = defaultWidth;
        height = defaultHeight;
        seed = Random.Range(int.MinValue, int.MaxValue).ToString();
    }
}
