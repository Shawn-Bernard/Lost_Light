using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [Header("Map Settings")]
    [SerializeField] MapSetting mapSetting;
    private int width;
    private int height;

    private string seed;

    private int randomFillPercent;

    private int mapSmooth;

    private int passageRadius;
    private int squareSize;

    private int[,] map;

    private int borderSize;
    private int wallThresHoldSize;
    private int roomThresHoldSize;

    [SerializeField] GameObject ground;

    private Room mainRoom;
    private List<Room> survivingRooms = new List<Room>();

    void Awake()
    {
        ApplySetting();
        GenerateMap();
        ground.transform.localScale = new Vector3(width, 1, height);
    }
    void ApplySetting()
    {
        width = mapSetting.width;
        height = mapSetting.height;
        if (SaveSystem.SaveFileExists() && !string.IsNullOrEmpty(mapSetting.seed))
        {
            Debug.Log("Using data seed");
            seed = mapSetting.seed;
        }
        else
        {
            Debug.Log("Using bad seed");
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString();
        }
        randomFillPercent = mapSetting.randomFillPercent;
        mapSmooth = mapSetting.mapSmooth;
        passageRadius = mapSetting.passageRadius;
        squareSize = mapSetting.squareSize;
        borderSize = mapSetting.borderSize;
        wallThresHoldSize = mapSetting.wallThresHoldSize;
        roomThresHoldSize = mapSetting.roomThresHoldSize;

        mapSetting.width = width;
        mapSetting.height = height;
        mapSetting.seed = seed;
    }

    #region Map Logic
    public Vector3 GetPositionInsideMap()
    {
        Room room;
        List<Coord> safeTiles;

        room = survivingRooms[UnityEngine.Random.Range(0, survivingRooms.Count)];
        safeTiles = new List<Coord>();

        foreach (Coord tile in room.tiles)
        {
            bool isSafe = true;
            // Checks for safe tiles to spawn thats not a wall
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && map[x, y] == 1)
                    {
                        isSafe = false;
                        break;
                    }
                }
            }
            if (isSafe) safeTiles.Add(tile);
        }

        Coord safeCoord = safeTiles[UnityEngine.Random.Range(0, safeTiles.Count)];
        return CoordToWorldPoint(safeCoord);
    }
    /// <summary>
    /// Creates a new array for map, smooths map, process the walls and rooms, creates mesh and spawn player 
    /// </summary>
    void GenerateMap()
    {
        Debug.Log($"seed {seed}");
        mapSetting.seed = seed;
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < mapSmooth; i++)
        {
            SmoothMap();
        }

        int[,] borderMap = new int[width + borderSize * 2, height + borderSize * 2];
        ProcessMap();

        for (int x = 0; x < borderMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderMap.GetLength(1); y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    borderMap[x,y] = map[x - borderSize,y - borderSize];
                }
                else
                {
                    borderMap[x, y] = 1;
                }
            }
        }
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderMap, squareSize);
    }
    /// <summary>
    /// Removes small room and walls, gets main room and connects rooms with passages
    /// </summary>
    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegion(1);


        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThresHoldSize)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }


        List<List<Coord>> roomRegions = GetRegion(0);
        survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresHoldSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }
        survivingRooms.Sort();
        mainRoom = survivingRooms[0];
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].IsAccessibleFromMainRoom = true;
        ConnectClosestRooms(survivingRooms);
    }
    /// <summary>
    /// Connects closet rooms to get access
    /// </summary>
    /// <param name="allRooms"></param>
    /// <param name="forceAccessibilityFromMainRoom"></param>
    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {

        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.IsAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }
    /// <summary>
    /// Crates a passage between the 2 rooms
    /// </summary>
    /// <param name="roomA"></param>
    /// <param name="roomB"></param>
    /// <param name="tileA"></param>
    /// <param name="tileB"></param>
    void CreatePassage(Room roomA,Room roomB,Coord tileA,Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        //Debug.DrawLine(CoordToWorldPoint(tileA),CoordToWorldPoint(tileB),Color.green,100f);

        List<Coord> line = GetLine(tileA,tileB);

        foreach (Coord coord in line)
        {
            DrawCircle(coord);
        }

    }

    /// <summary>
    /// helps make the passages wider 
    /// </summary>
    /// <param name="coord"></param>
    void DrawCircle(Coord coord)
    {
        for (int x = -passageRadius ; x <= passageRadius; x++)
        {
            for (int y = -passageRadius; y <= passageRadius; y++)
            {
                if (x*x + y*y <= passageRadius * passageRadius)
                {
                    int realX = coord.tileX + x;
                    int realY = coord.tileY + y;
                    if (IsInMapRange(realX, realY))
                    {
                        map[realX, realY] = 0; 
                    }
                }
            } 
        } 
    }
    /// <summary>
    /// Returns a list of coordinates thats a straight line from coord to coord
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    /// <summary>
    /// Converts a coord tile to a world position
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-width / 2 + .5f + tile.tileX, 0, -height / 2 + .5f + tile.tileY);
    }

    /// <summary>
    /// Returns a list of connects reigons
    /// </summary>
    /// <param name="tileType"></param>
    /// <returns></returns>
    List<List<Coord>> GetRegion(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x,y] == 0 && map[x,y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }

    /// <summary>
    /// Returns a list of coord tiles
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <returns></returns>
    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width,height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));

        mapFlags[startX, startY] = 1; //1 = seen

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1;x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x,y] == 0 && map[x,y] == tileType)
                        {
                            mapFlags[x,y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    bool IsInMapRange(int x, int y) 
    {
		return x >= 0 && x < width && y >= 0 && y < height;
	}
    void RandomFillMap()
    {
        // pseudo random number generator
        System.Random prng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width-1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
                
            }
        }
    }
    /// <summary>
    /// Runs a for loop that gets the neighbour walls and sets (1) wall or (2) air based on neighbours  
    /// </summary>
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighrbourWallTiles = GetSurroundingWallCount(x, y);
                
                if (neighrbourWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if (neighrbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }
    /// <summary>
    /// Runs a for loop around the x,y handed in and returns the walls counted
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX,neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    #endregion

    #region Map Structs
    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;

        public bool IsAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room() { }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();

            foreach (Coord tile in tiles)
            {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (x == tile.tileX || y == tile.tileY)
                        {
                            if (map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!IsAccessibleFromMainRoom)
            {
                IsAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA,Room roomB)
        {
            if (roomA.IsAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            } 
            else if (roomB.IsAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
                roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }
        
    }
    #endregion
}
[System.Serializable]
public struct MapData
{
    public int width;
    public int height;
    public string seed;
}
