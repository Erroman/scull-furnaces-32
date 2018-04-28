using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static Constants;
using static System.Console;
using static TransformWorldToScreen;
namespace ScullFurnaces_32
{
    class VisualHostForVacuumPlot : FrameworkElement
    {
        private static Scull_Furnaces_Main_Window scull_Furnaces_Main_Window = ((Scull_Furnaces_AppMain_Class)Application.Current).scull_Furnaces_Main_Window;
        private readonly VisualCollection _children;
        private readonly TickParamsAll unpackedParameters;
        private readonly ParameterName parameterName;
        private readonly Rect rectBounds;
        public VisualHostForVacuumPlot(TickParamsAll unpackedParameters, ParameterName parameterName, Rect rect)
        {
            //Основные параметры для рисования графика:
            this.unpackedParameters = unpackedParameters; //массив, содержащий исходные данные
            this.parameterName = parameterName; //выбранный параметр для построения графика
            this.rectBounds = rect;           //прямоугольник,ограничивающий место на экране для графика
                                              //внутри уже нарисованных осей координат
            _children = new VisualCollection(this)
        {
            CreateDrawingVisualPlot()
        };
        }
        private DrawingVisual CreateDrawingVisualPlot()
        {
            //const double dashTickLength = 10;
            double xmin = rectBounds.X;
            double xmax = rectBounds.Width;
            double ymin = rectBounds.Y;
            double ymax = rectBounds.Height;
            double LowerLimitForVoltageOnYAxis = 0;
            double UpperLimitForVoltageOnYAxis = Convert.ToDouble(scull_Furnaces_Main_Window.maxValueForVoltageOnYAxis.Text);

            double LowerLimitForTimeOnXAxis = Int32.Parse(scull_Furnaces_Main_Window.begTimeForVoltageOnXAxis.Ticks.Text);
            double UpperLimitForTimeOnXAxis = Int32.Parse(scull_Furnaces_Main_Window.endTimeForVoltageOnXAxis.Ticks.Text);
            double step = Math.Round((xmax - xmin) / (24 * 6));
            PrepareTransformations
            (
            LowerLimitForTimeOnXAxis, UpperLimitForTimeOnXAxis,
            LowerLimitForVoltageOnYAxis, UpperLimitForVoltageOnYAxis,
            xmin, xmax,
            ymin, ymax
            );
            int intSecondsPerDot = (int)((UpperLimitForTimeOnXAxis - LowerLimitForTimeOnXAxis) / (xmax - xmin));

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            Pen pen = new Pen(Brushes.Blue, 1.0);
            int numSec = 0;
            short intAssembled;
            double realValueOfFunction;
            double peakAtInterval = Int32.MinValue;   //максимальное значение функции на интервале
            double bottomAtInterval = Int32.MaxValue; //минимальное значение функции на интервале
            Point begPoint = WtoD(new Point(xmin, ymax)); //What should be put here?!
            Point endPoint; // = WtoD(new Point(xmin,ymax));
            WriteLine("LowerLimitForCurrentOnYAxis = {0}", LowerLimitForVoltageOnYAxis);
            int iParam;
            for
            (
            int currSec = (int)LowerLimitForVoltageOnYAxis;
            currSec < (UpperLimitForTimeOnXAxis - LowerLimitForTimeOnXAxis);
            currSec++
            )
            {
                iParam = (int)parameterName + currSec * Constants.ParamsBlockLengthInBytes;

                if (Constants.ParameterData[(int)(parameterName)].parameterType == ParameterType.аналоговый)
                {

                    intAssembled = BitConverter.ToInt16(unpackedParameters.inflatedParameters, iParam);
                    realValueOfFunction = (double)intAssembled / 10;
                }
                else
                    throw new Exception();

                if (peakAtInterval < realValueOfFunction) peakAtInterval = realValueOfFunction;
                if (bottomAtInterval > realValueOfFunction) bottomAtInterval = realValueOfFunction;
                if (numSec % intSecondsPerDot == 0)  //intSecondsPerDot обращается в 0!
                {
                    //отмечать в графике МАКСИМАЛЬЕОЕ значение на интервале
                    //но надо отмечать и МИНИМАЛЬНОЕ !

                    endPoint = WtoD(new Point(currSec, (bottomAtInterval + peakAtInterval) / 2));
                    drawingContext.DrawLine(pen, begPoint, endPoint);

                    begPoint = WtoD(new Point(currSec, bottomAtInterval));
                    endPoint = WtoD(new Point(currSec, peakAtInterval));
                    drawingContext.DrawLine(pen, begPoint, endPoint);

                    begPoint = new Point((begPoint.X + endPoint.X) / 2, (begPoint.Y + endPoint.Y) / 2);

                    peakAtInterval = 0;
                    bottomAtInterval = Int32.MaxValue;
                }
                numSec++;
                //if(numSec>15000)break;		

            }

            drawingContext.Close();
            return drawingVisual;
        }
        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount => _children.Count;

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }
    }
}