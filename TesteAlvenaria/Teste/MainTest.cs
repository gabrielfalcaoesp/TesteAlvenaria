using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteAlvenaria.Core;
using System.IO;

namespace TesteAlvenaria.Teste;


internal class MainTest
{

    public static List<Wall> RunTest(string path)
    {
        DataProcessing dataProcessing = new DataProcessing();
        string[] conteudo = File.ReadAllLines(path);

        var walls = dataProcessing.BlockOrOpening(conteudo);

        

        return walls;

    }
      

}
