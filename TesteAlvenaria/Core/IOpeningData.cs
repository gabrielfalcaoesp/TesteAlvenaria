using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using TesteAlvenaria.Teste;

namespace TesteAlvenaria.Core;

public interface IOpeningData
{
    int WallPosition { get; }
    int Height { get; }
    int Length { get; }
    int Elevation { get; }
}

public class Opening : IOpeningData
{

    public int WallPosition { get; set; }
    public int Height { get; set; }
    public int Length { get; set; }
    public int Elevation { get; set; }

    public int Wall { get; set; }

    public Opening(int wallPosition, int height, int length, int elevation, int wall)
    {
        WallPosition = wallPosition;
        Height = height;
        Length = length;
        Elevation = elevation;
        Wall = wall;
    }
}

    public static class OpeningFilter
    {
    public static List<Opening> FilterValues(List<string> doors, string doorOrWindows)
    {
        List<Opening> listOpening = new List<Opening>();
        int elevation = 0;
        foreach (string blockString in doors)
        {
            int wall = DataProcessing.ExtrairValor(blockString, 5);
            int wallPosition = DataProcessing.ExtrairValor(blockString, 4);
            int length = DataProcessing.ExtrairValor(blockString, 1);
            int height = DataProcessing.ExtrairValor(blockString, 2);

            if(doorOrWindows == "Windows")
            {
                elevation = DataProcessing.ExtrairValor(blockString, 3);
            }

            else
            {
                wall = 0;
                wallPosition = DataProcessing.ExtrairValor(blockString, 3);
                elevation = 0;
            }
            Opening opening = new Opening(wallPosition, height, length, elevation, wall);

            listOpening.Add(opening);
        }

        return listOpening;
    }

}

    

  