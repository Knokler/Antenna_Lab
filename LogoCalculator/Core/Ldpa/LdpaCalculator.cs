namespace LogoCalculator.Core.Ldpa
{
    
    /// <summary>
    /// Сервіс расчета классической Ldpa антенны.
    /// </summary>
    public class LdpaCalculator
    {
        /// <summary>
        /// Создание элемента
        /// </summary>
        /// <param name="input"></param>
        /// <param name="elementIndex">номер элемента</param>
        /// <param name="firstElementLengthMm">Длинна первого элемента</param>
        /// <param name="xCenterMm">растояние между центрами элементов</param>
        /// <returns></returns>
        private static LdpaElement CreateElement(
            LdpaInputParameters input,
            int elementIndex,
            double firstElementLengthMm,
            double xCenterMm)
                {
                    double fullLengthMm = firstElementLengthMm * Math.Pow(input.Tau, elementIndex);
                    double armLengthMm = fullLengthMm / 2.0;
                    double frequencyMHz = CalculateFrequencyMHz(fullLengthMm, input.VelocityFactor);
                    double spacingToNextMm = CalculateSpacingToNextMm(input.Sigma, fullLengthMm);
                    double widthMm = CalculateElementWidthMm(input, fullLengthMm, elementIndex);

                    return new LdpaElement
                    {
                        Index = elementIndex + 1,
                        FullLengthMm = fullLengthMm,
                        ArmLengthMm = armLengthMm,
                        FrequencyMHz = frequencyMHz,
                        SpacingToNextMm = spacingToNextMm,
                        XCenterMm = xCenterMm,
                        IsPhaseReversed = elementIndex % 2 == 1,
                        WidthMm = widthMm
                    };
        }
        /// <summary>
        /// Заполнение результата расчетов АНТЕННЫ
        /// </summary>
        /// <param name="result"></param>
        private static void FillResultSummary(LdpaInputParameters input,LdpaResult result)
        {
            result.LongestElementLengthMm = result.Elements.Max(element => element.FullLengthMm);
            result.ShortestElementLengthMm = result.Elements.Min(element => element.FullLengthMm);
            result.TotalWidthMm = result.LongestElementLengthMm;
            result.TotalLengthMm = result.Elements[^1].XCenterMm;
            result.AlphaDeg = CalculateAlphaDeg(input.Tau, input.Sigma);
            result.ApexAngleDeg = result.AlphaDeg * 2.0;

        }
        /// <summary>
        /// Расчет растояния между элементами
        /// </summary>
        /// <param name="sigma"></param>
        /// <param name="fullLengthMm">Полная длина элемента в мм</param>
        /// <returns>Растояние между элементами в мм</returns>
        private static double CalculateSpacingToNextMm(double sigma, double fullLengthMm)
        {
            return 2.0 * sigma * fullLengthMm;
        }
        /// <summary>
        /// Расчет частоты
        /// </summary>
        /// <param name="fullLengthMm">Полная длина элемента в мм</param>
        /// <param name="velocityFactor">Коэфицент укорочение длины волны на матерьяле</param>
        /// <returns>Частота в МГц </returns>
        private static double CalculateFrequencyMHz(double fullLengthMm, double velocityFactor)
        {
            double frequencyHz =
                SpeedOfLightMmPerSec * velocityFactor / (2.0 * fullLengthMm);

            return frequencyHz / 1_000_000.0;
        }
        /// <summary>
        /// Расчет первого элемента антенны
        /// </summary>
        /// <param name="input">Входные параметры</param>
        /// <returns>Длина элемента в мм</returns>
        private static double CalculateFirstElementLengthMm(LdpaInputParameters input)
        {
            double fMinHz = input.MinFrequencyMHz * 1_000_000.0;

            return SpeedOfLightMmPerSec * input.VelocityFactor / (2.0 * fMinHz);
        }
        /// <summary>
        /// Расчет угла раскрыва антенны в градусах на основе параметров tau и sigma.
        /// </summary>
        /// <param name="tau"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        private static double CalculateAlphaDeg(double tau, double sigma)
        {
            double alphaRad = Math.Atan((1.0 - tau) / (4.0 * sigma));
            return alphaRad*180.0 / Math.PI; // конвертация в градусы
        }
       /// <summary>
       /// Расчет длинны элементов
       /// </summary>
       /// <param name="input">Объект с параметрами антенны</param>
       /// <param name="fullLengthMm">Полная длина элемента в мм</param>
       /// <param name="elementIndex">Номер элемента</param>
       /// <returns></returns>
        private static double CalculateElementWidthMm(
            LdpaInputParameters input,
            double fullLengthMm,
            int elementIndex
            )
        { 
            double widthMm = input.WidthMode switch
            {
                ElementWidthMode.Constant => input.FirstElementWidthMm, 

                ElementWidthMode.ScaledByTau => input.FirstElementWidthMm * Math.Pow(input.Tau, elementIndex),

                ElementWidthMode.ProportionalToLength => fullLengthMm * input.ElementWidthRatio,

                ElementWidthMode.Custom => input.FirstElementWidthMm,

                _ => input.FirstElementWidthMm
            };

            return Math.Clamp(
                widthMm,
                input.MinElementWidthMm,
                input.MaxElementWidthMm
                );
        }
        /// <summary>
        /// Скорость света в мм/с
        /// </summary>
        private const double SpeedOfLightMmPerSec = 299_792_458_000.0; // скорость света в мм/с
        
        /// <summary>
        /// Блок расчета классической Ldpa антенны на основе входных параметров.
        /// </summary>
        /// <param name="input">Входные параметры для расчета Ldpa антенны.</param>
        /// <returns></returns>
        public LdpaResult Calculate(LdpaInputParameters input)
        { 
            ValidateInput(input);  // проверка входных параметров

            var result = new LdpaResult(); //создать объект результата
            double xCenterMm = 0.0; // TODO: расчет координаты центра элемента по оси X
            double firstElementLengthMm = CalculateFirstElementLengthMm(input); // посчитать первый элемент
            
            for (int i=0; i<input.ElementCount; i++) //создать остальные элементы на основе первого элемента и входных параметров
            {
                LdpaElement element = CreateElement(input,i,firstElementLengthMm,xCenterMm);

                result.Elements.Add(element);

                xCenterMm += element.SpacingToNextMm; // обновляем координату центра для следующего элемента
            }

            FillResultSummary(input,result); // заполнить результат расчетов

            AddWarnings(input, result); //добавить предупреждения

            return result; // вернуть результат
        }
        /// <summary>
        /// Метод проверки коректностии ввода параметров
        /// Вызывает исключение при не правильном вводе параметров
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidateInput(LdpaInputParameters input)
        { 
            if (input.MinFrequencyMHz <=0)
                throw new ArgumentException("Минимальная частота должна быть больше нуля.");
            
            if (input.MaxFrequencyMHz <= input.MinFrequencyMHz)
                throw new ArgumentException("Максимальная частота не должна быть меньще митнимальной");

            if (input.ElementCount < 2)
                throw new ArgumentException("Количество элементов должнобыть не меньше 2");

            if (input.Sigma <= 0)
                throw new ArgumentException("Sigma должен быть больше 0");

            if (input.MinElementWidthMm <= 0)
                throw new ArgumentException("Минимальный размер элемента должен быть больше 0");
            
            if(input.Tau <=0 || input.Tau >= 1)
                throw new ArgumentException("Tau должен быть в диапозоне от 0 < Tau < 1 "); // проверка диапозона тау
            
            if (input.VelocityFactor <= 0 || input.VelocityFactor > 1)
                throw new ArgumentException("Коэффициент укорочения должен быть в диапазоне 0 < K <= 1.");
        }

        private static void AddWarnings(LdpaInputParameters input, LdpaResult result) 
        {
            double highestCalculatedFrequency = result.Elements.Max(e => e.FrequencyMHz);

            if (highestCalculatedFrequency < input.MaxFrequencyMHz)
            {
                result.Warnings.Add("Количество элементов не достаточно для перекрытия верхней частоты диапозона.");
            }

            if (result.ShortestElementLengthMm < input.MinElementWidthMm * 5)
            {
                result.Warnings.Add("Самый короткий элемент слишком мал относительно минимальной технологической длинны.");
            }

            if (result.ApexAngleDeg > 60.0)
            {
                result.Warnings.Add("Геометричный угол раскрыва слишком большой. Возможны ухудшения направлености и согласования.");
            }
        }
        

    }
}
