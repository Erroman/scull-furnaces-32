using System;
using System.ComponentModel;
using static Constants;
public class TypeOfParameters:INotifyPropertyChanged
{
 public event PropertyChangedEventHandler PropertyChanged;
 void OnPropertyChanged(string propertyName)
    {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
}
