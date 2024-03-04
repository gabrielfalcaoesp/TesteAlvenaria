using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using TesteAlvenaria.Teste;

namespace TesteAlvenaria.Core;

internal interface IWallData
{
    string Name { get; }
    int PointX { get; }
    int PointY { get; }
    int Angle { get; }
    int Length { get; }
    List<Block> Blocks { get; }
    List<Opening> Openings { get; }
}


public class Wall : IWallData
{
    public string Name { get; }
    public int PointX { get; }
    public int PointY { get; }
    public int Angle { get; }
    public int Length { get; }
    public List<Block> Blocks { get; } = new List<Block>();
    public List<Opening> Openings { get; } = new List<Opening>();

    public Wall(string name, int pointX, int pointY, int angle, int length, List<Block> blocks, List<Opening> openings)
    {
        Name = name;
        PointX = pointX;
        PointY = pointY;
        Angle = angle;
        Length = length;
        Blocks = blocks;
        Openings = openings;
    }
}

public static class WallFilter
{
    public static List<Wall> FilterValues(List<Block> blocks, Dictionary<int, List<string>> paredes, List<Opening> listWindows, List<Opening> listDoors)
    {
        List<Wall> listWall = new List<Wall>();
        int minValueBlock = 0;
        int maxValueBlock = 0;
        foreach (KeyValuePair<int, List<string>> parede in paredes)
        {
            string name = "parede " + parede.Key;
            int pointX = parede.Value.Min(s => DataProcessing.ExtrairValor(s, 2));
            int angle = parede.Value.Min(s => DataProcessing.ExtrairValor(s, 4));
            int pointY = 0;
            int length = parede.Value.Max(s => DataProcessing.ExtrairValor(s, 3) + parede.Value.Max(s => DataProcessing.ExtrairValor(s, 1)));

            if (angle == 00)
            {
                pointX = 0;
                pointY = parede.Value.Min(s => DataProcessing.ExtrairValor(s, 2));
                Console.WriteLine(pointY);
            }


            maxValueBlock = maxValueBlock + parede.Value.Count;
            List<Block> listBlocks = blocks.GetRange(minValueBlock, maxValueBlock - minValueBlock);

            List<Block> newList = CreateBlocksElevation(listBlocks);
            listBlocks.AddRange(newList);
            if(parede.Key % 2 != 0)
            {
                BlockFilter.IncrementWallPosition(listBlocks);
                List<Block> removedBlocks = BlockFilter.RemoveLastBlock(listBlocks);
                listBlocks.RemoveAll(b => removedBlocks.Contains(b));
            }
            else
            {
                BlockFilter.IncrementWallPositionPair(listBlocks);
                List<Block> addBlocks = BlockFilter.AddLastBlock(listBlocks);
                listBlocks.AddRange(addBlocks);
            }
            

            minValueBlock = maxValueBlock;

            List<Opening> openings = new List<Opening>();
            for (int i = 0; i < listWindows.Count; i++)
            {
                if (listWindows[i].Wall == pointY)
                {
                    openings.Add(listWindows[i]);
                }
            }
            Console.WriteLine(openings);
            for (int i = 0; i < listDoors.Count; i++)
            {
                if (listDoors[i].WallPosition == pointX)
                {
                    openings.Add(listDoors[i]);
                }
            }

            List<Block> blocksShouldExcluded = BlockFilter.ExcludeBlocksOfOpening(openings, listBlocks);
            listBlocks.RemoveAll(b => blocksShouldExcluded.Contains(b));


            Wall wall = new Wall(name, pointX, pointY, angle, length, listBlocks, openings);
            listWall.Add(wall);


        }

        return listWall;
    }

    public static List<Block> CreateBlocksElevation(List<Block> listBlocksElevation)
    {
        List<Block> updatedBlocks = new List<Block>();

        foreach (Block block in listBlocksElevation)
        {
            for (int i = 0; i < 13; i++)
            {
                Block updatedBlock = new Block(
                    wallPosition: block.WallPosition,
                    length: block.Length,
                    elevation: block.Elevation + 20 * i 
                );

                updatedBlocks.Add(updatedBlock);
            }
        }

        return updatedBlocks;
    }


}