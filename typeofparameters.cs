using System;
using System.ComponentModel;
using static Constants;
public class TypeOfParameters:INotifyPropertyChanged
{
 public event PropertyChangedEventHandler PropertyChanged;
 void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            //
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(theNameOfParameter)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(theValueOfParameter)));

    }
//свойство  theNumberOfTab привязывается к XAML коду на окне,чтобы отображать, и далее запоминать 
//на диске номер просматриваемой  вкладки 
//между сеансами работы
 int _theNumberOfTab = 1;
 public int theNumberOfTab
 {
  	get 
	{
		return _theNumberOfTab;
	}
	set 
	{
		 _theNumberOfTab = value;
		 OnPropertyChanged(nameof(theNumberOfTab));
	}
 }
    //при переходе на другую вкладку изменияется привязка между таймером и массивом значений,
    //демонстратор значения устанваливает новую привязку,для нового параметра, который демонстрируется 
    //на данной вкладке.
    public string theNameOfParameter
    {
        get
        {
            //выписываем имя демонструемого на данной вкладке параметра
            //используя ассоциативный массив TabControlData
                       
            return (TabControlData[_theNumberOfTab].parameterNameToDisplay);
        }
    }
    public string theValueOfParameter
    {
        get
        {
            //устанвыливаем привязку между таймером и демонстрируемым значением параметра
            //вернуть не значение, а привязку! 
            //Нетушки, там уже есть привязка,возвращающая значения!
            //Привязку надо устанавливать где-то в другом месте, но только не здесь!
            //Поставить обработчик где - нибудь к PropertyChanged и менять в нём привязку.
            //Или привязаться обработчиком к SelectedIndex и там менять.
            return (_theNumberOfTab == 1) ? "Ток" : "Very big number indeed";
      
        }
    }

}
