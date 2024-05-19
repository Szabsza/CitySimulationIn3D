using Project.Managers;
using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Type = Project.Util.Type;

namespace Project.Entities;

public class GlCity
{
    public struct CityGrid(int x, int y, Type type, string name, Direction facingDirection, bool isRoad = false)
    {
        public readonly int X = x;
        public readonly int Y = y;
        public readonly Type Type = type;
        public readonly string Name = name;
        public readonly Direction FacingDirection = facingDirection;
    }

    private List<CityGrid> _buildingsLayout = new List<CityGrid>(new[]
    {
        // BUILDINGS
        new CityGrid(0, 0, Type.A, "building", Direction.South),
        new CityGrid(1, 0, Type.B, "building", Direction.South),
        new CityGrid(2, 0, Type.D, "building", Direction.South),
        new CityGrid(3, 0, Type.G, "building", Direction.South),
        new CityGrid(4, 0, Type.C, "building", Direction.South),
        new CityGrid(5, 0, Type.A, "building", Direction.South),
        new CityGrid(6, 0, Type.H, "building", Direction.South),
        new CityGrid(7, 0, Type.B, "building", Direction.South),
        new CityGrid(8, 0, Type.A, "building", Direction.South),
        new CityGrid(9, 0, Type.E, "building", Direction.South),
        new CityGrid(10, 0, Type.A, "building", Direction.South),
        new CityGrid(11, 0, Type.A, "building", Direction.South),
        new CityGrid(12, 0, Type.F, "building", Direction.South),
        new CityGrid(13, 0, Type.A, "building", Direction.South),
        new CityGrid(14, 0, Type.G, "building", Direction.South),

        new CityGrid(0, 14, Type.A, "building", Direction.North),
        new CityGrid(1, 14, Type.B, "building", Direction.North),
        new CityGrid(2, 14, Type.D, "building", Direction.North),
        new CityGrid(3, 14, Type.G, "building", Direction.North),
        new CityGrid(4, 14, Type.C, "building", Direction.North),
        new CityGrid(5, 14, Type.A, "building", Direction.North),
        new CityGrid(6, 14, Type.H, "building", Direction.North),
        new CityGrid(7, 14, Type.B, "building", Direction.North),
        new CityGrid(8, 14, Type.A, "building", Direction.North),
        new CityGrid(9, 14, Type.E, "building", Direction.North),
        new CityGrid(10, 14, Type.A, "building", Direction.North),
        new CityGrid(11, 14, Type.A, "building", Direction.North),
        new CityGrid(12, 14, Type.F, "building", Direction.North),
        new CityGrid(13, 14, Type.A, "building", Direction.North),
        new CityGrid(14, 14, Type.G, "building", Direction.North),

        new CityGrid(0, 1, Type.D, "building", Direction.East),
        new CityGrid(0, 2, Type.E, "building", Direction.East),
        new CityGrid(0, 3, Type.F, "building", Direction.East),
        new CityGrid(0, 4, Type.G, "building", Direction.East),
        new CityGrid(0, 5, Type.H, "building", Direction.East),
        new CityGrid(0, 6, Type.G, "building", Direction.East),
        new CityGrid(0, 7, Type.E, "building", Direction.East),
        new CityGrid(0, 8, Type.A, "building", Direction.East),
        new CityGrid(0, 9, Type.C, "building", Direction.East),
        new CityGrid(0, 10, Type.B, "building", Direction.East),
        new CityGrid(0, 11, Type.G, "building", Direction.East),
        new CityGrid(0, 12, Type.D, "building", Direction.East),
        new CityGrid(0, 13, Type.E, "building", Direction.East),

        new CityGrid(14, 1, Type.D, "building", Direction.West),
        new CityGrid(14, 2, Type.E, "building", Direction.West),
        new CityGrid(14, 3, Type.F, "building", Direction.West),
        new CityGrid(14, 4, Type.G, "building", Direction.West),
        new CityGrid(14, 5, Type.H, "building", Direction.West),
        new CityGrid(14, 6, Type.G, "building", Direction.West),
        new CityGrid(14, 7, Type.E, "building", Direction.West),
        new CityGrid(14, 8, Type.A, "building", Direction.West),
        new CityGrid(14, 9, Type.C, "building", Direction.West),
        new CityGrid(14, 10, Type.B, "building", Direction.West),
        new CityGrid(14, 11, Type.G, "building", Direction.West),
        new CityGrid(14, 12, Type.D, "building", Direction.West),
        new CityGrid(14, 13, Type.E, "building", Direction.West),

        // a
        new CityGrid(2, 2, Type.A, "building", Direction.North),
        new CityGrid(3, 2, Type.H, "building", Direction.North),
        new CityGrid(4, 2, Type.H, "building", Direction.North),
        new CityGrid(5, 2, Type.D, "building", Direction.North),
        new CityGrid(6, 2, Type.B, "building", Direction.North),

        //b
        new CityGrid(2, 4, Type.E, "building", Direction.West),
        new CityGrid(3, 4, Type.H, "building", Direction.North),
        new CityGrid(4, 4, Type.E, "building", Direction.North),
        new CityGrid(5, 4, Type.G, "building", Direction.North),
        new CityGrid(6, 4, Type.B, "building", Direction.East),

        new CityGrid(2, 6, Type.D, "building", Direction.West),
        new CityGrid(3, 6, Type.H, "building", Direction.South),
        new CityGrid(4, 6, Type.C, "building", Direction.South),
        new CityGrid(5, 6, Type.D, "building", Direction.South),
        new CityGrid(6, 6, Type.A, "building", Direction.East),

        new CityGrid(2, 5, Type.B, "building", Direction.West),
        new CityGrid(6, 5, Type.B, "building", Direction.East),

        //c
        new CityGrid(8, 2, Type.A, "building", Direction.North),
        new CityGrid(9, 2, Type.H, "building", Direction.North),
        new CityGrid(10, 2, Type.D, "building", Direction.North),
        new CityGrid(11, 2, Type.E, "building", Direction.North),
        new CityGrid(12, 2, Type.A, "building", Direction.North),

        //d
        new CityGrid(8, 4, Type.B, "building", Direction.West),
        new CityGrid(9, 4, Type.H, "building", Direction.North),
        new CityGrid(10, 4, Type.C, "building", Direction.North),
        new CityGrid(11, 4, Type.E, "building", Direction.North),
        new CityGrid(12, 4, Type.D, "building", Direction.East),

        new CityGrid(8, 6, Type.G, "building", Direction.West),
        new CityGrid(9, 6, Type.A, "building", Direction.South),
        new CityGrid(10, 6, Type.C, "building", Direction.South),
        new CityGrid(11, 6, Type.C, "building", Direction.South),
        new CityGrid(12, 6, Type.D, "building", Direction.East),

        new CityGrid(8, 5, Type.B, "building", Direction.West),
        new CityGrid(12, 5, Type.B, "building", Direction.East),

        //e
        new CityGrid(2, 8, Type.A, "building", Direction.West),
        new CityGrid(2, 9, Type.B, "building", Direction.West),
        new CityGrid(2, 10, Type.A, "building", Direction.West),
        new CityGrid(3, 10, Type.D, "building", Direction.South),
        new CityGrid(4, 10, Type.E, "building", Direction.South),
        new CityGrid(5, 10, Type.D, "building", Direction.South),
        new CityGrid(6, 10, Type.A, "building", Direction.South),

        //f
        new CityGrid(4, 8, Type.G, "building", Direction.North),
        new CityGrid(5, 8, Type.H, "building", Direction.North),
        new CityGrid(6, 8, Type.G, "building", Direction.North),

        //g
        new CityGrid(2, 12, Type.G, "building", Direction.South),
        new CityGrid(3, 12, Type.A, "building", Direction.South),
        new CityGrid(4, 12, Type.G, "building", Direction.South),
        new CityGrid(5, 12, Type.H, "building", Direction.South),
        new CityGrid(6, 12, Type.B, "building", Direction.South),

        //h
        new CityGrid(8, 8, Type.C, "building", Direction.West),
        new CityGrid(8, 9, Type.C, "building", Direction.West),
        new CityGrid(8, 10, Type.G, "building", Direction.West),

        new CityGrid(9, 8, Type.B, "building", Direction.East),
        new CityGrid(9, 9, Type.E, "building", Direction.East),
        new CityGrid(9, 10, Type.B, "building", Direction.East),

        //i
        new CityGrid(8, 12, Type.H, "building", Direction.South),
        new CityGrid(9, 12, Type.H, "building", Direction.South),

        //j
        new CityGrid(11, 8, Type.C, "building", Direction.West),
        new CityGrid(11, 9, Type.D, "building", Direction.West),
        new CityGrid(11, 10, Type.A, "building", Direction.West),

        new CityGrid(12, 8, Type.E, "building", Direction.East),
        new CityGrid(12, 9, Type.G, "building", Direction.East),
        new CityGrid(12, 10, Type.A, "building", Direction.East),

        //k
        new CityGrid(11, 12, Type.A, "building", Direction.South),
        new CityGrid(12, 12, Type.G, "building", Direction.South),
    });

    private List<CityGrid> _roadsLayout = new List<CityGrid>(new[]
    {
        // VERTICAL
        new CityGrid(1, 1, Type.Corner, "road", Direction.South),
        new CityGrid(1, 2, Type.Straight, "road", Direction.South),
        new CityGrid(1, 3, Type.Tsplit, "road", Direction.South),
        new CityGrid(1, 4, Type.Straight, "road", Direction.South),
        new CityGrid(1, 5, Type.Crossing, "road", Direction.South),
        new CityGrid(1, 6, Type.Straight, "road", Direction.South),
        new CityGrid(1, 7, Type.Tsplit, "road", Direction.South),
        new CityGrid(1, 8, Type.Straight, "road", Direction.South),
        new CityGrid(1, 9, Type.Straight, "road", Direction.South),
        new CityGrid(1, 10, Type.Straight, "road", Direction.South),
        new CityGrid(1, 11, Type.Tsplit, "road", Direction.South),
        new CityGrid(1, 12, Type.Straight, "road", Direction.South),
        new CityGrid(1, 13, Type.Corner, "road", Direction.East),

        new CityGrid(7, 1, Type.Tsplit, "road", Direction.West),
        new CityGrid(7, 2, Type.Straight, "road", Direction.South),
        new CityGrid(7, 3, Type.Junction, "road", Direction.South),
        new CityGrid(7, 4, Type.Straight, "road", Direction.South),
        new CityGrid(7, 5, Type.Crossing, "road", Direction.South),
        new CityGrid(7, 6, Type.Straight, "road", Direction.South),
        new CityGrid(7, 7, Type.Junction, "road", Direction.South),
        new CityGrid(7, 8, Type.Straight, "road", Direction.South),
        new CityGrid(7, 9, Type.Tsplit, "road", Direction.North),
        new CityGrid(7, 10, Type.Straight, "road", Direction.South),
        new CityGrid(7, 11, Type.Junction, "road", Direction.South),
        new CityGrid(7, 12, Type.Straight, "road", Direction.South),
        new CityGrid(7, 13, Type.Tsplit, "road", Direction.East),

        new CityGrid(13, 1, Type.Corner, "road", Direction.West),
        new CityGrid(13, 2, Type.Straight, "road", Direction.South),
        new CityGrid(13, 3, Type.Tsplit, "road", Direction.North),
        new CityGrid(13, 4, Type.Straight, "road", Direction.South),
        new CityGrid(13, 5, Type.Crossing, "road", Direction.South),
        new CityGrid(13, 6, Type.Straight, "road", Direction.South),
        new CityGrid(13, 7, Type.Tsplit, "road", Direction.North),
        new CityGrid(13, 8, Type.Straight, "road", Direction.South),
        new CityGrid(13, 9, Type.Crossing, "road", Direction.South),
        new CityGrid(13, 10, Type.Straight, "road", Direction.South),
        new CityGrid(13, 11, Type.Tsplit, "road", Direction.North),
        new CityGrid(13, 12, Type.Straight, "road", Direction.South),
        new CityGrid(13, 13, Type.Corner, "road", Direction.North),

        new CityGrid(10, 8, Type.Straight, "road", Direction.South),
        new CityGrid(10, 9, Type.Crossing, "road", Direction.South),
        new CityGrid(10, 10, Type.Straight, "road", Direction.South),
        new CityGrid(10, 11, Type.Junction, "road", Direction.South),
        new CityGrid(10, 12, Type.Straight, "road", Direction.South),

        new CityGrid(3, 8, Type.Straight, "road", Direction.South),

        // Horizontal
        new CityGrid(2, 1, Type.Straight, "road", Direction.East),
        new CityGrid(3, 1, Type.Straight, "road", Direction.East),
        new CityGrid(4, 1, Type.Crossing, "road", Direction.East),
        new CityGrid(5, 1, Type.Straight, "road", Direction.East),
        new CityGrid(6, 1, Type.Straight, "road", Direction.East),
        new CityGrid(8, 1, Type.Straight, "road", Direction.East),
        new CityGrid(9, 1, Type.Straight, "road", Direction.East),
        new CityGrid(10, 1, Type.Straight, "road", Direction.East),
        new CityGrid(11, 1, Type.Straight, "road", Direction.East),
        new CityGrid(12, 1, Type.Straight, "road", Direction.East),

        new CityGrid(2, 3, Type.Straight, "road", Direction.East),
        new CityGrid(3, 3, Type.Straight, "road", Direction.East),
        new CityGrid(4, 3, Type.Straight, "road", Direction.East),
        new CityGrid(5, 3, Type.Straight, "road", Direction.East),
        new CityGrid(6, 3, Type.Straight, "road", Direction.East),
        new CityGrid(8, 3, Type.Straight, "road", Direction.East),
        new CityGrid(9, 3, Type.Straight, "road", Direction.East),
        new CityGrid(10, 3, Type.Crossing, "road", Direction.East),
        new CityGrid(11, 3, Type.Straight, "road", Direction.East),
        new CityGrid(12, 3, Type.Straight, "road", Direction.East),

        new CityGrid(2, 7, Type.Straight, "road", Direction.East),
        new CityGrid(3, 7, Type.Tsplit, "road", Direction.West),
        new CityGrid(4, 7, Type.Straight, "road", Direction.East),
        new CityGrid(5, 7, Type.Straight, "road", Direction.East),
        new CityGrid(6, 7, Type.Straight, "road", Direction.East),
        new CityGrid(8, 7, Type.Straight, "road", Direction.East),
        new CityGrid(9, 7, Type.Straight, "road", Direction.East),
        new CityGrid(10, 7, Type.Tsplit, "road", Direction.West),
        new CityGrid(11, 7, Type.Straight, "road", Direction.East),
        new CityGrid(12, 7, Type.Straight, "road", Direction.East),

        new CityGrid(2, 11, Type.Straight, "road", Direction.East),
        new CityGrid(3, 11, Type.Straight, "road", Direction.East),
        new CityGrid(4, 11, Type.Crossing, "road", Direction.East),
        new CityGrid(5, 11, Type.Straight, "road", Direction.East),
        new CityGrid(6, 11, Type.Straight, "road", Direction.East),
        new CityGrid(8, 11, Type.Straight, "road", Direction.East),
        new CityGrid(9, 11, Type.Straight, "road", Direction.East),
        new CityGrid(11, 11, Type.Straight, "road", Direction.East),
        new CityGrid(12, 11, Type.Straight, "road", Direction.East),

        new CityGrid(2, 13, Type.Straight, "road", Direction.East),
        new CityGrid(3, 13, Type.Straight, "road", Direction.East),
        new CityGrid(4, 13, Type.Straight, "road", Direction.East),
        new CityGrid(5, 13, Type.Straight, "road", Direction.East),
        new CityGrid(6, 13, Type.Straight, "road", Direction.East),
        new CityGrid(8, 13, Type.Straight, "road", Direction.East),
        new CityGrid(9, 13, Type.Straight, "road", Direction.East),
        new CityGrid(10, 13, Type.Tsplit, "road", Direction.East),
        new CityGrid(11, 13, Type.Straight, "road", Direction.East),
        new CityGrid(12, 13, Type.Straight, "road", Direction.East),

        new CityGrid(3, 9, Type.Corner, "road", Direction.East),
        new CityGrid(4, 9, Type.Straight, "road", Direction.East),
        new CityGrid(5, 9, Type.Crossing, "road", Direction.East),
        new CityGrid(6, 9, Type.Straight, "road", Direction.East),
    });

    private GL _gl;

    private Vector3D<float> _offsetToOrigin;
    private Vector3D<float> _offsetHeightForCars;

    private readonly List<List<CityGrid>> _layout;
    private HashSet<Vector3D<float>> _placedStreetlights;

    public GlCity(GL gl)
    {
        _gl = gl;

        _placedStreetlights = new HashSet<Vector3D<float>>();

        _layout = new List<List<CityGrid>>();
        for (var i = 0; i < 15; i++)
        {
            _layout.Add(new List<CityGrid>());
            for (var j = 0; j < 15; j++)
            {
                _layout[i].Add(new CityGrid());
            }
        }

        _offsetToOrigin = new Vector3D<float>(-_layout.Count, 0f, -_layout.Count);
        _offsetHeightForCars = new Vector3D<float>(0, 0.15f, 0);
        
        GenerateLayout();
        SetUpObjects();
    }

    public void GenerateLayout()
    {
        // overwrites if two positions are the same, so it does not render 2 objects on the same position
        foreach (var building in _buildingsLayout)
        {
            _layout[building.X][building.Y] = building;
        }

        foreach (var road in _roadsLayout)
        {
            _layout[road.X][road.Y] = road;
        }
    }

    private Vector3D<float> RotateStreetlightsPosition(float x, float y, float z, GlRoad road)
    {
        var position = new Vector3D<float>(x, y, z);
        var translatedPosition = position - road.Position;

        Vector3D<float> rotatedPosition = road.FacingDirection switch
        {
            Direction.South => new Vector3D<float>(translatedPosition.X, translatedPosition.Y, translatedPosition.Z),
            Direction.North => new Vector3D<float>(-translatedPosition.X, translatedPosition.Y, -translatedPosition.Z),
            Direction.East => new Vector3D<float>(translatedPosition.Z, translatedPosition.Y, -translatedPosition.X),
            Direction.West => new Vector3D<float>(-translatedPosition.Z, translatedPosition.Y, translatedPosition.X),
            _ => throw new ArgumentOutOfRangeException()
        };

        return rotatedPosition + road.Position;
    }

    private void SetupStreetlight(Vector3D<float> position, Type type, Direction direction, bool overwrite = false)
    {
        if (overwrite)
        {
            _placedStreetlights.Remove(position);
            LampManager.Instance.RemoveLampAt(position);
        }

        if (_placedStreetlights.Add(position))
        {
            var newLamp = new GlLamp(_gl, type, position, direction);
            LampManager.Instance.AddLamp(newLamp);
        }
    }

    private void SetUpStreetlights(GlRoad road)
    {
        Direction streetLightDir;
        Direction trafficLightDir;
        Direction trafficLightOppositeDir;

        switch (road.FacingDirection)
        {
            case Direction.North:
                streetLightDir = Direction.South;
                trafficLightDir = Direction.West;
                trafficLightOppositeDir = Direction.East;
                break;
            case Direction.South:
                streetLightDir = Direction.North;
                trafficLightDir = Direction.East;
                trafficLightOppositeDir = Direction.West;
                break;
            case Direction.East:
                streetLightDir = Direction.West;
                trafficLightDir = Direction.North;
                trafficLightOppositeDir = Direction.South;
                break;
            case Direction.West:
                streetLightDir = Direction.East;
                trafficLightDir = Direction.South;
                trafficLightOppositeDir = Direction.North;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (road.Type)
        {
            case Type.Straight:
                SetupStreetlight(
                    RotateStreetlightsPosition(road.Position.X - 1, road.Position.Y, road.Position.Z + 1, road),
                    Type.StreetLight, streetLightDir);
                break;
            case Type.Corner:
                break;
            case Type.Junction:
                SetupStreetlight(
                    RotateStreetlightsPosition(road.Position.X + 1, road.Position.Y, road.Position.Z - 1, road),
                    Type.TrafficLight, trafficLightDir, true);
                SetupStreetlight(
                    RotateStreetlightsPosition(road.Position.X - 1, road.Position.Y, road.Position.Z + 1, road),
                    Type.TrafficLight, trafficLightOppositeDir, true);
                break;
            case Type.Tsplit:
                SetupStreetlight(
                    RotateStreetlightsPosition(road.Position.X + 1, road.Position.Y, road.Position.Z - 1, road),
                    Type.TrafficLight, trafficLightDir, true);
                break;
            case Type.Crossing:
                SetupStreetlight(
                    RotateStreetlightsPosition(road.Position.X - 1, road.Position.Y, road.Position.Z + 1, road),
                    Type.StreetLight, streetLightDir);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private List<Vector3D<float>> GetRoadNeighbours(int x, int y)
    {
        List<Vector3D<float>> neighbours = [];

        var directions = new List<(int x, int y)>([(0, 1), (1, 0), (-1, 0), (0, -1)]);

        foreach (var direction in directions)
        {
            if (_layout[x + direction.x][y + direction.y].Name == "road")
            {
                var neighbourPosition = new Vector3D<float>(
                    _layout[x + direction.x][y + direction.y].X * 2,
                    0f,
                    _layout[x + direction.x][y + direction.y].Y * 2
                ) + _offsetToOrigin + _offsetHeightForCars;
                
                neighbours.Add(neighbourPosition);
            }
        }
        
        return neighbours;
    }

    public void SetUpObjects()
    {
        for (var i = 0; i < _layout.Count; i++)
        {
            for (var j = 0; j < _layout[i].Count; j++)
            {
                var position = new Vector3D<float>(_layout[i][j].X * 2, 0f, _layout[i][j].Y * 2) + _offsetToOrigin;
                var currentGrid = _layout[i][j];

                switch (currentGrid.Name)
                {
                    case "building":
                    {
                        var newBuilding = new GlBuilding(_gl, _layout[i][j].Type, position,
                            _layout[i][j].FacingDirection);
                        BuildingsManager.Instance.AddBuilding(newBuilding);
                        break;
                    }
                    case "road":
                    {
                        var newRoad = new GlRoad(_gl, _layout[i][j].Type, position, _layout[i][j].FacingDirection);
                        RoadsManager.Instance.AddRoad(newRoad);
                        
                        var neighbours = GetRoadNeighbours(i, j);
                        RoadsManager.Instance.RoadGraph.Add(position + _offsetHeightForCars, neighbours);

                        SetUpStreetlights(newRoad);

                        break;
                    }
                }
            }
        }
    }
}