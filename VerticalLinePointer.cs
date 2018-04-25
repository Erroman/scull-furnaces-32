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
        void putTheTimeByMouse(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Console.WriteLine(" void putTheTimeByMouse(object sender, MouseEventArgs e)");
                Point clickPoint = e.GetPosition((Canvas)sender);
                momentOfTime.clockWatch.Ticks = (int)DtoW(clickPoint).X;
            }

        }
        void doTheLineDrawing(object sender, Point clickPoint,TabData tab)
        {
                LineGeometry myLineGeometry = new LineGeometry();
                myLineGeometry.StartPoint = new Point(clickPoint.X, ((Canvas)sender).ActualHeight-marginY);
                myLineGeometry.EndPoint = new Point(clickPoint.X, 0);

                Path myPath = new Path();
                myPath.Stroke = Brushes.Black;
                myPath.StrokeThickness = 0.5;
                myPath.Data = myLineGeometry;
                if(tab.lineToShowTheCursor!=null) ((Canvas)sender).Children.Remove(tab.lineToShowTheCursor);
                ((Canvas)sender).Children.Add(myPath);
                tab.lineToShowTheCursor = myPath;

        }
        //Привязываемся к номеру вкладки, чтобы начертить на графике это йвкладки вертикальный курсор

    }
}
