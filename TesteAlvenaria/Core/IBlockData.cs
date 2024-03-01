using System;
using System.Collections.Generic;
using TesteAlvenaria.Teste; 

namespace TesteAlvenaria.Core
{
    public interface IBlockData
    {
        int WallPosition { get; set; }
        int Height { get; }
        int Length { get; }
        int Elevation { get; }
    }

    public class Block : IBlockData
    {
        public int WallPosition { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public int Elevation { get; set; }

        public Block(int wallPosition, int length)
        {
            WallPosition = wallPosition;
            Length = length;
            Height = 20;
            Elevation = 0;
        }

        public Block(int wallPosition, int length, int elevation)
        {
            WallPosition = wallPosition;
            Length = length;
            Height = 20;
            Elevation = elevation;
        }
    }

    public static class BlockFilter
    {
        public static List<Block> FilterValues(List<string> blocks)
        {
            int elevationIncrement = 20;
            int maxElevation = 260;
            int currentElevation = 0;

            List<Block> listBlocks = new List<Block>();
            List<Block> listBlocksElevation = new List<Block>();


            foreach (string blockString in blocks)
            {
                int wallPosition = DataProcessing.ExtrairValor(blockString, 3);
                int length = DataProcessing.ExtrairValor(blockString, 1);

                Block block = new Block(wallPosition, length);
                listBlocks.Add(block);
                
            }


            
            while (currentElevation < maxElevation)
            {
                List<Block> newBlocks = new List<Block>();

                foreach (Block block in listBlocks)
                {

                    int wallPosition = block.WallPosition;
                    int length = block.Length;
                    int newElevation = currentElevation;

                    Block newBlock = new Block(wallPosition, length, newElevation);
                    newBlocks.Add(newBlock);

                }

                listBlocksElevation.AddRange(newBlocks);
                currentElevation += elevationIncrement;
            }

            return listBlocksElevation;
            
        }

        public static void IncrementWallPosition(List<Block> newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                Block block = newList[i];
                if (block.Elevation % 20 == 0 && (block.Elevation / 20) % 2 == 1)
                {
                    newList[i].WallPosition += 20;
                }
            }
        }

        public static void IncrementWallPositionPair(List<Block> listBlocksElevation)
        {
            foreach (Block block in listBlocksElevation)
            {
                if (block.Elevation % 20 == 0 && (block.Elevation / 20) % 2 == 1)
                {
                    block.WallPosition -= 20;
                }
            }
        }



    }
}
