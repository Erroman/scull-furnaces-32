using System;
using static System.Console;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using static TransformWorldToScreen;

namespace ScullFurnaces_32
{
    
    partial class Scull_Furnaces_Main_Window
    {
        //Point lastMousePosition;
        //bool isTheLine = false;
        Path theYLine = null;
        void drawTheVerticalLine(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            { 
                Point clickPoint = e.GetPosition((Canvas)sender);
                doTheLineDrawing(sender, clickPoint);
                momentOfTime.clockWatch.Alarm_On = false;
                momentOfTime.clockWatch.Ticks = (int)DtoW(clickPoint).X;
                momentOfTime.clockWatch.Alarm_On = true;
                WriteLine("drawTheVerticalLine, the name of a sender: {0}", ((Canvas)sender).Name);
            }

        }
        void doTheLineDrawing(object sender, Point clickPoint)
        {
                LineGeometry myLineGeometry = new LineGeometry();
                myLineGeometry.StartPoint = new Point(clickPoint.X, ((Canvas)sender).ActualHeight-marginY);
                myLineGeometry.EndPoint = new Point(clickPoint.X, 0);

                Path myPath = new Path();
                myPath.Stroke = Brushes.Black;
                myPath.StrokeThickness = 0.5;
                myPath.Data = myLineGeometry;
                if(theYLine!=null) ((Canvas)sender).Children.Remove(theYLine);
                ((Canvas)sender).Children.Add(myPath);
                theYLine = myPath;

        }
    }
}
