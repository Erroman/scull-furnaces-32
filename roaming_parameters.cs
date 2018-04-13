using static Constants;
namespace ScullFurnaces_32
{
   partial class Scull_Furnaces_Main_Window
    {
        public void setParameterValueOnTheTab(AlarmEventArgs aea)
        {
            TabData tab = TabControlData[_typeOfParameters.theNumberOfTab];
            
            if ((tab.parameterType)==(ParameterType.дискретный))
            TimeMover.numberOfSecond = aea.TicksToAlarm;
            else
            //устанавливается значение аналогового параметра
            //при выборе вкладки или изменении значения таймера
            {
                switch (tab.parameterName)
                {
                    case ParameterName.Напряжение_дуги:
                        instantParameterValue.Content = "Напруга (В)";
                        break;
                    case ParameterName.Ток_общ:
                        instantParameterValue.Content = "Ток (А)";
                        break;
                    case ParameterName.Вакуум:
                        instantParameterValue.Content = "Вакуум (?)";
                        break;
                    case ParameterName.Расход_воды:
                        instantParameterValue.Content = "Расход воды (м3/сек)";
                        break;
                    default:
                        break;
                }                     
            }
        }
    }
}
