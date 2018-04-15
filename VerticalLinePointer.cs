using System;
using static System.Console;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;

namespace ScullFurnaces_32
{
    partial class Scull_Furnaces_Main_Window
    {
        void putTheVerticalLine(object sender, MouseButtonEventArgs e)
        {
            WriteLine("Hello, I am Mouse!");
            LineGeometry myLineGeometry = new LineGeometry();
            Point clickPoint = e.GetPosition((Canvas)sender);
            myLineGeometry.StartPoint = new Point(clickPoint.X, ((Canvas)sender).ActualHeight);
            myLineGeometry.EndPoint = new Point(clickPoint.X, 0);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 0.5;
            myPath.Data = myLineGeometry;
            ((Canvas)sender).Children.Add(myPath);
        }
    }
}
