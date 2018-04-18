using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace ScullFurnaces_32
{
    partial class Scull_Furnaces_Main_Window
    {
        public TickParamsAll unpackedParameters; //данные для построения графика, инициализируются приложением, app.xaml.cs
        public Canvas theCanvasToDraw; //холст для отображения графика,инициализируется внутри метода, своего для каждого графика.
        const double marginX = 25;
        const double marginY = 25;
        const double dashTickLength = 10;
        const double dashHourTickLength = 10;
        const double dashHalfHourTickLength = 7;
        const double dashMinuteTickLength = 5;
        const double axisLineThickness = 1;
        double LowerLimitForTimeOnXAxis;
        double UpperLimitForTimeOnXAxis;
        


    }
}
