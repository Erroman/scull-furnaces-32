using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class TabData
{
	public ParameterName parameterName{get;set;}
    public ParameterType parameterType { get; set; }
    public string parameterNameToDisplay { get; set; }
    public Canvas canvasForDiplayedPlot { get; set; }
    public Path lineToShowTheCursor { get; set; }
    
}

