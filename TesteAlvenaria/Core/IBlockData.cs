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
    }

    public static class BlockFilter
    {
        public static List<Block> FilterValues(List<string> blocks)
        {
            List<Block> listBlocks = new List<Block>();

            foreach (string blockString in blocks)
            {
                int wallPosition = DataProcessing.ExtrairValor(blockString, 3);
                int length = DataProcessing.ExtrairValor(blockString, 1);


                Block block = new Block(wallPosition, length);

                listBlocks.Add(block);
                
            }

            return listBlocks;
        }
    }
}
