#define OUTPUT_TO_THE_SCREEN
using System;
using System.IO;
using System.Windows.Forms;

class G
{

    static byte[] readValues;

    public static ParameterName parameterName = ParameterName.Напряжение_дуги;

    static void Main(string[] s)
    {
        if (s.Length > 1 && File.Exists(s[1])) Globals.fileName = s[1];
        readAllTheBytes(Globals.fileName);
        //В переменной readValues находится адрес массива в памяти с данными из файла

        if (!(s.Length > 0 && Enum.IsDefined(typeof(ParameterName), s[0]))) //по названию параметра не выходит
        {
            try
            {
                Int32 theEnteredNumber;
                //parameterNumber = (ParameterName)Int32.Parse(s[0]);
                bool itIsANumber = Int32.TryParse(s[0], out theEnteredNumber);
                //проверим с помощью TryParse,
                //возможно,введён номер параметра вместо его символьного наименования

                if (itIsANumber & Enum.IsDefined(typeof(ParameterName), theEnteredNumber))
                    parameterName = (ParameterName)theEnteredNumber;
                else
                {
                    parameterName = ParameterName.Напряжение_дуги;
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                //Если введено неверное название параметра	
                Console.WriteLine("Задайте имя(или № байта) параметра и исходный файл в командной строке!");
                Environment.Exit(0);
            }
        }

        else parameterName = (ParameterName)Enum.Parse(typeof(ParameterName), s[0]);
        Console.Write("Имя введённого параметра: {0} ", parameterName);
        Console.WriteLine("Тип параметра {0}", Constants.ParameterData[(int)(parameterName)].parameterType);

        if (s.Length > 1 && !File.Exists(s[1])) { Console.WriteLine("Отсутствует указанный файл параметров"); return; }
        if (s.Length > 2)
        {
            Console.WriteLine("Будем вводить стартовое время для выдачи параметра");
            try
            {
                Globals.startOfParameterOutput = TimeSpan.Parse(s[2]);
                Console.WriteLine("{0} --> {1}", s[2], Globals.startOfParameterOutput.ToString("c"));
            }
            catch (FormatException)
            {
                Console.WriteLine("{0}: Неправильный формат для начала времени выводв", s[2]);
            }
            catch (OverflowException)
            {
                Console.WriteLine("{0}: Overflow", s[2]);
            }

        }
        Globals.startTimeOfFileRecording = new DateTime(Int32.Parse(Globals.fileName.Substring(0, 4)), Int32.Parse(Globals.fileName.Substring(4, 2)), Int32.Parse(Globals.fileName.Substring(6, 2)));
        Globals.startTimeOfFileRecording += Globals.deltaForStartingOfRecord;
        Console.WriteLine("Start time of writing to the file: {0:D}", Globals.startTimeOfFileRecording);

        // Would be better new TickParam:IEnumerator(string Globals.fileName)

        try
        {
            int FileMarkerIndex = 0;
            for (int i = 0; i < Constants.ParamsBlockLengthInBytes; i++)
            {
                FileMarkerIndex = 2 * i;
                if (i != readValues[FileMarkerIndex]) throw new Exception();
                //FileMarkerIndex++;
            }
            if (readValues[FileMarkerIndex + 2] != 0xFF || readValues[FileMarkerIndex + 3] != 0xFB)
                throw new Exception();
        }
        catch
        {
            MessageBox.Show("Неверная полная запись в начале файла!");
        }
        //File O.K, let's read it	

        ReadParamFromDisk packedParameters = new ReadParamFromDisk(Globals.fileName);
        TickParamsAll unpackedParameters = new TickParamsAll(packedParameters);
#if OUTPUT_TO_THE_SCREEN
        Console.WriteLine("Output to the screen from the fully inflated file of parameters");
        //выдадим всё в файл!
        Console.WriteLine("Длина массива = {0}", unpackedParameters.inflatedParameters.Length);
        //FileStream fs = File.Open(Globals.fileName+".txt",FileMode.Create);
        File.WriteAllBytes(Globals.fileName + ".bin", unpackedParameters.inflatedParameters);
        string unitOfMeasure = (string)Constants.ParameterData[(int)(parameterName)].parameterUnit.ToString();
        DateTime realDateTime = Globals.startTimeOfFileRecording + TimeSpan.FromSeconds(Globals.startOfParameterOutput.TotalSeconds);
        short intAssembled;
        bool stepMode = true;
        Console.WriteLine("Нажмите любую клавишу для пошагового чтения или ENTER для непрерывной выдачи");
        for (
            int iParam = (int)parameterName / (Constants.ParameterData[(int)(parameterName)].parameterType == ParameterType.аналоговый ? 1 : 10) + (int)Globals.startOfParameterOutput.TotalSeconds * Constants.ParamsBlockLengthInBytes;

            iParam < unpackedParameters.inflatedParameters.Length;

            iParam = iParam + Constants.ParamsBlockLengthInBytes)
        {
            //Console.WriteLine("Номер ячейки = {0}",iParam);
            //обеспечиваем построчную выдачу на экран
            if (stepMode)
            { if (Console.ReadKey(true).Key == ConsoleKey.Enter) stepMode = !stepMode; }
            else
            if (Console.KeyAvailable && ((Console.ReadKey(true).Key == ConsoleKey.Enter) || (Console.ReadKey(true).Key == ConsoleKey.Spacebar))) stepMode = !stepMode;

            /*		
            for(int i=127;i<unpackedParameters.inflatedParameters.Length;i=i+Constants.ParamsBlockLengthInBytes)
            Console.WriteLine( unpackedParameters.inflatedParameters[i]); */
            //Выдача на экран построчно заданного в командной строке параметра из заданного файла с заданного момента 
            if (Constants.ParameterData[(int)(parameterName)].parameterType == ParameterType.аналоговый)
            {
                intAssembled = BitConverter.ToInt16(unpackedParameters.inflatedParameters, iParam);
                //Globals.swapBytes(ref intAssembled);
                switch (Constants.ParameterData[(int)(parameterName)].parameterUnit)
                {
                    case ParameterUnit._:
                        unitOfMeasure = "";
                        break;
                    case ParameterUnit.м3_ч:
                        unitOfMeasure = "м3/ч";
                        break;
                    case ParameterUnit.мм_рт_ст:
                        if (intAssembled < 0)
                        {
                            unitOfMeasure = "мм рт.ст";
                            intAssembled &= 0x777;
                        }
                        else
                            unitOfMeasure = "мк";
                        break;

                }
                Console.WriteLine("Время с начала суток: {0}  Значение {1} : {2:N1}{3}", realDateTime, parameterName, (float)intAssembled / 10, unitOfMeasure);

            }
            else
            {
                bool onOff = ((byte)Constants.ParameterData[(int)(parameterName)].bitMask & unpackedParameters.inflatedParameters[iParam]) > 0;
                unitOfMeasure = "";
                Console.WriteLine("Время с начала суток: {0}  Значение {1} : {2:N1}{3}", realDateTime, parameterName, onOff, unitOfMeasure);

            }
            realDateTime = realDateTime.AddSeconds(1.0);
        }

#endif
    }

    static void readAllTheBytes(string fileName)
    {
        /*	FileStream fs = File.Open(Globals.fileName,FileMode.Open);
            Console.WriteLine("CanRead = {0}, CanWrite = {1}",fs.CanRead,fs.CanWrite);
            //BinaryReader br = new BinaryReader(fs);
            Console.WriteLine("Length of the Stream: {0}",fs.Length);*/
        // Open the file
        using (var stream = new FileStream(fileName, FileMode.Open))
        using (var reader = new BinaryReader(stream))
        {
            // Compute how many values the file has
            //		Console.WriteLine("Length of the Stream: {0}",stream.Length);
            var numValues = stream.Length;

            // Allocate an array to hold all those values
            readValues = new byte[numValues];
            //readValues = reader.ReadBytes(numValues); //прочитать всё сразу!
            // Open a reader to make reading those values easy
            // Read all the values
            for (var i = 0; i < numValues; ++i)
            {
                readValues[i] = reader.ReadByte();
            }

        }

    }
}


