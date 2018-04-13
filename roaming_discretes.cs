using System;
using System.Windows;
using System.Windows.Controls;
using static Constants;
namespace ScullFurnaces_32
{
public enum TimeUnit
{
	second,
	minute,
	hour
}
    //Устанавливает состояние панели с дискретами а заданный момент времени с начала суток,
    //записываемый в свойство numberOfSecond
    public static class TimeMover
{
	private static Scull_Furnaces_Main_Window scull_Furnaces_Main_Window = ((Scull_Furnaces_AppMain_Class)Application.Current).scull_Furnaces_Main_Window;
	private static TickParamsAll unpackedParameters;
	private static int _numberOfSecond = 0;
	
	public static event EventHandler numberOfSecondChanged; //не используется

    public static double Voltage { get; set;}
    public static double Current { get; set;}
    public static double Vacuum  { get; set;}
    public static double Water   { get; set;}

    public static int numberOfSecond 
	{
		get
		{
			return _numberOfSecond;
		}
		set
		{
			if(value>=0 & value<SecondsInADay)
			{
				_numberOfSecond = value;
                setAllTheDiscretePlaquesOnTheTab();
                    //set voltage,current,vacuum and water consumption at the moment
                    Console.WriteLine("Секунда для дискретов: {0}", _numberOfSecond);

            }
            if (numberOfSecondChanged != null) numberOfSecondChanged(null, EventArgs.Empty);

            void setAllTheDiscretePlaquesOnTheTab()
            {
                    unpackedParameters = ((Scull_Furnaces_AppMain_Class)Application.Current).unpackedParameters;
                    foreach (var buttonControl in scull_Furnaces_Main_Window.uniGrid.Children)
                    {
                        //по номеру секунды и номеру параметра выбираем нужный байт в памяти и маску для него
                        //1. Байт в памяти,содержащий значание данного дискретного параметра с номером (ключём)
                        //((DiscretePlaque)buttonControl).discreteNumber
                        //WriteLine(((DiscretePlaque)buttonControl).discreteNumber/10);
                        //WriteLine(ParameterData[((DiscretePlaque)buttonControl).discreteNumber].parameterName);
                        byte byteWithDiscretes = unpackedParameters.inflatedParameters[(_numberOfSecond) * Constants.ParamsBlockLengthInBytes + ((DiscretePlaque)buttonControl).discreteNumber / 10];
                        ((ColorSource)Constants.ParameterData[((DiscretePlaque)buttonControl).discreteNumber].colorBinding.Source).parameterState =
                        (byte)Constants.ParameterData[((DiscretePlaque)buttonControl).discreteNumber].bitMask & byteWithDiscretes;    // !!!!!!!!!!!!!!!

                    }

             }
                
			}
		
		}
	}
    

public class DiscretePlaque:Button
{
	 public int discreteNumber;
}

}
